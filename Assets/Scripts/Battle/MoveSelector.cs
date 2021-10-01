using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    MoveSelection[] moveSelections;
    List<Move> moves;
    private void Awake()
    {
        moveSelections = GetComponentsInChildren<MoveSelection>(true);
    }

    public void SetMoves(List<Move> moves)
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

    public void UpdateMoveSelection(int currentMove)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            if(i == currentMove)
            {
                moveSelections[i].SetSelected(true);
            }
            else
            {
                moveSelections[i].SetSelected(false);
            }
        }
    }
}
