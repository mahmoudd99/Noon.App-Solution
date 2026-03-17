using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noon.App.Dtos;
using Noon.App.Errors;
using Noon.App.Helpers;
using Noon.Core;
using Noon.Core.Entities.PruductModule;
using Noon.Core.Repositories;
using Noon.Core.Specifications;
using Noon.Repository;

namespace Noon.App.Controllers
{

    public class ProductsController : BaseApiController
    {

        //private readonly IGenaricRepository<Product> _reposProduct;
        //private readonly IGenaricRepository<ProductBrand> _brandRepos;
        //private readonly IGenaricRepository<ProductType> _typeRepos;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ProductsController(
            //IGenaricRepository<Product> ProductRepos,
            //IGenaricRepository<ProductBrand> BrandRepos,
            //IGenaricRepository<ProductType> TypeRepos,
            IUnitOfWork unit,
            IMapper mapper)
        {
            //_reposProduct = ProductRepos;
            //_brandRepos = BrandRepos;
            //_typeRepos = TypeRepos;
            _unit = unit;
            _mapper = mapper;
        }


       
        //[Authorize]
        //[CachedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery] ProdcutSpecParams specParams)
        {
            var spec = new ProductWithBrandAndType(specParams);
            var products = await _unit.Repository<Product>().GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countspecification = new ProductWithFilterationForCountSpecifiction(specParams);
            var count = await _unit.Repository<Product>().GetCountWithSpecAsync(countspecification);
            
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count,data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndType(id);

            var product = await _unit.Repository<Product>().GetByIdWithSpecAsync(spec);

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));


        }
        [HttpGet("brands")] //GET : /api/product/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unit.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("types")]  // GET : /api/products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unit.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }



    }
}
