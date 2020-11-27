using System;
using System.Collections.Generic;
using System.Linq;
using BlazorBattles.Shared.Entities;

namespace BlazorBattles.Client.Services
{
    public interface IUnitService
    {
        /// <summary>What types of units exist?</summary>
        IList<Unit> Units { get; }

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

    }

    class UnitService : IUnitService
    {
        public IList<Unit> Units { get; } = new List<Unit>
        {
            new Unit {Id = 1, Title = "Knight", Attack = 10, Defense = 10, BananaCost = 100},
            new Unit {Id = 2, Title = "Archer", Attack = 15, Defense = 5, BananaCost = 150},
            new Unit {Id = 3, Title = "Mage", Attack = 20, Defense = 1, BananaCost = 200},
        };

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
        
        public void AddUnit(int unitId)
        {
            var unit = UnitFor(unitId);
            MyUnits.Add(new UserUnit { UnitId = unit.Id, HitPoints = unit.HitPoints });
        }
    }
}