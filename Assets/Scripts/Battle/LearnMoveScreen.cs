using System.Linq;
using UnityEngine;
public class LearnMoveScreen : MonoBehaviour
{
    private MoveParty[] moveSlots;

    private BattleUnit unit;
    private int currentMove;
    private void Awake()
    {
        moveSlots = GetComponentsInChildren<MoveParty>(true);
    }

    public void SetMovesData(BattleUnit unit)
    {
        this.unit = unit;
        currentMove = 0;
        for (int i = 0; i < moveSlots.Length; i++)
        {
            moveSlots[i].SetData(unit.Pokemon.Moves[i]);
        }
    }
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove++;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove--;

        currentMove = (currentMove + moveSlots.Length) % moveSlots.Length;

        UpdateMoveSelection();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var forgetMove = unit.Pokemon.Moves[currentMove].Base;
            var newMove = unit.Pokemon.Moves.LastOrDefault().Base;

            unit.Pokemon.Moves.RemoveAt(currentMove);
            unit.Moves.RemoveAt(currentMove);

            StartCoroutine(BattleManager.Instance.CloseMoveScreen(forgetMove, newMove));
        }
    }

    private void UpdateMoveSelection()
    {
        for (int i = 0; i < moveSlots.Length; i++)
        {
            moveSlots[i].SetSelected(i == currentMove);
        }
    }
}
