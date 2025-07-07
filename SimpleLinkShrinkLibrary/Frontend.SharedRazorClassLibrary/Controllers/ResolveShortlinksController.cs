using Microsoft.AspNetCore.Mvc;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Exceptions;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Controllers
{
    [ApiController]
    public class ResolveShortlinksController : ControllerBase
    {
        private readonly IRepository _repository;

        public ResolveShortlinksController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("s/{alias}")]
        public async Task<ActionResult> Get(string alias)
        {
            try
            {
                var result = await _repository.Get(alias);
                return Redirect(result.TargetUrl);
            }
            catch (ShortlinkNotFoundException)
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
