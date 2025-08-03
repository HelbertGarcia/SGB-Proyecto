namespace SGB.Presentation.Endpoints.EndpointsPenalizacion
{
    public class PenalizacionEndpoints : IPenalizacionEndpoints
    {
        public string GetAll => "Penalizacion/GetPenalizaciones";
        public string GetById => "Penalizacion/GetPenalizacionById";
        public string Create => "Penalizacion/AddPenalizacion";
        public string Update => "Penalizacion/UpdatePenalizacion";
        public string Delete => "Penalizacion/DisablePenalizacion";
    }
}
