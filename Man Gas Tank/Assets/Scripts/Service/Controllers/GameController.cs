using Service.Storages;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Service.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private MatchFuelLevel[] _newFuelsOnLevels;
        [SerializeField] private MatchTransportCount[] _transportsCountsOnLevels;
        [Tooltip("index + 2 = order number")]
        [SerializeField] private int[] _levelsForNewOrder;
        [Tooltip("index + 1 = level")]
        [SerializeField] private int[] _fuelTanksOnLevels;
        [Tooltip("index + 1 = level")]
        [Multiline]
        [SerializeField] private string[] _introductionOnLevels;
        [SerializeField] private LevelData _levelData;
        [SerializeField] private int _level = 1;

        private List<FuelType> _availableFuels;
        private readonly Dictionary<int, List<MatchTransportCount>> _transportsCountsDict = new();
        private List<MatchTransportCount> _remainTransports;
        private bool _gasStationClosed = false;

        public static event Action LevelStarted;
        public static event Action LevelCompleted;
        public static event Action<CounterType, int> SetSlashResource;
        public static event Action<CounterType, int> SetResource;
        public static event Action<CounterType, int> AddResource;
        public static event Action<int, bool> SetActiveOrder;
        public static event Action<FuelType, bool> SetActiveToggle;
        public static event Action<int> SetFuelTanksCount;
        public static event Action<string, string, UnityAction, string> ShowInfoWindow;
        public static event Func<int> GetMoney;
        public static event Func<int> GetDay;

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
            LevelCompleted?.Invoke();
            _level++;
            AddResource?.Invoke(CounterType.Day, 1);
            SetResource?.Invoke(CounterType.Cars, 0);
            UpdateGameData();
            var statistics = $"Обслужено ТС: {_levelData.ServicedCars}\nЗаработано: {_levelData.Money} $\n" +
                $"Всего: {GetMoney()} $";
            ShowInfoWindow?.Invoke("Уровень пройден!", statistics, StartLevel, "ДАЛЬШЕ");
            _levelData.Clear();
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
            SetFuelTanksCount?.Invoke(_fuelTanksOnLevels[_level - 1]);
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
            UnityAction action = LevelStarted.Invoke;
            var confirmText = "ПОНЯТНО";
            if (_level <= 1)
            {
                action = () => ShowInfoWindow?.Invoke($"День {GetDay()}", "Сегодня клиентов не так много, ведь ты только открыл свою заправку. " +
                    "Самое время их хорошо обслужить, чтобы сарафанное радио сделало своё дело. Да, тебе пока больше не на что надеяться." +
                    "\n\n<color=red>ВАЖНО!!!</color> Запомни: легковым автомобилям и мотоциклам нужно столько бензина, сколько они закажут.",
                    LevelStarted.Invoke, "ПОНЯТНО");
            }
            else if (_level >= 6)
            {
                action = () => SceneManager.LoadScene(0);
                confirmText = "ЗАНОВО";
            }
            ShowInfoWindow?.Invoke($"День {GetDay()}", $"{_introductionOnLevels[_level - 1]}", action, confirmText);
        }

        public void ShowHelp()
        {
            var introduction = $"{_introductionOnLevels[_level - 1]}";
            if (_level <= 1)
            {
                introduction = "Сегодня клиентов не так много, ведь ты только открыл свою заправку. " +
                    "Самое время их хорошо обслужить, чтобы сарафанное радио сделало своё дело. Да, тебе пока больше не на что надеяться." +
                    "\n\n<color=red>ВАЖНО!!!</color> Запомни: легковым автомобилям и мотоциклам нужно столько бензина, сколько они закажут.";
            }
            ShowInfoWindow?.Invoke($"День {GetDay()}", introduction, null, "ПОНЯТНО");
        }
    }
}