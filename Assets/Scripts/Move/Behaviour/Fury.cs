using System;
using System.Linq;
using Battle;
using Random = UnityEngine.Random;

namespace Move.Behaviour
{
    public class Fury : Default
    {
        private static int HitCount
        {
            get
            {
                int n1 = Random.value <= 0.3f ? 2 : 0;
                int n2 = Random.value <= 0.5f ? 1 : 0;

                return 2 + n1 + n2;
            }
        }

        public override void Apply(Unit source, Unit target, Action consumePp)
        {
            RegisterMove(source, consumePp);

            bool isHit = move.AccuracyCheck.Apply(source, target);
            if (!isHit)
            {
                source.Modifier.OnMiss();
                return;
            }

            int hitCount = HitCount;
            foreach (int unused in Enumerable.Range(1, hitCount))
            {
                ApplyDamage(source, target);
            }
        }
    }
}