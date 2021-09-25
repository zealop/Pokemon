using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priority : MoveModifier
{
    [SerializeField] int priority;
    public override void ModifyMove()
    {
        _base.Priority = (s, t) => priority;
    }
}
