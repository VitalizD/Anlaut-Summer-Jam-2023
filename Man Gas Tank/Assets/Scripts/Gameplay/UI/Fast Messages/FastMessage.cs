using UnityEngine;
using TMPro;

namespace Gameplay.UI.FastMessages
{
    public class FastMessage : MonoBehaviour
    {
        [SerializeField] private GameObject _message;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Animator _animator;

        private readonly int ANIM_Show = Animator.StringToHash("Show");
        private Camera _mainCamera;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _mainCamera = Camera.main;
            _message.SetActive(false);
        }

        public void Show(string message)
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.Lerp(Vector3.zero, new Vector3(mousePosition.x, mousePosition.y, 0f), 0.7f);
            _text.text = message;
            _message.SetActive(true);
            _animator.SetTrigger(ANIM_Show);
        }
    }
}
