using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Extensions;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Models;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Controllers
{
    public class ManageShortlinksController(IShortlinkService service) : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUrl(ShortlinkCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(nameof(Index), model);

                var result = await service.Create(model.TargetUrl!);

                return RedirectToAction(nameof(State), new { id = result.Alias });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "There was a problem creating the shortlink. Please try again.");
                return View(nameof(Index), model);
            }
        }

        public async Task<ActionResult> State(string id)
        {
            try
            {
                var result = await service.GetByAlias(id);

                var model = new ShortlinkDetailViewModel
                {
                    ShortlinkId = result.Id,
                    TargetUrl = result.TargetUrl,
                    ShortlinkUrl = new Uri(Request.GetBaseUrl(), $"s/{result.Alias}").ToString(),
                    StatusUrl = Request.GetDisplayUrl(),
                    ExpirationDate = result.ExpirationDate
                };

                return View(model);
            }
            catch (RetrieveShortlinkException)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int shortlinkId)
        {
            try
            {
                await service.Delete(shortlinkId);

                return View();
            }
            catch (EntryNotFoundException)
            {
                return View();
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return View("Error");
            }
        }

        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
