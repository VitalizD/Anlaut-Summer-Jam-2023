using UnityEngine;

namespace Service.Storages
{
    public class LevelData : MonoBehaviour
    {
        public int Money { get; private set; }
        public int ServicedCars { get; private set; }

        public void AddMoney(int value) => Money += value;

        public void AddServicedCars(int value) => ServicedCars += value;

        public void Clear()
        {
            Money = 0;
            ServicedCars = 0;
        }
    }
}
