using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Orders
{
    public class OrdersManager : MonoBehaviour
    {
        [SerializeField] private Order[] _orders;
        [SerializeField] private TransportData[] _transportStorage;
        [SerializeField] private Vector2 _requirementFuelLitersLimits;

        private readonly Dictionary<TransportType, TransportData> _transportStorageDict = new();

        public static event Func<FuelType, bool> FuelIsAvailable;
        public static event Func<TransportType> GetNextTransport;
        public static event Func<bool> GasStationClosed;
        public static event Action DayCompleted;

        private void Awake()
        {
            for (var i = 0; i < _orders.Length; ++i)
            {
                _orders[i].Initialize(i);
            }
            foreach (var data in _transportStorage)
            {
                _transportStorageDict.Add(data.Type, data);
            }
        }

        public void Generate(int orderIndex)
        {
            if (!_orders[orderIndex].gameObject.activeSelf || GasStationClosed())
            {
                CheckDayCompleted();
                return;
            }
            var requirements = new List<(FuelType, float)>();
            var transport = GetNextTransport();
            var fuels = _transportStorageDict[transport].RequiredFuels;
            for (var i = 0; i < fuels.Length; ++i)
            {
                if (FuelIsAvailable(fuels[i]))
                {
                    var fuelCount = UnityEngine.Random.Range(_requirementFuelLitersLimits.x, _requirementFuelLitersLimits.y);
                    if (i > 0)
                    {
                        fuelCount = UnityEngine.Random.Range(_requirementFuelLitersLimits.x, requirements[i - 1].Item2);
                    }
                    fuelCount = Mathf.Round(fuelCount);
                    requirements.Add((fuels[i], fuelCount));
                }
            }
            var name = _transportStorageDict[transport].Names[UnityEngine.Random.Range(0, _transportStorageDict[transport].Names.Length)];
            var monolog = _transportStorageDict[transport].Phrases[UnityEngine.Random.Range(0, _transportStorageDict[transport].Phrases.Length)];
            var sprite = _transportStorageDict[transport].Sprites[UnityEngine.Random.Range(0, _transportStorageDict[transport].Sprites.Length)];
            var avatar = _transportStorageDict[transport].Avatars[UnityEngine.Random.Range(0, _transportStorageDict[transport].Avatars.Length)];
            var waitingTime = UnityEngine.Random.Range(_transportStorageDict[transport].WaitingTimeLimits.x, _transportStorageDict[transport].WaitingTimeLimits.y);
            _orders[orderIndex].Generate(requirements.ToArray(), sprite, avatar, waitingTime, name, monolog);

            var debugText = $"Заказ {orderIndex}: {transport}, ";
            foreach (var requirenment in requirements)
            {
                debugText += $"{requirenment.Item1} {requirenment.Item2} л, ";
            }
            Debug.Log(debugText);
        }

        public void SetActiveOrder(int index, bool value)
        {
            _orders[index].gameObject.SetActive(value);
        }

        public void OnStartLevel()
        {
            for (var i = 0; i < _orders.Length; ++i)
            {
                Generate(i);
            }
        }

        private void CheckDayCompleted()
        {
            foreach (var order in _orders)
            {
                if (order.InProgress)
                {
                    return;
                }
            }
            DayCompleted?.Invoke();
        }
    }
}
