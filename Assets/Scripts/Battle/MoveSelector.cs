using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class MoveSelector : MonoBehaviour
    {
        private MoveSelection[] moveSelections;
        private List<Move.Move> moves;

        private int currentIndex;
        private int MoveCount => moves.Count;
    
        public Action<int> OnSelectMove;
        public Action OnBack;
        private void Awake()
        {
            moveSelections = GetComponentsInChildren<MoveSelection>(true);
        }

        public void SetMoves(List<Move.Move> moves)
        {
            this.moves = moves;

            for (int i = 0; i < moveSelections.Length; i++)
            {
                if (i < moves.Count)
                {
                    moveSelections[i].gameObject.SetActive(true);
                    moveSelections[i].SetMove(moves[i]);
                }
                else
                {
                    moveSelections[i].gameObject.SetActive(false);
                }
            }
        }

        private void UpdateMoveSelection(int previousIndex)
        {
            moveSelections[currentIndex].SetSelected();
            moveSelections[previousIndex].SetUnselected();
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
                OnSelectMove(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                OnBack();
            }
        
            currentIndex = (currentIndex + MoveCount) % MoveCount;
            
            if(previousIndex != currentIndex) UpdateMoveSelection(previousIndex);
        }
    }
}
