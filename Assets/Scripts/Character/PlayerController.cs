using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnEncountered;

    private Vector2 input;

    private CharacterAnimator animator;
    private Character character;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        character = GetComponent<Character>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        //test battle
        //animator.SetBool("isMoving", false);
        //OnEncountered();

        if (!animator.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal movement
            if (input.x != 0) input.y = 0;


            if (input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, CheckForEncounters));
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    void Interact()
    {
        var faceDirection = new Vector3(animator.MoveX, animator.MoveY);
        var interactPos = transform.position + faceDirection;

        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider is object)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }

    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.GrassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                animator.IsMoving = false;
                OnEncountered();
            }
        }
    }
}
