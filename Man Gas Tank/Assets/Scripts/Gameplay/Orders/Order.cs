using Gameplay.FuelTanks;
using Gameplay.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private Animator _monologAnimator;
        [SerializeField] private Animator _carAnimator;
        [SerializeField] private Animator _timerAnimator;
        [SerializeField] private Image _carImage;
        [SerializeField] private Image _avatarImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _monologText;
        [SerializeField] private TMP_Text[] _requirementTexts;
        [SerializeField] private AnimationClip _exitAnimationClip;
        [SerializeField] private UITimer _timer;

        private readonly int ANIM_Enter = Animator.StringToHash("Enter");
        private readonly int ANIM_Exit = Animator.StringToHash("Exit");
        private readonly int ANIM_Showed = Animator.StringToHash("Showed");
        private int _id;
        private (FuelType FuelType, float Count)[] _requirements = new (FuelType, float)[2];

        public static event Action<CounterType, int> AddResource;
        public static event Action<CounterType, int> AddSlashResource;
        public static event Action<int> GenerateNewOrder;

        public bool InProgress { get; private set; } = false;

        public void Initialize(int id)
        {
            _id = id;
        }

        public void Generate((FuelType FuelType, float Count)[] requirements, Sprite car, Sprite avatar,
            float waitingTime, string name, string monolog)
        {
            InProgress = true;
            _requirements = requirements;
            _carImage.sprite = car;
            _avatarImage.sprite = avatar;
            _nameText.text = name;
            _monologText.text = monolog;
            _timerAnimator.SetBool(ANIM_Showed, true);
            for (var i = 0; i < _requirementTexts.Length; ++i)
            {
                if (i < requirements.Length)
                {
                    _requirementTexts[i].text = $"{requirements[i].Count} כ";
                }
                _requirementTexts[i].gameObject.SetActive(i < requirements.Length);
            }
            _carImage.gameObject.SetActive(true);
            _monologAnimator.gameObject.SetActive(true);
            _carAnimator.SetTrigger(ANIM_Enter);
            _monologAnimator.SetBool(ANIM_Showed, true);
            AddSlashResource?.Invoke(CounterType.Cars, 1);
            _timer.Run(waitingTime, Remove);
            StartCoroutine(a());

            IEnumerator a()
            {
                yield return new WaitForEndOfFrame();
                _timerAnimator.SetBool(ANIM_Showed, true);
            }
        }

        public void AcceptIncomingTank(FuelTank tank)
        {
            if (_requirements == null || _requirements.Length == 0)
            {
                return;
            }
            for (var i = 0; i < _requirements.Length; ++i)
            {
                if (_requirements[i].FuelType == tank.FuelType)
                {
                    if (_requirements[i].Count <= 0)
                    {
                        // רענאפ
                    }
                    else if (tank.LitersCount > _requirements[i].Count)
                    {
                        tank.AddFuel(-_requirements[i].Count);
                        UpdateRequirement(i, 0f);
                    }
                    else
                    {
                        UpdateRequirement(i, _requirements[i].Count - tank.LitersCount);
                        tank.Clear();
                    }
                    CheckCompleted();
                    return;
                }
            }
            // רענאפ
        }

        private void Remove()
        {
            _requirements = null;
            _timer.SetActive(false);
            for (var i = 0; i < _requirementTexts.Length; ++i)
            {
                _requirementTexts[i].gameObject.SetActive(false);
            }
            _carAnimator.SetTrigger(ANIM_Exit);
            _monologAnimator.SetBool(ANIM_Showed, false);            
            _timerAnimator.SetBool(ANIM_Showed, false);
            StartCoroutine(WaitAndGenerate());
        }

        private void UpdateRequirement(int index, float value)
        {
            _requirements[index].Count = value;
            _requirementTexts[index].text = $"{Mathf.Ceil(_requirements[index].Count)} כ";
        }

        private void CheckCompleted()
        {
            for (var i = 0; i < _requirements.Length; ++i)
            {
                if (_requirements[i].Count > 0)
                {
                    return;
                }
            }
            AddResource?.Invoke(CounterType.Money, 10);
            Remove();
        }

        private IEnumerator WaitAndGenerate()
        {
            yield return new WaitForSeconds(_exitAnimationClip.length);
            InProgress = false;
            GenerateNewOrder?.Invoke(_id);
        }
    }
}
