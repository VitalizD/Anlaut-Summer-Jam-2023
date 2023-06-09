using UnityEngine;

namespace Gameplay.UI.InfoWindow
{
    [RequireComponent(typeof(InformationWindow))]
    public class InformationWindowHandler : MonoBehaviour
    {
        private InformationWindow _informationWindow;

        private void Awake()
        {
            _informationWindow = GetComponent<InformationWindow>();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}
