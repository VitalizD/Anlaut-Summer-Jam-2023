using System;
using UnityEngine;

namespace Gameplay.Orders
{
    [Serializable]
    public class TransportData
    {
        public TransportType Type;
        public Sprite Sprite;
        public Vector2 WaitingTimeLimits;
        public FuelType[] RequiredFuels;
        public string[] Names;
        public string[] Phrases;
    }
}
