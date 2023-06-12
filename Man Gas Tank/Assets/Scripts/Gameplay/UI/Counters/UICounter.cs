using System;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Counters
{
    public class UICounter : MonoBehaviour
    {
        [SerializeField] private CounterType _type;
        [SerializeField] private string _prefix;
        [SerializeField] private string _postfix;
        [SerializeField] private TMP_Text _text;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private int _initialValue;

        public static event Action<CounterType, int> CounterChanged;

        public int Value { get; private set; }
        public CounterType Type => _type;
        public string Prefix => _prefix;
        public string Postfix => _postfix;
        public TMP_Text Text => _text;

        private void Awake()
        {
            if (_debug)
                SetValue(_initialValue);
        }

        public void SetValue(int value)
        {
            Value = Mathf.Clamp(value, 0, int.MaxValue);
            UpdateText();
            CounterChanged?.Invoke(_type, Value);
        }

        public void AddValue(int value)
        {
            SetValue(Value + value);
        }

        private void UpdateText() => _text.text = $"{_prefix}{Value}{_postfix}";
    }
}