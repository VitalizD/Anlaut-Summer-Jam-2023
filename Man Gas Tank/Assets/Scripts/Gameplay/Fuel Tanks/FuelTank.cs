using Gameplay.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.FuelTanks
{
    public class FuelTank : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private Image _bar;
        [SerializeField] private TMP_Text _volume;
        [SerializeField] private float _maxVolume;
        [SerializeField] private float _secondsForFillOneLiter;
        [SerializeField] private float _secondsForEmptyOneLiter;
        [SerializeField] private float _followSpeed;
        [SerializeField] private float _returnSpeed;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private float _initialLiters;
        [SerializeField] private FuelType _initialFuelType;

        private float _currentLiters;
        private Vector3 _initialPosition;
        private bool _isFilling = false;
        private bool _isReturning = false;
        private Camera _mainCamera;

        public static event Action<FuelType> TankFilled;
        public static event Action<FuelType> FillingTankEmptyStarted;
        public static event Action TankAlreadyEmpty;
        public static event Action<FuelType> FillingTankCaptured;
        public static event Func<FuelType, Color> GetFuelColor;

        public FuelType FuelType { get; private set; } = FuelType.None;
        public bool IsFull => _currentLiters >= _maxVolume;
        public bool IsEmpty => _currentLiters <= 0f;
        public float LitersCount => _currentLiters;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _initialPosition = transform.position;
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
                FillingTankEmptyStarted?.Invoke(FuelType);
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

        public void AddFuel(float value)
        {
            ChangeValue(Mathf.Clamp(Mathf.Ceil(_currentLiters + value), 0f, _maxVolume));
        }

        public void Clear()
        {
            FuelType = FuelType.None;
            ChangeValue(0f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (IsEmpty)
                {
                    TankAlreadyEmpty?.Invoke();
                }
                else
                {
                    StartEmpty();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isReturning = false;
            transform.SetAsLastSibling();
            if (_isFilling)
            {
                FillingTankCaptured?.Invoke(FuelType);
            }
            Stop();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var followPosition = new Vector3(mousePosition.x, mousePosition.y, 0f);
            transform.position = Vector3.Lerp(transform.position, followPosition, _followSpeed * Time.fixedDeltaTime);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isReturning = true;
            CheckCursorOver();
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
            TankFilled?.Invoke(FuelType);
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

        private void CheckCursorOver()
        {
            var hits = GetScreenRaycastResults();
            foreach (var obj in hits)
            {
                if (obj.TryGetComponent<Order>(out Order order))
                {
                    order.AcceptIncomingTank(this);
                }
            }
        }

        private GameObject[] GetScreenRaycastResults()
        {
            var pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            return raycastResults.Select(element => element.gameObject).ToArray();
        }

        private void ChangeVolumeText(int value) => _volume.text = $"{value} ë";

        private void SetBarColor(Color color) => _bar.color = color;

        private void Update()
        {
            if (_isReturning)
            {
                transform.position = Vector3.Lerp(transform.position, _initialPosition, _returnSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _initialPosition) <= 0.01f)
                    _isReturning = false;
            }
        }
    }
}
