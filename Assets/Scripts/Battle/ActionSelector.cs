using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelector : MonoBehaviour
{
    private Image[] actions;
    private void Awake()
    {
        actions = GetComponentsInChildren<Image>();
    }

    public void UpdateActionSelection(int currentAction)
    {
        for (int i = 0; i < 4; i++)
        {
            actions[i].color = i == currentAction ? Color.black : Color.white;
            actions[i].GetComponentInChildren<TextMeshProUGUI>().color = i == currentAction ? Color.white : Color.black;
        }
    }
}
