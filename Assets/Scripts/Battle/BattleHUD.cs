using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image expBar;
    [SerializeField] private Image statusImage;
    [SerializeField] private StatusSprite statusSprite;
    private float NormalizedHP => (float) unit.HP / unit.MaxHP;

    private float NormalizedEXP
    {
        get
        {
            int currentLevelEXP = EXPChart.GetEXPAtLevel(unit.Pokemon.Base.GrowthRate, unit.Level);
            int nextLevelEXP = EXPChart.GetEXPAtLevel(unit.Pokemon.Base.GrowthRate, unit.Level + 1);

            return (float) (unit.Pokemon.EXP - currentLevelEXP) / (nextLevelEXP - currentLevelEXP);
        }
    }

    private BattleUnit unit;
    private Sequence statusSequence;

    public void SetData(BattleUnit unit)
    {
        this.unit = unit;

        nameText.text = unit.Name;
        SetLevelText();

        hpBar.transform.localScale = new Vector3(NormalizedHP, 1);

        SetStatusImage();

        if (hpText is object)
        {
            hpText.text = $"{unit.HP}/{unit.MaxHP}";
        }

        if (expBar is object)
        {
            expBar.transform.localScale = new Vector3(NormalizedEXP, 1);
        }
    }

    private void SetLevelText()
    {
        levelText.text = $"Lvl {unit.Level}";
    }

    public IEnumerator UpdateHP()
    {
        if (hpText is object)
        {
            StartCoroutine(UpdateHPTextSmooth(1.5f));
        }

        yield return hpBar.transform.DOScaleX(NormalizedHP, 1.5f).WaitForCompletion();
    }

    private IEnumerator UpdateHPTextSmooth(float timer)
    {
        int curHp = int.Parse(hpText.text.Split('/')[0]);
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            int hp = unit.HP + Mathf.FloorToInt((curHp - unit.HP) * timer / 1.5f);
            hpText.text = $"{hp}/{unit.MaxHP}";
            yield return null;
        }
    }

    public IEnumerator UpdateEXP()
    {
        if (NormalizedEXP > 1f)
        {
            yield return expBar.transform.DOScaleX(1f, 1.5f).WaitForCompletion();
            expBar.transform.localScale = new Vector3(0, 1);

            yield return unit.LevelUp();
            SetLevelText();

            yield return UpdateEXP();
        }
        else
        {
            yield return expBar.transform.DOScaleX(NormalizedEXP, 1.5f).WaitForCompletion();
        }
    }

    public void SetStatusImage()
    {
        var status = unit.Status;

        if (status is object)
        {
            statusImage.gameObject.SetActive(true);
            statusImage.sprite = statusSprite.Sprites[status.ID];

            statusSequence = DOTween.Sequence();
            statusSequence.Append(statusImage.DOFade(0, 1));
            statusSequence.Append(statusImage.DOFade(1, 1));
            statusSequence.SetLoops(-1);
        }
        else
        {
            statusImage.gameObject.SetActive(false);
            statusSequence.Kill();
        }
    }
}