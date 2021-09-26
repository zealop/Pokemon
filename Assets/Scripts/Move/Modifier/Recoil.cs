using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MoveModifier
{
    [SerializeField] float ratio;
    [SerializeField] bool isStruggle;
    public override void ModifyMove()
    {
        _base.Recoil = NormalRecoil;
    }

    IEnumerator NormalRecoil(int damage, BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;
        int recoil = isStruggle ? source.MaxHP / 4 : Mathf.FloorToInt(damage * ratio);

        yield return source.TakeDamage(recoil);
        yield return dialogBox.TypeDialog($"{source.Name} is damaged from recoil!");

    }
}

public class Drain : MoveModifier
{
    [SerializeField] float ratio;
    public override void ModifyMove()
    {
        _base.Recoil = Draining;
    }

    IEnumerator Draining(int damage, BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;
        int drain = Mathf.FloorToInt(damage * ratio);

        yield return source.TakeDamage(-drain);
        yield return dialogBox.TypeDialog($"{target.Name} had its energy drained!");

    }
}

