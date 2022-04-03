using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class ActionSelector : MonoBehaviour
    {
        private const int ActionCount = 4;

        private int currentIndex;
        private Image[] actions;

        public Action<int> OnSelectAction;

        private void Awake()
        {
            actions = GetComponentsInChildren<Image>();
        }

        public void HandleUpdate()
        {
            int previousIndex = currentIndex;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex++;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex--;
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                OnSelectAction(currentIndex);
            }

            currentIndex = (currentIndex + ActionCount) % ActionCount;

            if (previousIndex != currentIndex) UpdateActionSelection(previousIndex);
        }

        private void UpdateActionSelection(int previousIndex)
        {
            var currentAction = actions[currentIndex];
            var previousAction = actions[previousIndex];

            currentAction.color = Color.black;
            currentAction.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

            previousAction.color = Color.white;
            previousAction.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }
}