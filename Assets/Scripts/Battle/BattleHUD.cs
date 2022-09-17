using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Condition;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class BattleHUD : MonoBehaviour
    {
        private const float AnimationDuration = 1.5f;
        private static readonly Color HpGreen = Color.green;
        private static readonly Color HpYellow = Color.yellow;
        private static readonly Color HpRed = Color.red;
        
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image hpBar;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Image expBar;
        [SerializeField] private Image statusImage;

        private bool hasHpText;
        private bool hasExpBar;
            
        private int curHp;
        private int maxHp;

        private int CurHp
        {
            set
            {
                curHp = value;
                SetHpInfo();
            }
        }

        private float NormalizedHp => (float) curHp / maxHp;
        
        private static Queue<IEnumerator> AnimationQueue => BattleManager.i.AnimationQueue;

        public void Bind(Unit unit)
        {
            unit.OnHealthChanged += () => AnimationQueue.Enqueue(UpdateHp(unit.Hp));
            unit.OnStatusChanged += () => AnimationQueue.Enqueue(SetStatusImageCoroutine(unit.status));
        }

        private void Awake()
        {
            hasHpText = hpText != null;
            hasExpBar = expBar != null;
        }

        public void Setup(Unit unit)
        {
            maxHp = unit.MaxHp;
            CurHp = unit.Hp;
            
            nameText.text = unit.Name;
            levelText.text = $"Lvl {unit.Level}";

            SetStatusImage(unit.status);
            
            // if (expBar is object)
            // {
            //     expBar.transform.localScale = new Vector3(normalizedExp, 1);
            // }
            
            gameObject.SetActive(true);
        }

        private IEnumerator UpdateHp(int newHp, float duration = AnimationDuration)
        {
            float timer = duration;
            int hpDiff = curHp - newHp;
            
            while (timer > 0)
            {
                timer = Mathf.Max(0, timer - Time.deltaTime);

                CurHp = newHp + Mathf.FloorToInt(hpDiff * timer / duration);

                yield return null;
            }
        }

        // public IEnumerator UpdateExp()
        // {
        //     if (NormalizedExp > 1f)
        //     {
        //         var transform1 = expBar.transform;
        //         yield return transform1.DOScaleX(1f, 1.5f).WaitForCompletion();
        //         transform1.localScale = new Vector3(0, 1);
        //
        //         yield return unit.LevelUp();
        //         levelText.text = $"Lvl {unit.Level}";
        //
        //         yield return UpdateExp();
        //     }
        //     else
        //     {
        //         yield return expBar.transform.DOScaleX(NormalizedExp, 1.5f).WaitForCompletion();
        //     }
        // }

        private void SetStatusImage(Status status)
        {
            if (status != null)
            {
                statusImage.gameObject.SetActive(true);
                statusImage.sprite = StatusSprite.I.Sprites[status.ID];

                statusImage.DOFade(0, AnimationDuration).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                statusImage.gameObject.SetActive(false);
            }
        }

        private IEnumerator SetStatusImageCoroutine(Status status)
        {
            SetStatusImage(status);
            yield return null;
        }
        private void SetHpInfo()
        {
            hpBar.transform.localScale = new Vector3(NormalizedHp, 1);
            SetHpColor(NormalizedHp);

            if (hasHpText)
            {
                SetHpText(curHp, maxHp);
            }
        }

        private void SetHpColor(float normalizedHp)
        {
            var hpColor = normalizedHp switch
            {
                < 0.25f => HpRed,
                < 0.5f => HpYellow,
                _ => HpGreen
            };

            hpBar.color = hpColor;
        }

        private void SetHpText(int curHp, int maxHp)
        {
            hpText.text = $"{curHp}/{maxHp}";
        }
    }
}