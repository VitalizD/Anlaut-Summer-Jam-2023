using Service.Controllers;
using UnityEngine;

namespace Gameplay.UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class UICounterDayHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            GameController.GetDay += Get;
        }

        private void OnDisable()
        {
            GameController.GetDay -= Get;
        }

        private int Get() => _counter.Value;
    }
}
