using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGB.Application.Contracts.Repository.Interfaces;
using SGB.Application.Contracts.Service.ILibroServices;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Validators.BusinessValidators;
using SGB.Domain.Base;
using SGB.Domain.Entities.Libro;
using SGB.Domain.Entities.Prestamos;
using System.Linq.Expressions;

namespace SGB.Application.Services.LibrosServices
{
    public class LibroService : ILibroService
    {
        private readonly ILibroBusinessValidator _libroValidator;
        private readonly ILibroRepository _libroRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ICategoriaRepository _categoriaRepository; 
        private readonly ILogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        public LibroService(
        ILibroRepository libroRepository,
        IPrestamoRepository prestamoRepository,
        ICategoriaRepository categoriaRepository,
        ILogger<LibroService> logger, 
        IConfiguration configuration)
        {
            _libroRepository = libroRepository;
            _prestamoRepository = prestamoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = logger; 
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
                var libroEntidad = await _libroRepository.ObtenerParaActualizacionAsync(id);
                if (libroEntidad == null)
                {
                    //-- CORRECCIÓN: Se añade un mensaje por defecto con '??'
                    var errorMessage = _configuration["ErrorMessages:Global:ResourceNotFound"] ?? "El libro no fue encontrado.";
                    return OperationResult<LibroDto>.Failure(errorMessage);
                }

                var categoriaResult = await _categoriaRepository.GetByIdAsync(dto.IDCategoria);
                if (!categoriaResult.IsSuccess || categoriaResult.Data == null)
                {
                    return OperationResult<LibroDto>.Failure("La nueva categoría especificada no existe.");
                }

                libroEntidad.ActualizarDetalles(dto.Titulo, dto.Autor, dto.Editorial, dto.FechaPublicacion, dto.IDCategoria);

                var repoResult = await _libroRepository.UpdateAsync(libroEntidad);
                if (!repoResult.IsSuccess)
                    return OperationResult<LibroDto>.Failure(repoResult.Message);

                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el libro con ID: {ID}", id);
                return OperationResult<LibroDto>.Failure(_configuration["ErrorMessages:Global:UnexpectedError"]);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var prestamosActivos = await _prestamoRepository.FindByConditionAsync(p => p.Id == id && p.Estado == EstadoPrestamo.Activo);
                if (prestamosActivos.IsSuccess && prestamosActivos.Data.Any())
                {
                    //-- CORRECCIÓN: Se añade un mensaje por defecto con '??'
                    var errorMessage = _configuration["ErrorMessages:Libros:BookIsOnLoan"] ?? "El libro no se puede eliminar porque está prestado.";
                    return OperationResult<bool>.Failure(errorMessage);
                }

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
            return await _libroRepository.ObtenerDetallesDTOPorIdAsync(id);
        }

        public async Task<OperationResult<IEnumerable<LibroDto>>> GetAllAsync()
        {
            return await _libroRepository.ObtenerTodosConDetallesAsync();
        }

        #endregion

        #region Implementación de ILibroService (Métodos Específicos)

        public async Task<OperationResult<LibroDto>> BuscarPorIsbnAsync(string isbn)
        {
            try
            {
                var resultadoRepo = await _libroRepository.BuscarPorIsbnAsync(isbn);

                if (!resultadoRepo.IsSuccess)
                {
                    return OperationResult<LibroDto>.Failure(resultadoRepo.Message);
                }
                if (resultadoRepo.Data == null)
                {
                    return OperationResult<LibroDto>.Success(null, "Libro no encontrado.");
                }

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
                Expression<Func<Libro, bool>> filtro = l =>
                    (l.Titulo.Contains(terminoBusqueda) || l.Autor.Contains(terminoBusqueda))
                    && l.EstaActivo;

                var resultadoRepo = await _libroRepository.FindByConditionAsync(filtro);
                if (!resultadoRepo.IsSuccess)
                {
                    return OperationResult<IEnumerable<LibroDto>>.Failure(resultadoRepo.Message);
                }

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