using Service.Controllers;
using UnityEngine;

namespace Gameplay.UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class UICounterMoneyHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            GameController.GetMoney += Get;
        }

        private void OnDisable()
        {
            GameController.GetMoney -= Get;
        }

        private int Get() => _counter.Value;
    }
}