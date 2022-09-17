using System.Collections.Generic;
using System.Linq;
using Pokemons;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISavable
{
    public static PlayerController i { get; private set; }
    
    [SerializeField] private new string name;
    [SerializeField] private Sprite sprite;

    public string Name => name;
    public Sprite Sprite => sprite;

    private Vector2 input;

    public Character character { get; private set; }
    public PokemonPartyMono party { get; private set; }
    private void Awake()
    {
        i = this;
        character = GetComponent<Character>();
        party = GetComponent<PokemonPartyMono>();
    }

    public void HandleUpdate()
    {
        //test battle
        //character.Animator.IsMoving = false;
        //OnEncountered();

        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal movement
            if (input.x != 0) input.y = 0;


            if (input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    private void Interact()
    {
        var faceDirection = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + faceDirection;

        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.Instance.InteractableLayer);
        if (collider is object)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, Character.OffsetY), 0.1f,
            GameLayers.Instance.TriggerableLayers);

        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable is null) continue;
            character.Animator.IsMoving = false;
            triggerable.OnPlayerTriggered(this);
            break;
        }
    }

    public object CaptureState()
    {
        var data = new PlayerSaveData()
        {
            position = transform.position,
            party = party.Pokemons.Select(p => p.GetSaveData()).ToList()
        };


        return data;
    }

    public void RestoreState(object state)
    {
        var data = (PlayerSaveData) state;
        transform.position = data.position;

        party.RestorePartyState(data.party.Select(s => new Pokemon(s)).ToList());
    }
}

public class PlayerSaveData
{
    public Vector3 position;
    public List<PokemonSaveData> party;
}