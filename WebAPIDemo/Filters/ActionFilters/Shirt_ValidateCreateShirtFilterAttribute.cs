using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing;
using System.Reflection;
using WebAPIDemo.Data;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ActionFilters
{
    public class Shirt_ValidateCreateShirtFilterAttribute : ActionFilterAttribute
    {
        public Shirt_ValidateCreateShirtFilterAttribute(Repository repository)
        {
            Repository = repository;
        }

        public Repository Repository { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var shirt = context.ActionArguments["shirt"] as Shirt;
            if (shirt == null)
            {
                context.ModelState.AddModelError("Shirt", "Shirt is invalid.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var existingShirt = Repository.Shirts.FirstOrDefault(x => !string.IsNullOrWhiteSpace(shirt.Brand)
                                                && !string.IsNullOrWhiteSpace(x.Brand)
                                                && shirt.Brand.ToLower() == x.Brand.ToLower()
                                                && !string.IsNullOrWhiteSpace(shirt.Gender)
                                                && !string.IsNullOrWhiteSpace(x.Gender)
                                                && shirt.Gender.ToLower() == x.Gender.ToLower()
                                                && !string.IsNullOrWhiteSpace(shirt.Color)
                                                && !string.IsNullOrWhiteSpace(x.Color)
                                                && shirt.Color.ToLower() == x.Color
                                                && shirt.Size.HasValue
                                                && x.Size.HasValue
                                                && shirt.Size.Value == x.Size.Value);

                if(existingShirt != null)
                {
                    context.ModelState.AddModelError("Shirt", "Shirt already exists.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }
    }
}
