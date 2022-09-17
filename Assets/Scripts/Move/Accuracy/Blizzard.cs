using Battle;

namespace Move.Accuracy
{
    public class Blizzard : Default
    {
        public override bool Apply(Unit source, Unit target)
        {
            //TODO: bypass acc check when Hail
            // var a = true;
            return base.Apply(source, target);
        }
    }
}