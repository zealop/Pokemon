using Battle;

namespace Move.Behaviour
{
    public class LowKick : Default
    {
        public override void Apply(Unit source, Unit target)
        {
            ModifyPower(target);
            base.Apply(source, target);
        }

        private void ModifyPower(Unit target)
        {
            int weight = target.Weight;
            int modifier = weight switch
            {
                < 100 => 1,
                < 250 => 2,
                < 500 => 3,
                < 1000 => 4,
                < 2000 => 5,
                _ => 6
            };

            moveBuilder.Power(moveBuilder.power * modifier);
        }
    }
}