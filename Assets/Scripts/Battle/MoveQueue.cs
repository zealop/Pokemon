using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    public class MoveQueue
    {
        private readonly List<MoveNode> list = new List<MoveNode>();
        public int Count => list.Count;

        public void Enqueue(Move.Move move, Unit source, Unit target)
        {
            list.Add(new MoveNode(move, source, target));
        }

        public MoveNode Dequeue()
        {
            MoveNode result = null;
            if (list.Count > 0)
            {
                result = list.First();

                result = list.Aggregate(result, FindFaster);
            }

            list.Remove(result);

            return result;
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Cancel(Unit unit)
        {
            var deletedNode = list.FirstOrDefault(node => node.Source.Equals(unit));
            list.Remove(deletedNode);
        }

        private static MoveNode FindFaster(MoveNode node1, MoveNode node2)
        {
            MoveNode result;

            var m1 = node1.Move.Base;
            var s1 = node1.Source;

            var m2 = node2.Move.Base;
            var s2 = node2.Source;

            if (m1.Priority == m2.Priority)
            {
                if (s1.Speed == s2.Speed)
                {
                    result = Random.value > 0.5f ? node1 : node2;
                }
                else
                {
                    result = s1.Speed > s2.Speed ? node1 : node2;
                }
            }
            else
            {
                result = m1.Priority > m2.Priority ? node1 : node2;
            }

            return result;
        }
    }


    public class MoveNode
    {
        public Move.Move Move { get; }
        public Unit Source { get; }
        public Unit Target { get; }

        public MoveNode(Move.Move move, Unit source, Unit target)
        {
            Move = move;
            Source = source;
            Target = target;
        }
    }
}