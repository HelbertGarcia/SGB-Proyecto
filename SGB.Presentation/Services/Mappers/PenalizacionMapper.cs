using SGB.Application.Dtos.Prestamos_PenalizacionDto.PenalizacionDto;
using SGB.Presentation.Models.PenalizacionModels;

namespace SGB.Presentation.Services.Mappers
{
    public static class PenalizacionMapper
    {
        public static AddPenalizacionDto ToAddPenalizacionDto(PenalizacionCreateModel model)
        {
            return new AddPenalizacionDto
            {
                UsuarioId = model.UsuarioId,
                IDPrestamo = model.IDPrestamo,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                Monto = model.Monto,
                Motivo = model.Motivo
            };
        }

        public static UpdatePenalizacionDto ToUpdatePenalizacionDto(PenalizacionEditModel model)
        {
            return new UpdatePenalizacionDto
            {
                IDPenalizacion = model.IDPenalizacion,
                FechaFin = model.FechaFin,
                Monto = model.Monto,
                Motivo = model.Motivo
            };
        }


    }
}

