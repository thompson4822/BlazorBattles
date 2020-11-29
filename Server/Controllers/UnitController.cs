using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBattles.Server.Data;
using BlazorBattles.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly DataContext _dataContext;

        private readonly ILogger<UnitController> _logger;

        public UnitController(ILogger<UnitController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        private async Task<IList<Unit>> FetchUnits() => await _dataContext.Units.ToListAsync();
        
        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            return Ok(await FetchUnits());
        }

        [HttpPost]
        public async Task<IActionResult> AddUnit(Unit unit)
        {
            await _dataContext.Units.AddAsync(unit);
            await _dataContext.SaveChangesAsync();
            return Ok(await FetchUnits());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, Unit unit)
        {
            Unit dbUnit = await _dataContext.Units.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUnit == null)
            {
                return NotFound("Unit with the given id doesn't exist");
            }

            // I think this is where C# 9's records would be really useful
            dbUnit.Title = unit.Title;
            dbUnit.Attack = unit.Attack;
            dbUnit.Defense = unit.Defense;
            dbUnit.BananaCost = unit.BananaCost;
            dbUnit.HitPoints = unit.HitPoints;

            await _dataContext.SaveChangesAsync();
            return Ok(dbUnit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            Unit dbUnit = await _dataContext.Units.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUnit == null)
            {
                return NotFound("Unit with the given id doesn't exist");
            }

            _dataContext.Units.Remove(dbUnit);
            await _dataContext.SaveChangesAsync();
            return Ok(await FetchUnits());
        }
        
    }
}