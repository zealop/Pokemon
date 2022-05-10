using System.Collections.Generic;

namespace Move
{
    public class DamageDetail
    {
        public readonly int Value;
        public readonly List<string> Messages = new List<string>();

        public DamageDetail(int value)
        {
            Value = value;
        }

        public DamageDetail(int value, string message) : this(value)
        {
            Messages.Add(message);
        }
    }
}