using System;
using System.Collections.Generic;
using System.Linq;
using Game.Constants;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Battles
{
    public class StatStage
    {
        private Unit Unit { get; }
        private StageModifier StageModifier => Unit.Battle.Context.StageModifier;

        private float[] NormalModifiers => StageModifier.NormalModifier;
        private float[] PrecisionModifiers => StageModifier.PrecisionModifier;
        private float[] CritModifiers => StageModifier.CritModifier;

        private int PrecisionStageMax => PrecisionModifiers.Length;

        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
        public int Speed { get; set; }
        public int Accuracy { get; set; }
        public int Evasion { get; set; }
        public int Crit { get; set; }

        public float AccuracyModifier(int targetEvasionStage)
        {
            var stage = Mathf.Clamp(Accuracy + targetEvasionStage, -PrecisionStageMax, PrecisionStageMax);
            return stage switch
            {
                > 0 => PrecisionModifiers[stage],
                < 0 => 1 / PrecisionModifiers[^stage],
                _ => 1
            };
        }

        public StatStage(Unit unit)
        {
            Unit = unit;
        }
    }
}