using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    //[SerializeField] HPBar hpBar;

    [SerializeField] Color highlightedColor;
    Pokemon _pokemon;
    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = $"Lvl {pokemon.Level}";
        //hpBar.SetHP(pokemon.HP, pokemon.MaxHP);
    }

    public void SetSelected(bool selected)
    {
        nameText.color = selected ? highlightedColor : Color.black;
    }
}
