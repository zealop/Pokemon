
using System.Collections.Generic;
using System.Linq;
using Game.Moves;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Battles
{
    public class MoveQueue
    {
        private readonly List<MoveNode> list = new();
        public int Count => list.Count;
        public bool IsEmpty => list.IsNullOrEmpty();
        
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
            var deletedNode = list.FirstOrDefault(node => node.Source.Equals(unit));
            list.Remove(deletedNode);
        }

        public void Prepare()
        {
            foreach (var _ in list)
            {
                // node.move.Prepare(node.source);
            }
        }
        
        private static MoveNode FindFaster(MoveNode node1, MoveNode node2)
        {
            MoveNode result;

            var m1 = node1.Move;
            var s1 = node1.Source;
            
            var m2 = node2.Move;
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
}