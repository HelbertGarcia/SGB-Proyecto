namespace SGB.Presentation.Endpoints.EndpointsPrestamo
{
    public interface IPrestamoEndpoints
    {
        string GetAll { get; }
        string GetById { get; }
        string Create { get; }
        string Update { get; }
        string Delete { get; }
        string RegistrarDevolucion { get; }
    }
}
