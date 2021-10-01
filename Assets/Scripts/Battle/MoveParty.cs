using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveParty : MonoBehaviour
{
    [SerializeField] Image typeCard;
    [SerializeField] Text moveName;
    [SerializeField] Text ppText;
    [SerializeField] TypeSpriteDB typeSprite;

    public void SetData(Move move)
    {
        typeCard.sprite = typeSprite.Data[move.Base.Type].Card;
        moveName.text = move.Name;
        ppText.text = $"{move.PP}/{move.MaxPP}";
    }
}
