using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleVisual : MonoBehaviour
{
    
    [SerializeField] BattleHUD hud;

    BattleUnit unit;

    public BattleHUD HUD { get => hud; }

    Image image;
    Vector3 originalPos;
    Color originalColor;
    private void Awake()
    {
        image = GetComponent<Image>();

        originalPos = image.transform.localPosition;
        originalColor = image.color;

        unit = GetComponent<BattleUnit>();
    }

    public void Setup()
    {
        hud.SetData(unit);

        image.color = originalColor;

        PlayEnterAnimation();
    }
    public void Transform(PokemonBase pokemon)
    {
        image.sprite = unit.IsPlayerUnit ? pokemon.BackSprite : pokemon.FrontSprite;
    }
    public void PlayEnterAnimation()
    {
        float x = unit.IsPlayerUnit ? -500f : 500f;

        image.transform.localPosition = new Vector3(x, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();

        float x = unit.IsPlayerUnit ? 50f : -50f;

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x + x, 0.25f));
        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

}
