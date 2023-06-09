using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Buttons.FuelToggles
{
    public class FuelToggle : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Animator _animator;
        [SerializeField] private FuelType _fuelType;

        private readonly int ANIM_Pulsate = Animator.StringToHash("Pulsate");

        public static event Action<FuelType> FuelToggleOn;
        public static event Action FuelToggleOff;

        private void Awake()
        {
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        public void Switch(bool value)
        {
            _toggle.isOn = value;
        }

        private void OnValueChanged(bool value)
        {
            _animator.SetBool(ANIM_Pulsate, value);
            if (value)
            {
                FuelToggleOn?.Invoke(_fuelType);
            }
            else
            {
                FuelToggleOff?.Invoke();
            }
        }
    }
}
