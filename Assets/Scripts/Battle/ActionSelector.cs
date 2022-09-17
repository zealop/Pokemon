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
        private Image[] images;
        private TextMeshProUGUI[] texts;

        public event Action<int> OnSelectAction;

        private void Awake()
        {
            images = GetComponentsInChildren<Image>();
            texts = GetComponentsInChildren<TextMeshProUGUI>();
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
                OnSelectAction?.Invoke(currentIndex);
            }

            currentIndex = (currentIndex + ActionCount) % ActionCount;

            if (previousIndex != currentIndex)
            {
                UpdateActionSelection(previousIndex);
            }
        }

        private void UpdateActionSelection(int previousIndex)
        {
            var currentImage = images[currentIndex];
            var currentText = texts[currentIndex];
            var previousImage = images[previousIndex];
            var previousText = texts[previousIndex];

            currentImage.color = Color.black;
            currentText.color = Color.white;

            previousImage.color = Color.white;
            previousText.color = Color.black;
        }
    }
}