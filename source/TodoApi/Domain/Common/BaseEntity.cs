namespace TodoApi.Domain.Common
{
    public abstract class BaseEntity
    {

        public DateTime CreatedOn { get; protected set; } = DateTime.Now;
        public virtual int Id { get; protected set; }
        public DateTime? LastModifiedOn { get; set; }

    }
}
