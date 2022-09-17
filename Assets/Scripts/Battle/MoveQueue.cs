using System.Collections.Generic;
using System.Linq;
using Move;
using UnityEngine;

namespace Battle
{
    public class MoveQueue
    {
        private readonly List<MoveNode> list = new();
        public int Count => list.Count;
        
        public void Enqueue(MoveBuilder move, Unit source, Unit target)
        {
            list.Add(new MoveNode(move, source, target));
        }

        public MoveNode Dequeue()
        {
            MoveNode result = null;
            if (list.Count > 0)
            {
                result = list.Aggregate(FindFaster);
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
            var deletedNode = list.FirstOrDefault(node => node.source.Equals(unit));
            list.Remove(deletedNode);
        }

        public void Prepare()
        {
            foreach (var node in list)
            {
                node.move.Prepare(node.source);
            }
        }
        
        private static MoveNode FindFaster(MoveNode node1, MoveNode node2)
        {
            MoveNode result;

            var m1 = node1.move;
            var s1 = node1.source;

            var m2 = node2.move;
            var s2 = node2.source;

            if (m1.priority == m2.priority)
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
                result = m1.priority > m2.priority ? node1 : node2;
            }

            return result;
        }
    }


    public class MoveNode
    {
        public readonly MoveBuilder move;
        public readonly Unit source;
        public readonly Unit target;

        public MoveNode(MoveBuilder move, Unit source, Unit target)
        {
            this.move = move;
            this.source = source;
            this.target = target;
        }
    }
}