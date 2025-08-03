namespace SGB.Presentation.Endpoints.EndpointsPrestamo
{
    public class PrestamoEndpoints : IPrestamoEndpoints
    {
        public string GetAll => "Prestamo/GetPrestamos";
        public string GetById => "Prestamo/GetPrestamosById";
        public string Create => "Prestamo/AddPrestamo";
        public string Update => "Prestamo/UpdatePrestamo";
        public string Delete => "Prestamo/DisablePrestamo";
        public string RegistrarDevolucion => "Prestamo/Registrar-devolucion";
    }
}
