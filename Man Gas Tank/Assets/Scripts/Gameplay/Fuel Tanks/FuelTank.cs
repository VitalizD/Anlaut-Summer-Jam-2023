using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.FuelTanks
{
    public class FuelTank : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _bar;
        [SerializeField] private TMP_Text _volume;
        [SerializeField] private float _maxVolume;
        [SerializeField] private float _secondsForFillOneLiter;
        [SerializeField] private float _secondsForEmptyOneLiter;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private float _initialLiters;
        [SerializeField] private FuelType _initialFuelType;

        private float _currentLiters;
        private bool _isFilling = false;

        public static event Action TankFilled;
        public static event Action FillingTankEmptyStarted;
        public static event Action TankAlreadyEmpty;
        public static event Func<FuelType, Color> GetFuelColor;

        public FuelType FuelType { get; private set; } = FuelType.None;
        public bool IsFull => _currentLiters >= _maxVolume;
        public bool IsEmpty => _currentLiters <= 0f;

        private void Awake()
        {
            if (_debug)
            {
                ChangeValue(Mathf.Clamp(_initialLiters, 0f, _maxVolume));
                SetBarColor(GetFuelColor(_initialFuelType));
                FuelType = _initialFuelType;
            }
            else
            {
                ChangeValue(0);
            }
        }

        public void StartFill(FuelType fuelType, Color fuelColor)
        {
            FuelType = fuelType;
            SetBarColor(fuelColor);
            Stop();
            StartCoroutine(Fill());
        }

        public void StartEmpty()
        {
            if (_isFilling)
            {
                FillingTankEmptyStarted?.Invoke();
            }    
            Stop();
            StartCoroutine(Empty());
        }

        public void Stop()
        {
            _isFilling = false;
            StopAllCoroutines();
        }

        public void StopFill()
        {
            if (_isFilling)
            {
                Stop();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (IsEmpty)
                {
                    TankAlreadyEmpty?.Invoke();
                }
                StartEmpty();
            }
        }

        private IEnumerator Fill()
        {
            _isFilling = true;
            while (_currentLiters < _maxVolume)
            {
                ChangeValue(_currentLiters + (Time.deltaTime / _secondsForFillOneLiter));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            _isFilling = false;
            TankFilled?.Invoke();
        }

        private IEnumerator Empty()
        {
            while (_currentLiters > 0f)
            {
                ChangeValue(_currentLiters - (Time.deltaTime / _secondsForEmptyOneLiter));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            FuelType = FuelType.None;
        }

        private void ChangeValue(float value)
        {
            _currentLiters = value;
            _bar.fillAmount = value / _maxVolume;
            ChangeVolumeText((int)value);
        }

        private void ChangeVolumeText(int value) => _volume.text = $"{value} ë";

        private void SetBarColor(Color color) => _bar.color = color;
    }
}
