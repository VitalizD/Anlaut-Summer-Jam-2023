using Gameplay.Orders;
using Service.Controllers;
using UnityEngine;

namespace Gameplay.UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class UICounterHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            Order.AddResource += AddValue;
            GameController.SetResource += SetValue;
        }

        private void OnDisable()
        {
            Order.AddResource -= AddValue;
            GameController.SetResource -= SetValue;
        }

        private void SetValue(CounterType type, int value)
        {
            if (TypeEqual(type))
                _counter.SetValue(value);
        }

        private void AddValue(CounterType type, int value)
        {
            if (TypeEqual(type))
                _counter.AddValue(value);
        }

        private bool TypeEqual(CounterType type) => type == _counter.Type;
    }
}
