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

        private readonly Unit source;
        private readonly MoveBase move;
        private readonly string message;
        public Bound(MoveBase move, Unit source, string message)
        {
            counter = Random.Range(4, 6);

            this.move = move;
            this.source = source;
            this.message = message;
        }

        public override void OnStart()
        {
            unit.Modifier.OnTurnEndList.Add(ResidualDamage);
            unit.CanSwitch += Trapped;
            
            //Todo: implement ending status when source is out
            //source.OnSwitchOut
            
            Log(string.Format(message, source?.Name, unit.Name));
        }

        public override void OnEnd()
        {
            unit.Modifier.OnTurnEndList.Remove(ResidualDamage);
            unit.CanSwitch -= Trapped;
            
            Log($"{unit.Name} is freed from {move.Name}!");
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