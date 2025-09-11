using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Models;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Extensions;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Models;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Controllers
{
    public class ManageShortlinksController : Controller
    {
        private readonly IShortlinkService _service;

        public ManageShortlinksController(IShortlinkService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUrl(ShortlinkCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), model);

            var result = await _service.Create(model.TargetUrl!);

            return RedirectToAction(nameof(State), new { alias = result.Alias });
        }

        [Route("State/{alias}")]
        public async Task<ActionResult> State(string alias)
        {
            try
            {
                var result = await _service.GetByAlias(alias);

                var model = new ShortlinkDetailViewModel
                {
                    Id = result.Id,
                    TargetUrl = result.TargetUrl,
                    ShortlinkUrl = new Uri(Request.GetBaseUrl(), $"s/{result.Alias}").ToString(),
                    StatusUrl = Request.GetDisplayUrl(),
                    ExpirationDate = result.ExpirationDate
                };

                return View(model);
            }
            catch (RetrieveShortlinkException)
            {
                return RedirectToAction(nameof(PageNotFound));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);

                return View();
            }
            catch (EntryNotFoundException)
            {
                return RedirectToAction(nameof(PageNotFound));
            }
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
