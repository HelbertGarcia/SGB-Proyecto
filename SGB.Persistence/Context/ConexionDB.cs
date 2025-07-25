using System;
using System.Data.SqlClient;

namespace SGB.Infrastructure
{
    public class ConexionDB
    {
        private readonly string _connectionString =
            "Server=DESKTOP-3N5TAVS;Database=SGB;Integrated Security=True;TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            try
            {
                var conexion = new SqlConnection(_connectionString);
                conexion.Open();
                Console.WriteLine("✅ Conexión exitosa a la base de datos.");
                return conexion;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al conectar: {ex.Message}");
                throw;
            }
        }
    }
}
