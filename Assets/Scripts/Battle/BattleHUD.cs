using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class BattleHUD : MonoBehaviour
    {
        private const float AnimationDuration = 1.5f;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image hpBar;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Image expBar;
        [SerializeField] private Image statusImage;

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

        private static readonly Color HpGreen = Color.green;
        private static readonly Color HpYellow = Color.yellow;
        private static readonly Color HpRed = Color.red;

        private static Queue<IEnumerator> AnimationQueue => BattleManager.I.AnimationQueue;
        
        public void Setup(Unit unit)
        {
            maxHp = unit.MaxHp;
            CurHp = unit.Hp;
            unit.OnHealthChanged += () => AnimationQueue.Enqueue(UpdateHp(unit.Hp));
            unit.OnStatusChanged += () => AnimationQueue.Enqueue(SetStatusImageCoroutine(unit.Status));
            
            nameText.text = unit.Name;
            levelText.text = $"Lvl {unit.Level}";

            SetStatusImage(unit.Status);
            
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

        private void SetStatusImage(StatusCondition status)
        {
            if (status is object)
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

        private IEnumerator SetStatusImageCoroutine(StatusCondition status)
        {
            SetStatusImage(status);
            yield return null;
        }
        private void SetHpInfo()
        {
            hpBar.transform.localScale = new Vector3(NormalizedHp, 1);
            SetHpColor(NormalizedHp);

            if (hpText is object)
            {
                SetHpText(curHp, maxHp);
            }
        }

        private void SetHpColor(float normalizedHp)
        {
            var hpColor = HpGreen;
            if (normalizedHp < 0.25f)
            {
                hpColor = HpRed;
            }
            else if (normalizedHp < 0.5f)
            {
                hpColor = HpYellow;
            }

            hpBar.color = hpColor;
        }

        private void SetHpText(int curHp, int maxHp)
        {
            hpText.text = $"{curHp}/{maxHp}";
        }
    }
}