using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noon.App.Errors;
using Noon.Repository.Data;

namespace Noon.App.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("notfound")] // GET: /api/Buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbcontext.Products.Find(100);
            if(product is null) return NotFound(new ApiResponse(404));
            return Ok(product);

        }

        [HttpGet("serverError")] // GET: /api/Buggy/serverError
        public ActionResult GetServerError()
        {
            var product = _dbcontext.Products.Find(200);
            var producrToReturn = product.ToString(); // Will Throw Exception

            return Ok(producrToReturn);
        }


        [HttpGet("fortytwo")]  // GET: /api/Buggy/badRequest
        public ActionResult GetBadReuest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badRequest/{id}")]  // GET: /api/Buggy/validationError/3

        public ActionResult GetBadRequest(int id) //ValidationError
        {
            return Ok();
        }


    }
}
