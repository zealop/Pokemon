using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class PartyScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private TypeSpriteDB typeSprite;
        [SerializeField] private Image typeCards1;
        [SerializeField] private Image typeCards2;

        private PartyMemberUI[] memberSlots;
        private MoveParty[] moveSlots;
        private static List<Pokemon> party => BattleManager.I.PlayerParty.Party;
        private static BattleUnit PlayerUnit => BattleManager.I.PlayerUnit;
        private static int MemberCount => party.Count;
        
        private int currentIndex;
        
        public Action<Pokemon> OnSelectMember;
        public Action OnBack;
        
        private void Awake()
        {
            memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
            moveSlots = GetComponentsInChildren<MoveParty>(true);
        }

        public void Init()
        {
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

        private void UpdateMemberSelection(int previousIndex)
        {
            memberSlots[currentIndex].SetSelected();
            memberSlots[previousIndex].SetUnselected();

            SetTypeCards();
            SetMoveList();
        }

        private void SetTypeCards()
        {
            var type1 = party[currentIndex].Base.Type1;
            var type2 = party[currentIndex].Base.Type2;

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

        private void SetMoveList()
        {
            var moves = party[currentIndex].Moves;

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

        private void SetMessageText(string message)
        {
            messageText.text = message;
        }

        public void HandleUpdate()
        {
            int previousIndex = currentIndex;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex++;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex--;
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                ValidateMember();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                OnBack?.Invoke();
            }

            currentIndex = (currentIndex + MemberCount) % MemberCount;
            if (currentIndex != previousIndex) UpdateMemberSelection(previousIndex);
        }

        private void ValidateMember()
        {
            var pokemon = party[currentIndex];
            
            if (!PlayerUnit.CanSwitch())
            {
                SetMessageText($"{PlayerUnit.Name} can't switch out!");
            }
            else if (pokemon.HP <= 0)
            {
                SetMessageText("You can't send out a fainted Pokemon!");
            }
            else if (pokemon == PlayerUnit.Pokemon)
            {
                SetMessageText($"This Pokemon is already out!");
            }
            else
            {
                OnSelectMember(pokemon);
            }
        }
    }
}