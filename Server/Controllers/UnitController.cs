using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorBattles.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        public IList<Unit> Units { get; } = new List<Unit>
        {
            new Unit {Id = 1, Title = "Knight", Attack = 10, Defense = 10, BananaCost = 100},
            new Unit {Id = 2, Title = "Archer", Attack = 15, Defense = 5, BananaCost = 150},
            new Unit {Id = 3, Title = "Mage", Attack = 20, Defense = 1, BananaCost = 200},
        };

        private readonly ILogger<UnitController> _logger;

        public UnitController(ILogger<UnitController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            return await Task.Run(() => Ok(Units));
        }
    }
}