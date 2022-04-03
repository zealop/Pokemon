using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class BattleVisual : MonoBehaviour
    {
        [SerializeField] private bool isPlayerSprite;
        
        private static Queue<IEnumerator> AnimationQueue => BattleManager.I.AnimationQueue;

        private Image image;
        private Vector3 originalPos;
        private Color originalColor;
        private void Awake()
        {
            image = GetComponent<Image>();

            originalPos = image.transform.localPosition;
            originalColor = image.color;
        }

        public void Setup(BattleUnit unit)
        {
            unit.OnHit += () => AnimationQueue.Enqueue(PlayHitAnimation());
            unit.OnFaint += () => AnimationQueue.Enqueue(PlayFaintAnimation());
            unit.OnAttack += () => AnimationQueue.Enqueue(PlayAttackAnimation());
            
            image.color = originalColor;

            transform.localScale = new Vector3(1, 1, 1);

            SetSprite(unit.Pokemon.Base);
        }
        
        public IEnumerator Transform(PokemonBase pokemon)
        {
            SetSprite(pokemon);
            yield return null;
        }

        private void SetSprite(PokemonBase pokemon)
        {
            image.sprite = isPlayerSprite ? pokemon.Sprite.Back : pokemon.Sprite.Front;
        }
        
        
        public IEnumerator PlayEnterAnimation()
        {
            float x = isPlayerSprite ? -500f : 500f;

            image.transform.localPosition = new Vector3(x, originalPos.y);
            
            gameObject.SetActive(true);
            
            yield return image.transform.DOLocalMoveX(originalPos.x, 1f).WaitForCompletion();
        }

        public IEnumerator PlayAttackAnimation()
        {
            var sequence = DOTween.Sequence();

            float x = isPlayerSprite ? 50f : -50f;

            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + x, 0.25f));
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));

            yield return sequence.WaitForCompletion();
        }

        private IEnumerator PlayHitAnimation()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(image.DOColor(Color.gray, 0.1f));
            sequence.Append(image.DOColor(originalColor, 0.1f));

            yield return sequence.WaitForCompletion();
        }

        public IEnumerator PlayFaintAnimation()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
            sequence.Join(image.DOFade(0f, 0.5f));

            yield return sequence.WaitForCompletion();
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
}
