using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    public event System.Action<int> onMenuSelected;
    public event System.Action onBack;

    private List<Text> menuItems;
    private int currentSelection;
    private void Awake()
    {
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
        UpdateItemSelection();
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
    }
    public void HandleUpdate()
    {
        int previousSelection = currentSelection;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSelection++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentSelection--;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            onMenuSelected?.Invoke(currentSelection);
            CloseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
            CloseMenu();
        }
        currentSelection = (currentSelection + menuItems.Count) % menuItems.Count;

        if (currentSelection != previousSelection)
            UpdateItemSelection();
    }
    private void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].color = i == currentSelection ? Color.blue : Color.black;
        }
    }
}
