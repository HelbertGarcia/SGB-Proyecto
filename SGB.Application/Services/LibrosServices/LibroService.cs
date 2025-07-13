using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Validators.BusinessValidators;
using SGB.Domain.Base;
using SGB.Domain.Entities.Categoria;
using SGB.Domain.Entities.Libro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGB.Application.Services.LibrosServices
{
    public class LibroService : ILibroService
    {
        private readonly ILibroBusinessValidator _libroValidator;
        private readonly ILibroRepository _libroRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ICategoriaRepository _categoriaRepository; // La dependencia que faltaba
        private readonly ILogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        // --- CONSTRUCTOR CORREGIDO Y COMPLETO ---
        // Ahora sí incluye todas las dependencias necesarias.
        public LibroService(
            ILibroRepository libroRepository,
            ILibroBusinessValidator libroValidator,
            IPrestamoRepository prestamoRepository,
            ICategoriaRepository categoriaRepository, // Se añade como parámetro
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _libroRepository = libroRepository;
            _libroValidator = libroValidator;
            _prestamoRepository = prestamoRepository;
            _categoriaRepository = categoriaRepository; // Se inicializa correctamente
            _logger = loggerFactory.CreateLogger<LibroService>();
            _configuration = configuration;
        }

        #region Implementación de IBaseService

        public async Task<OperationResult<LibroDto>> AddAsync(AddLibroDto dto)
        {
            try
            {
                if (dto is null)
                    return OperationResult<LibroDto>.Failure("Datos nulos.");

                var resultadoExistencia = await _libroRepository.BuscarPorIsbnAsync(dto.ISBN);
                if (resultadoExistencia.IsSuccess && resultadoExistencia.Data != null)
                    return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Libros:IsbnAlreadyExists"]);

                // Esta línea ahora funcionará porque _categoriaRepository está inicializado.
                var categoriaResult = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
                if (!categoriaResult.IsSuccess || categoriaResult.Data == null)
                    return OperationResult<LibroDto>.Failure("La categoría especificada no existe.");

                var libroEntidad = new Libro(dto.ISBN, dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.AddAsync(libroEntidad);
                if (!repoResult.IsSuccess)
                    return OperationResult<LibroDto>.Failure(repoResult.Message);

                var libroCreado = repoResult.Data;
                var libroCreadoDto = new LibroDto
                {
                    Id = libroCreado.Id,
                    ISBN = libroCreado.ISBN,
                    Titulo = libroCreado.Titulo,
                    Autor = libroCreado.Autor,
                    Editorial = libroCreado.Editorial,
                    FechaPublicacion = libroCreado.FechaPublicacion,
                    NombreCategoria = categoriaResult.Data.Nombre,
                    Estado = "Disponible",
                    FechaRegistro = libroCreado.FechaRegistro
                };
                return OperationResult<LibroDto>.Success(libroCreadoDto, "Libro creado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el libro con ISBN: {ISBN}", dto?.ISBN);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<LibroDto>> UpdateAsync(int id, UpdateLibroDto dto)
        {
            try
            {
                // 1. DELEGA la validación de negocio.
                var validationResult = await _libroValidator.ValidateForUpdateAsync(id, dto);
                if (!validationResult.IsSuccess)
                    return OperationResult<LibroDto>.Failure(validationResult.Message);

                var libroEntidad = validationResult.Data;
                libroEntidad.ActualizarDetalles(dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.UpdateAsync(libroEntidad);
                if (!repoResult.IsSuccess)
                    return OperationResult<LibroDto>.Failure(repoResult.Message);

                // Después de actualizar, devolvemos el DTO completo.
                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al actualizar libro con ID: {ID}", id);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                // 1. DELEGA la validación de negocio.
                var validationResult = await _libroValidator.ValidateForDeleteAsync(id);
                if (!validationResult.IsSuccess)
                    return validationResult;

                // 2. Si la validación pasa, ACTÚA.
                return await _libroRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al eliminar libro con ID: {ID}", id);
                return OperationResult<bool>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }


        public async Task<OperationResult<LibroDto>> GetByIdAsync(int id)
        {
            // Llama al método del repositorio que ya devuelve el DTO de forma eficiente.
            return await _libroRepository.ObtenerDetallesDTOPorIdAsync(id);
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> GetAllAsync()
        {
            // Llama al método del repositorio que devuelve la lista de DTOs de forma eficiente.
            return await _libroRepository.ObtenerTodosConDetallesAsync();
        }

        #endregion

        #region Implementación de ILibroService (Métodos Específicos)

        // --- Implementación de los métodos específicos de ILibroService ---

        public async Task<OperationResult<LibroDto>> BuscarPorIsbnAsync(string isbn)
        {
            try
            {
                // 1. Llama al método correcto del repositorio. Este devuelve OperationResult<Libro>.
                var resultadoRepo = await _libroRepository.BuscarPorIsbnAsync(isbn);

                // 2. Comprueba si la operación del repositorio falló o si no se encontraron datos.
                if (!resultadoRepo.IsSuccess)
                {
                    return OperationResult<LibroDto>.Failure(resultadoRepo.Message);
                }
                if (resultadoRepo.Data == null)
                {
                    return OperationResult<LibroDto>.Success(null, "Libro no encontrado.");
                }

                // 3. Si se encontró la entidad, el servicio hace el trabajo de mapeo.
                var libroEntidad = resultadoRepo.Data;
                var categoriaResult = await _categoriaRepository.GetByIdAsync(libroEntidad.IDCategoria);

                var libroDto = new LibroDto
                {
                    Id = libroEntidad.Id,
                    ISBN = libroEntidad.ISBN,
                    Titulo = libroEntidad.Titulo,
                    Autor = libroEntidad.Autor,
                    Editorial = libroEntidad.Editorial,
                    FechaPublicacion = libroEntidad.FechaPublicacion,
                    NombreCategoria = categoriaResult.Data?.Nombre ?? "Desconocida",
                    Estado = libroEntidad.EstaActivo ? "Disponible" : "Inactivo",
                    FechaRegistro = libroEntidad.FechaRegistro
                };

                return OperationResult<LibroDto>.Success(libroDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al buscar libro por ISBN: {ISBN}", isbn);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> BuscarLibrosAsync(string terminoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                return await GetAllAsync();
            }

            try
            {
                // 1. Se crea el filtro para la búsqueda.
                Expression<Func<Libro, bool>> filtro = l =>
                    (l.Titulo.Contains(terminoBusqueda) || l.Autor.Contains(terminoBusqueda))
                    && l.EstaActivo;

                // 2. Se llama al método genérico del repositorio.
                var resultadoRepo = await _libroRepository.FindByConditionAsync(filtro);
                if (!resultadoRepo.IsSuccess)
                {
                    return OperationResult<IEnumerable<LibroDto>>.Failure(resultadoRepo.Message);
                }

                // 3. El servicio se encarga de mapear la lista de entidades a una lista de DTOs.
                //    (Esta es la parte ineficiente que discutimos, pero es correcta según la interfaz actual).
                var librosEncontrados = resultadoRepo.Data;
                var listaDto = new List<LibroDto>();

                foreach (var libro in librosEncontrados)
                {
                    var categoria = await _categoriaRepository.GetByIdAsync(libro.IDCategoria);
                    listaDto.Add(new LibroDto
                    {
                        Id = libro.Id,
                        ISBN = libro.ISBN,
                        Titulo = libro.Titulo,
                        Autor = libro.Autor,
                        Editorial = libro.Editorial,
                        FechaPublicacion = libro.FechaPublicacion,
                        NombreCategoria = categoria.Data?.Nombre ?? "Desconocida",
                        Estado = libro.EstaActivo ? "Disponible" : "Inactivo",
                        FechaRegistro = libro.FechaRegistro
                    });
                }

                return OperationResult<IEnumerable<LibroDto>>.Success(listaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar libros con el término: {Termino}", terminoBusqueda);
                return OperationResult<IEnumerable<LibroDto>>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        #endregion
    }
}