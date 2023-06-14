using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI.InfoWindow
{
    public class InformationWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _window;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _confirmText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Animator _animator;

        private readonly int ANIM_Showed = Animator.StringToHash("Showed");
        private UnityAction _confirmAction;

        private void Awake()
        {
            _confirmButton.onClick.AddListener(OnClickConfirmButton);
        }

        public void Show(string title, string description, UnityAction action, string confirmButton = "œŒÕﬂ“ÕŒ")
        {
            _title.text = title;
            _description.text = description;
            _confirmAction = action;
            _confirmText.text = confirmButton;
            _window.SetActive(true);
            _animator.SetBool(ANIM_Showed, true);
        }

        public void OnClickConfirmButton()
        {
            Hide();
            if (_confirmAction != null)
            {
                _confirmAction?.Invoke();
            }
        }

        public void Hide()
        {
            _window.SetActive(false);
            _animator.SetBool(ANIM_Showed, false);
        }
    }
}
