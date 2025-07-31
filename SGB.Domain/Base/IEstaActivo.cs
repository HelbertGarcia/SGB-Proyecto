namespace SGB.Domain.Base
{
    public interface IEstaActivo
    {
        public bool EstaActivo { get;} 
        public void Deshabilitar();
        public void Habilitar();
    }
}
