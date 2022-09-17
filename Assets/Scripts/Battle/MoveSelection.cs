using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class MoveSelection : MonoBehaviour
    {
        [SerializeField] private Image typeIcon;
        [SerializeField] private TextMeshProUGUI moveText;
        [SerializeField] private TextMeshProUGUI ppText;
        [SerializeField] private TypeSpriteDB typeSprite;

        [SerializeField] private Sprite outlined;
        [SerializeField] private Sprite normal;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }
        public void SetMove(Move.MoveSlot moveSlot)
        {
            typeIcon.sprite = typeSprite.Data[moveSlot.Base.Type].Icon;
            moveText.text = moveSlot.Name;
            ppText.text = $"{moveSlot.Pp}/{moveSlot.MaxPp}";

            image.color = typeSprite.Data[moveSlot.Base.Type].Color;
        }
        public void SetSelected()
        {
            image.sprite = outlined;
        }
        public void SetUnselected()
        {
            image.sprite = normal;
        }
    }
}
