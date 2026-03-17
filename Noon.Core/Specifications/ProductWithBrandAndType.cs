using Noon.Core.Entities.PruductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Specifications
{
    public class ProductWithBrandAndType : BaseSpecificationEntity<Product>
    {

        public ProductWithBrandAndType(ProdcutSpecParams specParams)
            : base(P =>
                    (!string.IsNullOrEmpty(specParams.Search)|| P.Name.ToLower().Contains(specParams.Search)) &&
                   
                    (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId)&&
                    (!specParams.TypeId.HasValue || P.ProductTypeId==specParams.TypeId)
                 )  
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);


            if (!string.IsNullOrEmpty(specParams.Sort))
                switch (specParams.Sort)
                {
                    case "priceAsc":
                         AddOrderBy(P=>P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                        default:
                        AddOrderBy(P => P.Name);
                        break;
                }

            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        }


        public ProductWithBrandAndType(int id) : base(P => P.Id == id)
        {

            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
