using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class ShirtsController : Controller
    {
        public ShirtsController(IWebApiExecuter webApiExecuter)
        {
            WebApiExecuter = webApiExecuter;
        }

        public IWebApiExecuter WebApiExecuter { get; }

        public async Task<IActionResult> Index()
        {
            return View(await WebApiExecuter.InvokeGet<List<Shirt>>("shirts"));
        }
        public IActionResult CreateShirt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var response = await WebApiExecuter.InvokePost("shirts", shirt);
                    if (response != null)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (WebAPIException ex)
                {
                    HandleWebApiException(ex);
                }
                
            }
            return View(shirt);
        }

        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            try
            {
                var shirt = await WebApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");
                if (shirt != null)
                {
                    return View(shirt);
                }
            }
            catch (WebAPIException ex)
            {
                HandleWebApiException(ex);
                return View();
            }
            
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    await WebApiExecuter.InvokePut($"shirts/{shirt.ShirtId}", shirt);
                    return RedirectToAction(nameof(Index));
                }
                catch (WebAPIException ex)
                {
                    HandleWebApiException(ex);
                    return View();
                }
                
            }
            return View(shirt);
        }

        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    await WebApiExecuter.InvokeDelete($"shirts/{shirtId}");
                    return RedirectToAction(nameof(Index));
                }
                catch (WebAPIException ex)
                {
                    HandleWebApiException(ex);
                    return View(nameof(Index), await WebApiExecuter.InvokeGet<List<Shirt>>("shirts"));
                }
            }
            return View();
        }

        private void HandleWebApiException(WebAPIException webAPIException)
        {
            if (webAPIException.ErrorResponse != null
                        && webAPIException.ErrorResponse.Errors != null
                        && webAPIException.ErrorResponse.Errors.Count > 0)
            {
                foreach (var error in webAPIException.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join(";", error.Value));
                }
            }
        }
    }
}
