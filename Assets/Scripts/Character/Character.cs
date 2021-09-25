using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    CharacterAnimator animator;
    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }
    public IEnumerator Move(Vector2 moveVector, System.Action OnMoveOver = null)
    {
        animator.MoveX = moveVector.x;
        animator.MoveY = moveVector.y;

        var targetPos = transform.position;
        targetPos.x += moveVector.x;
        targetPos.y += moveVector.y;

        if (!IsWalkable(targetPos))
            yield break;

        animator.IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        animator.IsMoving = false;

        OnMoveOver?.Invoke();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

}
