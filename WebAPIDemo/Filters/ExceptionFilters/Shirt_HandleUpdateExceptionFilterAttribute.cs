using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Data;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ExceptionFilters
{
    public class Shirt_HandleUpdateExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public Shirt_HandleUpdateExceptionFilterAttribute(Repository repository)
        {
            Repository = repository;
        }

        public Repository Repository { get; }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            var shirtId = int.TryParse(context.RouteData.Values["id"] as string , out int ShirtId);
            var shirt = Repository.Shirts.FirstOrDefault(x => x.ShirtId == ShirtId);
            if ( shirt == default)
            {
                context.ModelState.AddModelError("ShirtId", "Shirt does not exists anymore.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound,
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
        }
    }
}
