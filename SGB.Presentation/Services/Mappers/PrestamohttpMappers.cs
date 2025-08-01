namespace SGB.Presentation.Services.Mappers
{
    // Mappers/PrestamoMapper.cs

    using global::SGB.Application.Dtos.Prestamos_PenalizacionDto.PrestamoDto;
    using global::SGB.Presentation.Models.PrestamoModels;

    namespace SGB.Presentation.Mappers
    {
        public static class PrestamoMapper
        {
            public static AddPrestamoDto ToAddPrestamoDto(PrestamoCreateModel model)
            {
                return new AddPrestamoDto
                {
                    UsuarioId = model.UsuarioId,
                    ISBN = model.ISBN,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin
                };
            }

            public static UpdatePrestamoDto ToUpdatePrestamoDto(PrestamoEditModel model)
            {
                return new UpdatePrestamoDto
                {
                    IDPrestamo = model.IDPrestamo,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin
                };
            }

            public static RegistrarDevolucionDto ToRegistrarDevolucionDto(PrestamoDevolucionModel model)
            {
                return new RegistrarDevolucionDto
                {
                    IdPrestamo = model.IdPrestamo,
                    FechaDevolucion = model.FechaDevolucion
                };
            }


        }
    }
}


