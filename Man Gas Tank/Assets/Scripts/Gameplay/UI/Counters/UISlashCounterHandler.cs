using Gameplay.Orders;
using Service.Controllers;
using UnityEngine;

namespace Gameplay.UI.Counters
{
    [RequireComponent(typeof(UISlashCounter))]
    [RequireComponent(typeof(UICounter))]
    public class UISlashCounterHandler : MonoBehaviour
    {
        private UISlashCounter _slashCounter;
        private UICounter _counter;

        private void Awake()
        {
            _slashCounter = GetComponent<UISlashCounter>();
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            Order.AddSlashResource += AddValue;
            GameController.SetSlashResource += SetSlashValue;
        }

        private void OnDisable()
        {
            Order.AddSlashResource -= AddValue;
            GameController.SetSlashResource -= SetSlashValue;
        }

        private void SetValue(CounterType type, int value)
        {
            if (TypeEqual(type))
            {
                _slashCounter.SetValue(value);
            }
        }

        private void AddValue(CounterType type, int value)
        {
            if (TypeEqual(type))
            {
                _slashCounter.AddValue(value);
            }
        }

        private void SetSlashValue(CounterType type, int value)
        {
            if (TypeEqual(type))
            {
                _slashCounter.SetSlashValue(value);
            }
        }

        private bool TypeEqual(CounterType type) => _counter.Type == type;
    }
}