using TMPro;
using UnityEngine;
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

        private void Awake()
        {
            _confirmButton.onClick.AddListener(Hide);
        }

        public void Show(string title, string description, string confirmButton = "œŒÕﬂ“ÕŒ")
        {
            _title.text = title;
            _description.text = description;
            _confirmText.text = confirmButton;
            _window.SetActive(true);
            _animator.SetBool(ANIM_Showed, true);
        }

        public void Hide()
        {
            _window.SetActive(false);
            _animator.SetBool(ANIM_Showed, false);
        }
    }
}
