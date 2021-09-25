using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleHUD : SerializedMonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text statusText;
    [SerializeField] HPBar hpBar;

    BattleUnit unit;

    [OdinSerialize] Dictionary<StatusID, Color> statusColors;
    public void SetData(BattleUnit unit)
    {
        this.unit = unit;

        nameText.text = unit.Name;
        levelText.text = $"Lvl {unit.Level}";
        hpBar.SetHP((float)unit.HP / unit.MaxHP);

        SetStatusText();
    }
  

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)unit.HP / unit.MaxHP);
    }

    public void SetStatusText()
    {
        if (unit.Status is null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = unit.Status.ID.ToString();
            statusText.color = statusColors[unit.Status.ID];
        }
    }
}
