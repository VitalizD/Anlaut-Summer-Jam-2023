using System;

namespace Service.Controllers
{
    [Serializable]
    public class MatchFuelLevel
    {
        public FuelType Fuel;
        public int RequireLevel;
    }
}