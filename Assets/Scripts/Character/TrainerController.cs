using System.Collections;
using Pokemons;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] private new string name;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Dialog dialog;
    [SerializeField] private Dialog dialogAfterBattle;
    [SerializeField] private GameObject exclamation;
    [SerializeField] private GameObject fov;

    private bool battleLost;
    public string Name => name;
    public Sprite Sprite => sprite;

    private Character character { get; set; }
    public PokemonPartyMono party { get; private set; }
    private void Awake()
    {
        character = GetComponent<Character>();
        party = GetComponent<PokemonPartyMono>();
    }

    private void Start()
    {
        SetFOVRotation(character.Animator.DefaultDirection);
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        //exclamation appear
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        //move toward player
        var diff = player.transform.position - transform.position;
        var moveVector = diff - diff.normalized;
        moveVector = new Vector2(Mathf.Round(moveVector.x), Mathf.Round(moveVector.y));


        yield return character.Move(moveVector);

        //show dialog
        yield return DialogManager.Instance.ShowDialog(dialog,
            () => { GameController.I.StartTrainerBattle(this); });
    }

    private void SetFOVRotation(FacingDirection direction)
    {
        float angle = direction switch
        {
            FacingDirection.Up => 180,
            FacingDirection.Left => 270,
            FacingDirection.Right => 90,
            _ => 0
        };

        fov.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);

        if (battleLost)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfterBattle));
        }
        else
        {
            //show dialog
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog,
                () => { GameController.I.StartTrainerBattle(this); }));
        }
    }

    public void BattleLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }

    public object CaptureState()
    {
        return battleLost;
    }

    public void RestoreState(object state)
    {
        if ((bool) state) BattleLost();
    }
}