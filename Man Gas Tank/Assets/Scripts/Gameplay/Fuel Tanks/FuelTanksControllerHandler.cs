using Gameplay.UI.Buttons.FuelToggles;
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
        }

        private void OnDisable()
        {
            FuelToggle.FuelToggleOn -= _fuelTankController.Fill;
            FuelToggle.FuelToggleOff -= _fuelTankController.StopFill;
            FuelTank.GetFuelColor -= _fuelTankController.GetFuelColor;
        }
    }
}