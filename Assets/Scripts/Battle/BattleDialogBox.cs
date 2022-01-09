using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] private int lettersPerSecond;


    [SerializeField] private TextMeshProUGUI dialogText;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        //yield return new WaitForSeconds(1f);
    }

    public IEnumerator TypeDialog(IEnumerable<string> messages)
    {
        return messages.Select(TypeDialog).GetEnumerator();
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
}
