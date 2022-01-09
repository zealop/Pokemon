using UnityEngine;

public class TrainerFOV : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        GameController.Instance.OnEnterTrainerFOV(GetComponentInParent<TrainerController>());
    }
}