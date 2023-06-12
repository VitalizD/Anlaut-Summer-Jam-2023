using Gameplay.FuelTanks;
using Service.Controllers;
using UnityEngine;

namespace Gameplay.UI.Buttons.FuelToggles
{
    [RequireComponent(typeof(FuelToggle))]
    public class FuelToggleHandler : MonoBehaviour
    {
        private FuelToggle _fuelToggle;

        private void Awake()
        {
            _fuelToggle = GetComponent<FuelToggle>();
        }

        private void OnEnable()
        {
            FuelTank.TankFilled += Off;
            FuelTank.FillingTankEmptyStarted += Off;
            FuelTank.FillingTankCaptured += Off;
            FuelTanksController.NoFreeTanks += Off;
            GameController.SetActiveToggle += SetActive;
        }

        private void OnDisable()
        {
            FuelTank.TankFilled -= Off;
            FuelTank.FillingTankEmptyStarted -= Off;
            FuelTank.FillingTankCaptured -= Off;
            FuelTanksController.NoFreeTanks -= Off;
            GameController.SetActiveToggle -= SetActive;
        }

        private void Off()
        {
            _fuelToggle.Switch(false);
        }

        private void SetActive(FuelType fuelType, bool value)
        {
            if (fuelType == _fuelToggle.FuelType)
            {
                _fuelToggle.SetActive(value);
            }
        }
    }
}
