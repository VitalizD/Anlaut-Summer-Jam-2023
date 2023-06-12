﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gameplay.UI
{
    public class UIBar : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Color _barPositiveColor;
        [SerializeField] private Color _barNegativeColor;

        public bool Filled { get => bar.fillAmount >= 0.99f; }

        public bool Empty { get => bar.fillAmount <= 0.01f; }

        public float Value { get => bar.fillAmount; }

        /// <param name="value">From 0 to 1</param>
        public void SetValue(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            bar.fillAmount = value;
            bar.color = Color.Lerp(_barNegativeColor, _barPositiveColor, value);
            if (valueText != null)
                valueText.text = $"{(int)value} %";
        }

        public void AddValue(float value)
        {
            SetValue(bar.fillAmount + value);
        }

        public void SetTitle(string value)
        {
            if (title == null)
                return;

            title.text = value;
        }
    }
}