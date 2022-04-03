using Battle;
using Move;
using UnityEngine;

namespace Data.Volatile
{
    public class Bound : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Bound;

        private int counter;
        private const float Damage = 1 / 8f;

        private readonly BattleUnit source;
        private readonly MoveBase move;

        public Bound(MoveBase move, BattleUnit source)
        {
            counter = Random.Range(4, 6);

            this.move = move;
            this.source = source;
        }

        public override void OnStart()
        {
            unit.Modifier.OnTurnEndList.Add(ResidualDamage);
            unit.CanSwitch += Trapped;
            
            //implement ending status when source is out
            //source.OnSwitchOut
        }

        public override void OnEnd()
        {
            unit.Modifier.OnTurnEndList.Remove(ResidualDamage);
            unit.CanSwitch -= Trapped;
            
            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is freed from {move.Name}!"));
        }

        private void ResidualDamage()
        {
            if (counter > 0)
            {
                counter--;
                var damage = new DamageDetail(Mathf.FloorToInt(unit.MaxHp * Damage),
                    $"{unit.Name} is hurt by {move.Name}!");
                unit.TakeDamage(damage);
            }
            else
            {
                unit.RemoveVolatileCondition(ID);
            }
        }

        private bool Trapped()
        {
            return unit.Types.Contains(PokemonType.Ghost);
        }
    }
}