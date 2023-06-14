using Gameplay.Orders;
using UnityEngine;

namespace Service.Storages
{
    [RequireComponent(typeof(LevelData))]
    public class LevelDataHandler : MonoBehaviour
    {
        private LevelData _levelData;

        private void Awake()
        {
            _levelData = GetComponent<LevelData>();
        }

        private void OnEnable()
        {
            Order.AddSlashResource += AddServicesCars;
            Order.AddResource += AddMoney;
        }

        private void OnDisable()
        {
            Order.AddSlashResource -= AddServicesCars;
            Order.AddResource -= AddMoney;
        }

        private void AddServicesCars(CounterType type, int value)
        {
            if (type == CounterType.Cars)
            {
                _levelData.AddServicedCars(value);
            }
        }

        private void AddMoney(CounterType type, int value)
        {
            if (type == CounterType.Money)
            {
                _levelData.AddMoney(value);
            }
        }
    }
}
