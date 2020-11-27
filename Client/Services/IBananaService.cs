using System;

namespace BlazorBattles.Client.Services
{
    public interface IBananaService
    {
        int Bananas { get; set; }

        void EatBananas(int amount);

        event Action OnChange;
        void AddBananas(int amount);
    }

    class BananaService : IBananaService
    {
        public int Bananas { get; set; } = 1000;

        public void EatBananas(int amount)
        {
            Bananas -= amount;
            BananasChanged();
        }

        public event Action OnChange;
        public void AddBananas(int amount)
        {
            Bananas += amount;
            BananasChanged();
        }

        void BananasChanged() => OnChange.Invoke();
    }
}