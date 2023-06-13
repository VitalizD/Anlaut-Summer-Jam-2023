using Service.Controllers;
using UnityEngine;

namespace Gameplay.UI.Buttons.FuelToggles
{
    public class FuelTogglesHandler : MonoBehaviour
    {
        [SerializeField] private FuelToggle[] _fuelToggles;

        private void OnEnable()
        {
            GameController.SetActiveToggle += SetActive;
        }

        private void OnDisable()
        {
            GameController.SetActiveToggle -= SetActive;
        }

        private void SetActive(FuelType fuelType, bool value)
        {
            foreach (var toggle in _fuelToggles)
            {
                if (fuelType == toggle.FuelType)
                {
                    toggle.SetActive(value);
                }
            }
        }
    }
}