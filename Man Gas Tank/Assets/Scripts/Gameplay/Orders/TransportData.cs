using System;
using UnityEngine;

namespace Gameplay.Orders
{
    [Serializable]
    public class TransportData
    {
        public TransportType Type;
        public Vector2 WaitingTimeLimits;
        public Vector2 FuelLitersLimits;
        public Sprite[] Sprites;
        public Sprite[] Avatars;
        public FuelType[] RequiredFuels;
        public string[] Names;
        public string[] Phrases;
    }
}
