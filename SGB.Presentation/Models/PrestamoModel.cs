namespace SGB.Presentation.Models
{
    public class PrestamoModel
    {
        public int id { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime? fechaDevolucion { get; set; }
        public string estado { get; set; }
        public bool estaActivo { get; set; }
        public int usuarioId { get; set; }
        public string isbn { get; set; }
    }


    public class GetAllPrestamoResponse

    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public List<PrestamoModel> data { get; set; }


    }

    public class GetPrestamoById
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public PrestamoModel data { get; set; }
    }


    public class CreatePrestamo
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public PrestamoModel data { get; set; }
    }

    public class UpdatePrestamo
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public PrestamoModel data { get; set; }
    }


    public class DeletePrestamo
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public PrestamoModel data { get; set; }
    }


    public class RegistrarDevolucion
    {

        public bool isSuccess { get; set; }
        public string message { get; set; }
        public PrestamoModel data { get; set;}

    }

}
    


