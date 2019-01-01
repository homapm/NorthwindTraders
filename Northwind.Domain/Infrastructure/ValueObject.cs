using System.Collections.Generic;
using System.Linq;

namespace Northwind.Domain.Infrastructure
{
    // Source: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
    public abstract class ValueObject
    {

        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            // If at least one diference is found (One of objects is null)
            // Return false : Not Equal
            if (left is null ^ right is null)
            {
                return false;
            }

            return left?.Equals(right) != false;
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            // If the object is null , 
            // Or the type of object type is not the same with the current object, 
            // Return false : Not Equal
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            // Cast the object into a Valueobject class
            var other = (ValueObject)obj;

            // Get the collection of object's properties
            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                // If at least one difference between the properties exists, return false (Notequal)
                if (thisValues.Current is null ^ otherValues.Current is null)
                {
                    return false;
                }

                // if this value is not null and it is not equal to other value return false (NotEqual)
                if (thisValues.Current != null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            // If iteration is compeleted and There is not any difference return true. otherwise, false.
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
