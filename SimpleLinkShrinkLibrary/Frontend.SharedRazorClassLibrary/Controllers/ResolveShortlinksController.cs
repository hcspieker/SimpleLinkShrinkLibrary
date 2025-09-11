using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Controllers
{
    [ApiController]
    public class ResolveShortlinksController : ControllerBase
    {
        private readonly IShortlinkService _service;

        public ResolveShortlinksController(IShortlinkService service)
        {
            _service = service;
        }

        [HttpGet("s/{alias}")]
        public async Task<ActionResult> Get(string alias)
        {
            try
            {
                var result = await _service.GetByAlias(alias);
                return Redirect(result.TargetUrl);
            }
            catch (EntryNotFoundException)
            {
                return RedirectToRoute(new
                {
                    controller = "ManageShortlinks",
                    action = "PageNotFound"
                });
            }
        }
    }
}
