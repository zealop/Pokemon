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

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void SetData(Move move)
    {
        typeCard.sprite = typeSprite.Data[move.Base.Type].Card;
        moveName.text = move.Name;
        ppText.text = $"{move.PP}/{move.MaxPP}";
    }

    public void SetSelected(bool selected)
    {
        if(selected)
        {
            image.color = Color.black;
            moveName.color = Color.white;
        }
        else
        {
            image.color = Color.white;
            moveName.color = Color.black;
        }
        
    }
}
