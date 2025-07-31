namespace SGB.Domain.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity(int id)
        {
            Id = id;
        }
        protected BaseEntity() { }

        public int Id { get; private set; }
    }
}
