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
        public void SetMove(Move.Move move)
        {
            typeIcon.sprite = typeSprite.Data[move.Base.Type].Icon;
            moveText.text = move.Name;
            ppText.text = $"{move.Pp}/{move.MaxPp}";

            image.color = typeSprite.Data[move.Base.Type].Color;
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
