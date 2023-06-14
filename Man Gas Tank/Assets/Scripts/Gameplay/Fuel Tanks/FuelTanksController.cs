using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.FuelTanks
{
    public class FuelTanksController : MonoBehaviour
    {
        [SerializeField] private FuelTank[] _fuelTanks;
        [SerializeField] private MatchFuelColor[] _fuelColors;

        private readonly Dictionary<FuelType, Color> _fuelColorsDict = new();

        public static event Action NoFreeTanks;

        private void Awake()
        {
            foreach (var fuelColor in _fuelColors)
            {
                _fuelColorsDict.Add(fuelColor.FuelType, fuelColor.Color);
            }
        }

        public void Fill(FuelType fuelType)
        {
            var noFuel = new List<FuelTank>();
            var partialFuel = new List<FuelTank>();
            foreach (var tank in _fuelTanks)
            {
                if (!tank.gameObject.activeSelf) 
                {
                    continue;
                }
                if (tank.FuelType == FuelType.None)
                {
                    noFuel.Add(tank);
                }
                else if (tank.FuelType == fuelType && !tank.IsFull)
                {
                    partialFuel.Add(tank);
                }                 
            }
            if (partialFuel.Count > 0)
            {
                partialFuel[0].StartFill(fuelType, GetFuelColor(fuelType));
            }
            else if (noFuel.Count > 0)
            {
                noFuel[0].StartFill(fuelType, GetFuelColor(fuelType));
            }
            else
            {
                NoFreeTanks?.Invoke();
            }
        }

        public void StopFill(FuelType fuelType)
        {
            foreach (var tank in _fuelTanks)
            {
                if (tank.FuelType == fuelType)
                {
                    tank.StopFill();
                }
            }
        }

        public Color GetFuelColor(FuelType fuelType) => _fuelColorsDict[fuelType];

        public void ClearAll()
        {
            foreach (var tank in _fuelTanks)
            {
                tank.Clear();
            }
        }

        public void SetCount(int value)
        {
            for (var i = 0; i < _fuelTanks.Length; ++i)
            {
                _fuelTanks[i].gameObject.SetActive(i < value);
            }
        }
    }
}
