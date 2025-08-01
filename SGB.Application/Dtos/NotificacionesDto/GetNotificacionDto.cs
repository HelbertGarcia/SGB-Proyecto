namespace SGB.Api.Dtos.NotificacionesDto
{
    public record GetNotificacionDto(
        int Id,
        string Tipo,
        string Mensaje,
        DateTime Fecha,
        bool Leida
    );
}
