using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Application.Services.LibrosServices
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        public LibroService(
            ILibroRepository libroRepository,
            ICategoriaRepository categoriaRepository,
            IPrestamoRepository prestamoRepository,
            ILogger<LibroService> logger,
            IConfiguration configuration)
        {
            _libroRepository = libroRepository;
            _categoriaRepository = categoriaRepository;
            _prestamoRepository = prestamoRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> AddLibroAsync(AddLibroDto addLibroDto)
        {
            try
            {
                if (addLibroDto is null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:InvalidInput"] ?? "Los datos de entrada no pueden ser nulos." });
                }

                var resultadoExistencia = await _libroRepository.BuscarPorIsbnAsync(addLibroDto.ISBN);
                if (resultadoExistencia.Success && resultadoExistencia.Data is IEnumerable<Libro> lista && lista.Any())
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Libros:IsbnAlreadyExists"] });
                }

                var categoriaResult = await _categoriaRepository.GetByIdAsync(addLibroDto.IDCategoria);
                if (categoriaResult == null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = "La categoría especificada no existe." });
                }

                var libroEntidad = new Libro(
                    addLibroDto.ISBN,
                    addLibroDto.Titulo,
                    addLibroDto.Autor,
                    addLibroDto.Editorial,
                    addLibroDto.FechaPublicacion,
                    addLibroDto.IDCategoria
                );
                libroEntidad.Habilitar();

                var resultadoRepo = await _libroRepository.AddAsync(libroEntidad);
                if (!resultadoRepo.Success)
                {
                    return resultadoRepo;
                }

                var libroCreadoDto = new LibroDto
                {
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoriaResult.Nombre,
                    Estado = libroEntidad.EstaActivo ? "Disponible" : "Inactivo",
                    FechaRegistro = libroEntidad.FechaRegistro
                };

                return new OperationResult { Success = true, Data = libroCreadoDto, Message = "Libro creado exitosamente." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el libro con ISBN: {ISBN}", addLibroDto?.ISBN);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetLibroDetailsAsync(string isbn)
        {
            try
            {
                var resultadoRepo = await _libroRepository.BuscarPorIsbnAsync(isbn);

                if (!resultadoRepo.Success)
                {
                    return resultadoRepo; 
                }

                var librosEncontrados = (IEnumerable<Libro>)resultadoRepo.Data;
                var libroEntidad = librosEncontrados.FirstOrDefault(); 
                if (libroEntidad == null)
                {
                    return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] };
                }

                var categoria = await _categoriaRepository.GetByIdAsync(libroEntidad.IDCategoria);
                var libroDto = new LibroDto
                {
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoria?.Nombre ?? "Desconocida", // El '?' evita un error si la categoría es nula
                    Estado = libroEntidad.EstaActivo ? "Disponible" : "Inactivo",
                    FechaRegistro = libroEntidad.FechaRegistro
                };

                return new OperationResult { Success = true, Data = libroDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener detalles del libro con ISBN: {ISBN}", isbn);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetAllLibrosAsync()
        {
            try
            {
                var librosDesdeRepo = await _libroRepository.GetAllAsync();

                var listaDto = librosDesdeRepo.Select(libro => new LibroDto
                {
                    ISBN = libro.ISBN,
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    Estado = libro.EstaActivo ? "Disponible" : "Inactivo"
                }).ToList();

                return new OperationResult { Success = true, Data = listaDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de libros.");
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Libros:GetAll"] };
            }
        }

        public async Task<OperationResult> UpdateLibroAsync(string isbn, UpdateLibroDto libroDto)
        {
            try
            {
                //--- CORRECCIÓN 1: Se llama al método especializado para obtener una entidad "trackeada".
                // Este método devuelve un único objeto 'Libro', no una lista.
                var libroEntidad = await _libroRepository.ObtenerParaActualizacionAsync(isbn);

                // 1. Validación: Verificar que el libro exista.
                if (libroEntidad == null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:ResourceNotFound"] });
                }

                // 2. Validación: Verificar que la nueva categoría exista.
                var categoriaResult = await _categoriaRepository.GetByIdAsync(libroDto.IDCategoria);
                if (categoriaResult == null)
                {
                    return await Task.FromResult(new OperationResult { Success = false, Message = "La nueva categoría especificada no existe." });
                }

                // 3. Lógica de Negocio: Se usa el método de la entidad para actualizar sus datos.
                libroEntidad.ActualizarDetalles(
                    libroDto.Titulo,
                    libroDto.Autor,
                    libroDto.Editorial,
                    libroDto.FechaPublicacion,
                    libroDto.IDCategoria
                );

                // 4. Persistencia: Se llama al Update del repositorio.
                var resultadoRepo = await _libroRepository.UpdateAsync(libroEntidad);

                if (!resultadoRepo.Success)
                {
                    return resultadoRepo; // Propagar el error del repositorio
                }

                // 5. Respuesta: Se mapea la entidad actualizada a un DTO de respuesta completo.
                var libroActualizadoDto = new LibroDto
                {
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoriaResult.Nombre,
                    Estado = libroEntidad.EstaActivo ? "Disponible" : "Inactivo",
                    FechaRegistro = libroEntidad.FechaRegistro
                };

                return new OperationResult { Success = true, Data = libroActualizadoDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el libro con ISBN: {ISBN}", isbn);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> DeleteLibroAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "El ISBN no puede estar vacío." });
            }

            try
            {
                // 2. Validación de Negocio: Verificar si el libro tiene préstamos activos.
                // Se asume que la propiedad en la entidad 'Prestamo' que guarda el ISBN es 'LibroISBN'.
                // También se compara contra el enum 'EstadoPrestamo.Activo' para mayor seguridad.
                var prestamosActivosResult = await _prestamoRepository.FindByConditionAsync(p => p.EjemplarId == isbn && p.Estado == EstadoPrestamo.Activo);

                // Primero, siempre se debe verificar si la operación del repositorio tuvo éxito.
                if (!prestamosActivosResult.Success)
                {
                    _logger.LogWarning("La búsqueda de préstamos activos falló antes de la eliminación del libro.");
                    return prestamosActivosResult; // Propagamos el error del repositorio.
                }

                //-- CORRECCIÓN: Se simplifica la comprobación de la lista.
                // Se convierte la propiedad 'Data' a una colección de la entidad correcta
                // y se usa .Any() de LINQ para ver si tiene elementos.
                if (prestamosActivosResult.Data is IEnumerable<Prestamo> listaPrestamos && listaPrestamos.Any())
                {
                    // Si la lista tiene al menos un préstamo, no se puede eliminar el libro.
                    return await Task.FromResult(new OperationResult { Success = false, Message = _configuration["ErrorMessages:Libros:BookIsOnLoan"] });
                }

                // 3. Si no hay préstamos activos, se procede con la eliminación lógica.
                return await _libroRepository.DeleteLogicoAsync(isbn);
            }
            catch (Exception ex)
            {
                // Este bloque captura cualquier otro error inesperado durante el proceso.
                _logger.LogError(ex, "Error en el servicio al eliminar el libro con ISBN: {ISBN}", isbn);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }

        public async Task<OperationResult> GetLibrosAsync(string terminoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                return await GetAllLibrosAsync();
            }

            try
            {
                Expression<Func<Libro, bool>> filtro = l =>
                    (l.Titulo.Contains(terminoBusqueda) || l.Autor.Contains(terminoBusqueda))
                    && l.EstaActivo;

                var resultadoRepo = await _libroRepository.FindByConditionAsync(filtro);

                if (!resultadoRepo.Success)
                {
                    return resultadoRepo;
                }

                var librosEncontrados = (IEnumerable<Libro>)resultadoRepo.Data;
                var listaDto = librosEncontrados.Select(libro => new LibroDto
                {
                    ISBN = libro.ISBN,
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    Estado = libro.EstaActivo ? "Disponible" : "Inactivo"
                }).ToList();

                return new OperationResult { Success = true, Data = listaDto };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al buscar libros con el término: {Termino}", terminoBusqueda);
                return new OperationResult { Success = false, Message = _configuration["ErrorMessages:Global:UnexpectedError"] };
            }
        }
    }
}
