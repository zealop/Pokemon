using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    private MoveSelection[] moveSelections;
    private List<Move> moves;
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
            moveSelections[i].SetSelected(i == currentMove);
        }
    }
}
