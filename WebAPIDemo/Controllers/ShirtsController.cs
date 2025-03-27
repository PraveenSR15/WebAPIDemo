using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Data;
using WebAPIDemo.Filters;
using WebAPIDemo.Filters.ActionFilters;
using WebAPIDemo.Filters.AuthFilters;
using WebAPIDemo.Filters.ExceptionFilters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenAuthFilter]
    public class ShirtsController : ControllerBase
    {
        public Repository Repository { get; }

        public ShirtsController(Repository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(Repository.Shirts.ToList());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult GetShirtByID(int id)
        {
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            Repository.Shirts.Add(shirt);
            Repository.SaveChanges();
            return CreatedAtAction(nameof(GetShirtByID), new { id = shirt.ShirtId}, shirt);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateShirtUpdateFilter]
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionFilterAttribute))]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Gender = shirt.Gender;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Size = shirt.Size;

            Repository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult DeleteShirt(int id)
        {
            var shirt = HttpContext.Items["shirt"] as Shirt;
            Repository.Shirts.Remove(shirt);
            Repository.SaveChanges();
            return Ok(shirt);
        }
    }
}
