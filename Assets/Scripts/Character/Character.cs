using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool IsMoving { get; private set; }
    public CharacterAnimator Animator { get; private set; }
    public static float OffsetY => 0.3f;

    private void Awake()
    {
        Animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    public IEnumerator Move(Vector2 moveVector, Action OnMoveOver = null)
    {
        Animator.MoveX = Mathf.Clamp(moveVector.x, -1, 1);
        Animator.MoveY = Mathf.Clamp(moveVector.y, -1, 1);

        var targetPos = transform.position;
        targetPos.x += moveVector.x;
        targetPos.y += moveVector.y;

        if (!IsPathClear(targetPos))
            yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        Animator.IsMoving = IsMoving;
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var direction = diff.normalized;
        int layers = GameLayers.Instance.SolidLayer | GameLayers.Instance.InteractableLayer |
                     GameLayers.Instance.PlayerLayer;


        return !Physics2D.BoxCast(transform.position + direction, new Vector2(0.2f, 0.2f), 0f, direction,
            diff.magnitude - 1, layers);
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.1f,
            GameLayers.Instance.SolidLayer | GameLayers.Instance.InteractableLayer) == null;
    }

    public void LookTowards(Vector3 targetPos)
    {
        int xdiff = Mathf.FloorToInt(targetPos.x) - Mathf.FloorToInt(transform.position.x);
        int ydiff = Mathf.FloorToInt(targetPos.y) - Mathf.FloorToInt(transform.position.y);

        if (xdiff == 0 || ydiff == 0)
        {
            Animator.MoveX = Mathf.Clamp(xdiff, -1, 1);
            Animator.MoveY = Mathf.Clamp(ydiff, -1, 1);
        }
        else
        {
            Debug.LogError("Character diagonal look is invalid!");
        }
    }
}