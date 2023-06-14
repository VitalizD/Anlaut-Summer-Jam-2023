using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.UI
{
    public class UITimer : MonoBehaviour
    {
        [SerializeField] private UIBar _progressBar;

        private UnityAction _execute;
        private float _remainSeconds;

        public bool Activated { get; private set; } = false;

        public float BarValue => _progressBar.Value;

        public void Run(float seconds, UnityAction execute)
        {
            _remainSeconds = seconds;
            _execute = execute;
            Activated = true;
            _progressBar.gameObject.SetActive(true);
            _progressBar.SetValue(1f);
        }

        public void Add(float value) => _progressBar.AddValue(value);

        public void SetActive(bool value) => Activated = value;

        private void Update()
        {
            if (!Activated)
                return;

            _progressBar.AddValue(-Time.deltaTime / _remainSeconds);
            if (_progressBar.Empty)
                Stop();
        }

        private void Stop()
        {
            Activated = false;
            _progressBar.SetValue(0f);
            _progressBar.gameObject.SetActive(false);
            _execute?.Invoke();
        }
    }
}