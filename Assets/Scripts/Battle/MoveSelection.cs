using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelection : MonoBehaviour
{
    [SerializeField] Image typeIcon;
    [SerializeField] Text moveName;
    [SerializeField] Text ppText;
    [SerializeField] TypeSpriteDB typeSprite;

    [SerializeField] Sprite outlined;
    [SerializeField] Sprite normal;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void SetMove(Move move)
    {
        typeIcon.sprite = typeSprite.Data[move.Base.Type].Icon;
        moveName.text = move.Name;
        ppText.text = $"{move.PP}/{move.MaxPP}";
 
        image.color = typeSprite.Data[move.Base.Type].Color;
    }
    public void SetSelected(bool select)
    {
        image.sprite = select ? outlined : normal;
    }
}
