using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Microsoft.Extensions.Options;
using Seido.Utilities.SeedGenerator;
using Configuration.Options;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly VersionOptions _versionOptions;
        private readonly IAdminService _service;

        public AdminController(IOptions<VersionOptions> versionOptions, IAdminService service)
        {
            _versionOptions = versionOptions.Value;
            _service = service;
        }

        //GET: api/admin/version
        [HttpGet()]
        [ActionName("Version")]
        [ProducesResponseType(typeof(VersionOptions), 200)]
        public IActionResult Version()
        {
            try
            {
                return Ok(_versionOptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("Seed")]
        public async Task<IActionResult> Seed(int count)
        {
            try
            {
                await _service.SeedAsync(count);
                return Ok($"Seeded {count} credit cards.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}