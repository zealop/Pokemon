using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class MoveParty : MonoBehaviour
    {
        [SerializeField] private Image typeCard;
        [SerializeField] private TextMeshProUGUI moveText;
        [SerializeField] private TextMeshProUGUI ppText;
        [SerializeField] private TypeSpriteDB typeSprite;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void SetData(Move.Move move)
        {
            typeCard.sprite = typeSprite.Data[move.Base.Type].Card;
            moveText.text = move.Name;
            ppText.text = $"{move.Pp}/{move.MaxPp}";
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                image.color = Color.black;
                moveText.color = Color.white;
            }
            else
            {
                image.color = Color.white;
                moveText.color = Color.black;
            }
        }
    }
}