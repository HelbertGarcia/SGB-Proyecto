using Microsoft.AspNetCore.Mvc.Rendering;
using SGB.Application.Dtos.LibrosDto.LibroDto;
using SGB.Application.Wrappers;
using SGB.Presentation.Models.Libro;
using SGB.Presentation.Services.Base;

namespace SGB.Presentation.Services
{
    public class LibroHttpService : ILibroHttpService
    {
        private readonly IHttpService _httpService;
        private readonly ICategoriaHttpService _categoriaHttpService;

        public LibroHttpService(IHttpService httpService, ICategoriaHttpService categoriaHttpService)
        {
            _httpService = httpService;
            _categoriaHttpService = categoriaHttpService;
        }

        public async Task<List<LibroModel>> ObtenerTodos()
        {
            var response = await _httpService.GetAsync<List<LibroModel>>("api/Libro/GetAllLibros");
            return (response != null && response.IsSuccess) ? response.Data : new List<LibroModel>();
        }

        public async Task<LibroModel> ObtenerPorId(int id)
        {
            var response = await _httpService.GetAsync<LibroModel>($"api/Libro/GetLibroById/{id}");
            return (response != null && response.IsSuccess) ? response.Data : null;
        }

        public async Task<ApiResponse<LibroDto>> Crear(AddLibroDto dto)
        {
            return await _httpService.PostAsJsonAsync<AddLibroDto, LibroDto>("api/Libro/AddLibro", dto);
        }

        public async Task<ApiResponse<object>> Actualizar(int id, UpdateLibroDto dto)
        {
            return await _httpService.PutAsJsonAsync<UpdateLibroDto, object>($"api/Libro/UpdateLibro/{id}", dto);
        }

        public async Task<ApiResponse<bool>> Eliminar(int id)
        {
            return await _httpService.DeleteAsync($"api/Libro/DisableLibro/{id}");
        }

        public async Task<LibroCreateViewModel> PrepararCreateViewModel()
        {
            var categorias = await _categoriaHttpService.ObtenerTodas();
            var viewModel = new LibroCreateViewModel
            {
                Libro = new LibroModel(),
                CategoriasDisponibles = new SelectList(categorias, "id", "nombre")
            };
            return viewModel;
        }

        public async Task<LibroEditViewModel> PrepararEditViewModel(int id)
        {
            var libro = await ObtenerPorId(id);
            if (libro == null) return null;

            var categorias = await _categoriaHttpService.ObtenerTodas();
            var categoriaActual = categorias.FirstOrDefault(c => c.nombre == libro.nombreCategoria);
            if (categoriaActual != null)
            {
                libro.IDCategoria = categoriaActual.id;
            }

            var viewModel = new LibroEditViewModel
            {
                Libro = libro,
                CategoriasDisponibles = new SelectList(categorias, "id", "nombre", libro.IDCategoria)
            };
            return viewModel;
        }
    }
}