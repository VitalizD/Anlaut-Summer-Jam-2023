using Gameplay.Orders;
using Gameplay.UI.Counters;
using UnityEngine;

namespace Service.Controllers
{
    [RequireComponent(typeof(GameController))]
    public class GameControllerHandler : MonoBehaviour
    {
        private GameController _gameController;

        private void Awake()
        {
            _gameController = GetComponent<GameController>();
        }

        private void OnEnable()
        {
            OrdersManager.GetNextTransport += _gameController.GetNextTransport;
            OrdersManager.FuelIsAvailable += _gameController.FuelIsAvailable;
            OrdersManager.GasStationClosed += _gameController.GasStationClosed;
            OrdersManager.DayCompleted += _gameController.FinishLevel;
            UISlashCounter.ValueEqualSlashValue += CloseGasStation;
        }

        private void OnDisable()
        {
            OrdersManager.GetNextTransport -= _gameController.GetNextTransport;
            OrdersManager.FuelIsAvailable -= _gameController.FuelIsAvailable;
            OrdersManager.GasStationClosed -= _gameController.GasStationClosed;
            OrdersManager.DayCompleted -= _gameController.FinishLevel;
            UISlashCounter.ValueEqualSlashValue -= CloseGasStation;
        }

        private void CloseGasStation(CounterType counterType)
        {
            if (counterType == CounterType.Cars)
            {
                _gameController.CloseGasStation();
            }
        }
    }
}
