using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] TypeSpriteDB typeSprite;
    [SerializeField] Image typeCards1;
    [SerializeField] Image typeCards2;

    PartyMemberUI[] memberSlots;
    MoveParty[] moveSlots;


    List<Pokemon> party;
    private void Awake()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
        moveSlots = GetComponentsInChildren<MoveParty>(true);
    }

    public void SetPartyData(List<Pokemon> party)
    {
        this.party = party;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < party.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].SetData(party[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }

        messageText.text = "Choose a Pokemon";
    }

    public void UpdateMemberSelection(int selected)
    {
        for (int i = 0; i < party.Count; i++)
        {
            if (i == selected)
            {
                memberSlots[i].SetSelected(true);
            }
            else
            {
                memberSlots[i].SetSelected(false);
            }
        }

        SetTypeCards(selected);
        SetMoveList(selected);
    }

    private void SetBattleStateText(int selected)
    {
        //TODO
    }

    private void SetTypeCards(int selected)
    {
        var type1 = party[selected].Base.Type1;
        var type2 = party[selected].Base.Type2;

        typeCards1.sprite = typeSprite.Data[type1].Card;

        if (type2 != PokemonType.None)
        {
            typeCards2.gameObject.SetActive(true);
            typeCards2.sprite = typeSprite.Data[type2].Card;
        }
        else
        {
            typeCards2.gameObject.SetActive(false);
        }

    }

    private void SetMoveList(int selected)
    {
        var moves = party[selected].Moves;

        for (int i = 0; i < moveSlots.Length; i++)
        {
            if (i < moves.Count)
            {
                moveSlots[i].gameObject.SetActive(true);
                moveSlots[i].SetData(moves[i]);
            }
            else
            {
                moveSlots[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetMessageText(string message)
    {
        messageText.text = message;
    }

}
