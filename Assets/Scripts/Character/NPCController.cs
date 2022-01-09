using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog dialog;
    [SerializeField] private List<Vector2> movementPattern;
    [SerializeField] private float timeBetweenPattern;

    private NPCState state;
    private float idleTimer;
    private int currentPattern;
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(initiator.position);

            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
            {
                idleTimer = 0;
                state = NPCState.Idle;
            }));
        }
    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0;
                if (movementPattern.Count > 0)

                    StartCoroutine(Walk());
            }
        }

        character.HandleUpdate();
    }

    private IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPosition = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        if (transform.position != oldPosition)
        {
            currentPattern = (currentPattern + 1) % movementPattern.Count;
        }

        state = NPCState.Idle;
    }
}

public enum NPCState
{
    Idle,
    Walking,
    Dialog
}