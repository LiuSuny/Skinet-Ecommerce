using Core.Entities;

namespace Core.Specification
{
    public class TypeListSpecification: BaseSpecification<Product, string>
    {
        public TypeListSpecification()
        {
            AddSelectByBrandAndType(x => x.Type);
            ApplyDistinct();
        }
    }
}