using System.ComponentModel.DataAnnotations;

namespace AbcCompany.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }


        protected BaseEntity()
        {

        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseEntity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(BaseEntity a, BaseEntity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(BaseEntity a, BaseEntity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        public T DeepClone<T>() where T : BaseEntity
        {
            T newEntity = (T)this.MemberwiseClone();
            newEntity.Id = 0;
            return newEntity;
        }

    }
}
