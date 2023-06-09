using Gameplay.FuelTanks;
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
            FuelTanksController.NoFreeTanks += Off;
        }

        private void OnDisable()
        {
            FuelTank.TankFilled -= Off;
            FuelTank.FillingTankEmptyStarted -= Off;
            FuelTanksController.NoFreeTanks -= Off;
        }

        private void Off()
        {
            _fuelToggle.Switch(false);
        }
    }
}
