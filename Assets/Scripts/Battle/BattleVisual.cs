using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
        hud.gameObject.SetActive(true);
        hud.SetData(unit);

        image.color = originalColor;

        transform.localScale = new Vector3(1, 1, 1);

        PlayEnterAnimation();
    }
    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    public void Transform(PokemonBase pokemon)
    {
        image.sprite = unit.IsPlayerUnit ? pokemon.Sprite.Back : pokemon.Sprite.Front;
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
    public IEnumerator PlayCaptureAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Join(image.DOFade(0, 0.5f));
        sequence.Join(transform.DOLocalMoveY(originalPos.y + 50f, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(0.3f, 0.3f, 1f), 0.5f));

        yield return sequence.WaitForCompletion();
    }

    public IEnumerator PlayBreakoutAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Join(image.DOFade(1, 0.5f));
        sequence.Join(transform.DOLocalMoveY(originalPos.y, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));

        yield return sequence.WaitForCompletion();
    }
}
