using System.Collections;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable
{
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfterBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    bool battleLost = false;
    public string Name { get => name; }
    public Sprite Sprite { get => sprite; }



    Character character;
    private void Awake()
    {
        character = GetComponent<Character>();
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
        yield return DialogManager.Instance.ShowDialog(dialog, () =>
        {
            GameController.Instance.StartTrainerBattle(this);
        });
    }

    public void SetFOVRotation(FacingDirection direction)
    {
        float angle = 0;
        switch (direction)
        {
            case FacingDirection.Up:
                angle = 180;
                break;
            case FacingDirection.Left:
                angle = 270;
                break;
            case FacingDirection.Right:
                angle = 90;
                break;
        }

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
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
            {
                GameController.Instance.StartTrainerBattle(this);
            }));
        }


    }

    public void BatteLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }
}


