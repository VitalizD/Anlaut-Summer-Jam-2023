using System;
using UnityEngine;

namespace Gameplay.UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class UISlashCounter : MonoBehaviour
    {
        [SerializeField] private bool _debug;
        [SerializeField] private int _initialShashValue;

        private UICounter _counter;

        public static event Action<CounterType> ValueEqualSlashValue;
        
        public int SlashValue { get; private set; }

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        public void SetSlashValue(int value)
        {
            SlashValue = value;
            UpdateText();
        }

        public void SetValue(int value)
        {
            _counter.SetValue(value);
            UpdateText();
            CheckEquals();
        }

        public void AddValue(int value)
        {
            SetValue(_counter.Value + value);
        }

        private void CheckEquals()
        {
            if (_counter.Value == SlashValue)
            {
                ValueEqualSlashValue?.Invoke(_counter.Type);
            }
        }

        private void UpdateText() => _counter.Text.text = $"{_counter.Prefix}{_counter.Value}/{SlashValue}{_counter.Postfix}";
    }
}
