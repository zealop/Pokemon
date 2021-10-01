using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelector : MonoBehaviour
{
    Image[] actions;
    private void Awake()
    {
        actions = GetComponentsInChildren<Image>();
    }

    public void UpdateActionSelection(int currentAction)
    {
        for(int i = 0; i< 4; i++)
        {
            actions[i].color = i == currentAction ? Color.black : Color.white;
            actions[i].GetComponentInChildren<Text>().color = i == currentAction ? Color.white : Color.black;
        }
    }
}
