using Gameplay.UI.Buttons.FuelToggles;
using Service.Controllers;
using UnityEngine;

namespace Gameplay.FuelTanks
{
    [RequireComponent(typeof(FuelTanksController))]
    public class FuelTanksControllerHandler : MonoBehaviour
    {
        private FuelTanksController _fuelTankController;

        private void Awake()
        {
            _fuelTankController = GetComponent<FuelTanksController>();
        }

        private void OnEnable()
        {
            FuelToggle.FuelToggleOn += _fuelTankController.Fill;
            FuelToggle.FuelToggleOff += _fuelTankController.StopFill;
            FuelTank.GetFuelColor += _fuelTankController.GetFuelColor;
            GameController.LevelCompleted += _fuelTankController.ClearAll;
            GameController.SetFuelTanksCount += _fuelTankController.SetCount;
        }

        private void OnDisable()
        {
            FuelToggle.FuelToggleOn -= _fuelTankController.Fill;
            FuelToggle.FuelToggleOff -= _fuelTankController.StopFill;
            FuelTank.GetFuelColor -= _fuelTankController.GetFuelColor;
            GameController.LevelCompleted -= _fuelTankController.ClearAll;
            GameController.SetFuelTanksCount -= _fuelTankController.SetCount;
        }
    }
}