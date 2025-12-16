using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Controllers
{
    [ApiController]
    public class ResolveShortlinksController(IShortlinkService service) : ControllerBase
    {
        [HttpGet("s/{alias}")]
        public async Task<ActionResult> Get(string alias)
        {
            try
            {
                var result = await service.GetByAlias(alias);
                return Redirect(result.TargetUrl);
            }
            catch (RetrieveShortlinkException)
            {
                return RedirectToRoute(new
                {
                    controller = nameof(ManageShortlinksController).Replace("Controller",""),
                    action = nameof(ManageShortlinksController.PageNotFound)
                });
            }
        }
    }
}
