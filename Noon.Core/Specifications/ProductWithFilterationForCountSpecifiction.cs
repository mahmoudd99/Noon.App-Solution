using Noon.Core.Entities.PruductModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Specifications
{
    public class ProductWithFilterationForCountSpecifiction:BaseSpecificationEntity<Product>
    {
        public ProductWithFilterationForCountSpecifiction(ProdcutSpecParams specParams)
        : base(P =>
                    (!string.IsNullOrEmpty(specParams.Search)|| P.Name.ToLower().Contains(specParams.Search)) &&
                    
                    
                    (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId)&&
                    (!specParams.TypeId.HasValue || P.ProductTypeId==specParams.TypeId)
              )
        {
            
        }
    }
}
