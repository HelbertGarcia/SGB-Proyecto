namespace SGB.Presentation.Endpoints.EndpointsPenalizacion
{
    public interface IPenalizacionEndpoints
    {
        string GetAll { get; }
        string GetById { get; }
        string Create { get; }
        string Update { get; }
        string Delete { get; }
    }
}
