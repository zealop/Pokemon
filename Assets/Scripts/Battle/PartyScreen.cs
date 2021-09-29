using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;

    List<Pokemon> party;
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);

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
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }


    public void SetMessageText(string message)
    {
        messageText.text = message;
    }

}
