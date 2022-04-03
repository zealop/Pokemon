using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Battle
{
    public class DialogBox : MonoBehaviour
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
            yield return messages.Select(TypeDialog).GetEnumerator();
        }
    }
}
