using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private MatchFuelLevel[] _newFuelsOnLevels;
        [SerializeField] private MatchTransportCount[] _transportsCountsOnLevels;
        [Tooltip("index + 2 = order number")]
        [SerializeField] private int[] _levelsForNewOrder;
        [SerializeField] private int _level = 1;

        private List<FuelType> _availableFuels;
        private readonly Dictionary<int, List<MatchTransportCount>> _transportsCountsDict = new();
        private List<MatchTransportCount> _remainTransports;
        private bool _gasStationClosed = false;

        public static event Action LevelStarted;
        public static event Action<CounterType, int> SetSlashResource;
        public static event Action<int, bool> SetActiveOrder;
        public static event Action<FuelType, bool> SetActiveToggle;

        private void Awake()
        {
            foreach (var info in _transportsCountsOnLevels)
            {
                if (_transportsCountsDict.ContainsKey(info.Level))
                {
                    _transportsCountsDict[info.Level].Add(info);
                }
                else
                {
                    _transportsCountsDict.Add(info.Level, new List<MatchTransportCount> { info });
                }
            }
        }

        private void Start()
        {
            UpdateGameData();
            StartLevel();
        }

        public TransportType GetNextTransport()
        {
            var info = _remainTransports[UnityEngine.Random.Range(0, _remainTransports.Count)];
            if (--info.Count <= 0)
            {
                _remainTransports.Remove(info);
            }
            return info.Transport;
        }

        public bool FuelIsAvailable(FuelType type) => _availableFuels.Contains(type);

        public void CloseGasStation()
        {
            _gasStationClosed = true;
        }

        public bool GasStationClosed() => _gasStationClosed;

        public void FinishLevel()
        {
            _level++;
            UpdateGameData();
            StartLevel();
        }

        private void UpdateGameData()
        {
            _availableFuels = new List<FuelType>();
            for (var i = 0; i < _newFuelsOnLevels.Length; ++i)
            {
                if (_newFuelsOnLevels[i].RequireLevel <= _level)
                {
                    _availableFuels.Add(_newFuelsOnLevels[i].Fuel);
                }
                SetActiveToggle?.Invoke(_newFuelsOnLevels[i].Fuel, _newFuelsOnLevels[i].RequireLevel <= _level);
            }
            for (var i = 0; i < _levelsForNewOrder.Length; ++i)
            {
                SetActiveOrder?.Invoke(i + 1, _levelsForNewOrder[i] <= _level);
            }
        }

        private void StartLevel()
        {
            _gasStationClosed = false;
            var transportsCount = 0;
            _remainTransports = new List<MatchTransportCount>(_transportsCountsDict[_level]);
            foreach (var info in _remainTransports)
            {
                transportsCount += info.Count;
            }
            SetSlashResource?.Invoke(CounterType.Cars, transportsCount);
            LevelStarted?.Invoke();
        }
    }
}