using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorBattles.Shared.Entities;
using Blazored.Toast.Services;

namespace BlazorBattles.Client.Services
{
    public interface IUnitService
    {
        /// <summary>What types of units exist?</summary>
        IList<Unit> Units { get; set; }

        /// <summary>
        /// What icon should we use for the given user unit?
        /// </summary>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        string IconFor(UserUnit userUnit);
        
        /// <summary>What units do I currently have (what is my army)?</summary>
        IList<UserUnit> MyUnits { get; set; }

        /// <summary>
        /// Add a unit to the army using the given unit id
        /// </summary>
        /// <param name="unitId">the id of the unit</param>
        void AddUnit(int unitId);
        
        /// <summary>
        /// What unit matches the given unit id?
        /// </summary>
        /// <param name="id">the unit id</param>
        /// <returns></returns>
        Unit UnitFor(int id);

        Task LoadUnitsAsync();

    }

    class UnitService : IUnitService
    {
        private readonly IToastService _toastService;
        private readonly HttpClient _httpClient;

        public UnitService(IToastService toastService, HttpClient httpClient)
        {
            _toastService = toastService;
            _httpClient = httpClient;
        }

        public IList<Unit> Units { get; set; } = new List<Unit>();

        public string IconFor(UserUnit unit)
        {
            Dictionary<int, string> iconMap = new()
            {
                {1, "icons/W_Sword006.png"},
                {2, "icons/S_Bow08.png"},
                {3, "icons/C_Hat01.png"},
            };
            return iconMap[unit.UnitId];
        }

        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

        public Unit UnitFor(int id) => Units.FirstOrDefault(unit => unit.Id == id);
        
        public async Task LoadUnitsAsync()
        {
            if (Units.Count == 0)
            {
                Units = await _httpClient.GetFromJsonAsync<IList<Unit>>("api/unit");
            }
        }

        public void AddUnit(int unitId)
        {
            var unit = UnitFor(unitId);
            MyUnits.Add(new UserUnit { UnitId = unit.Id, HitPoints = unit.HitPoints });
            _toastService.ShowSuccess($"Your {unit.Title} has been built", "Unit Built");
        }
    }
}