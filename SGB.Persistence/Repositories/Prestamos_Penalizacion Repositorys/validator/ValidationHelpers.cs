using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGB.Persistence.Repositories.Prestamos_Penalizacion_Repositorys.validator
{
    public static class ValidationHelpers
    {

        public static (bool IsValid, string Message) ValidateEntityNotNull<T>(T entity, string entityName)
        {
            if (entity == null)
                return (false, $"{entityName} no puede ser nulo.");
            return (true, string.Empty);
        }

        public static (bool IsValid, string Message) ValidateId(int id, string idName)
        {
            if (id <= 0)
                return (false, $"El {idName} es inválido.");
            return (true, string.Empty);
        }

        public static (bool IsValid, string Message) ValidateIsbn(string? isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return (false, "ISBN no puede estar vacío.");
            if (isbn.Length != 13 || !isbn.All(char.IsDigit))
                return (false, "El ISBN debe contener exactamente 13 caracteres .");
            return (true, string.Empty);
        }

        public static (bool IsValid, string Message) ValidateStringNotNullOrWhitespace(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return (false, $"{fieldName} no puede ser nulo o vacío.");
            return (true, string.Empty);
        }

        public static (bool IsValid, string Message) ValidateDateRange(DateTime start, DateTime end)
        {
            if (start == default || end == default)
                return (false, "Las fechas de inicio y fin no pueden ser vacías.");
            if (end < start)
                return (false, "La fecha fin no puede ser anterior a la fecha inicio.");
            return (true, string.Empty);
        }
    }
}
