using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveQueue
{
    List<MoveNode> list;

    public MoveQueue()
    {
        list = new List<MoveNode>();
    }

    public int Count {get => list.Count;}

    public void Enqueue(Move move, BattleUnit source, BattleUnit target)
    {
        list.Add(new MoveNode(move, source, target));

    }

    public MoveNode Dequeue()
    {
        MoveNode result = null;
        
        if(list.Count > 0)
        {
            result = list.First();

            foreach(var node in list)
            {
                result = FindFaster(result, node);
            }
        }

        list.Remove(result);

        return result;
    }

    static MoveNode FindFaster(MoveNode node1, MoveNode node2)
    {
        var m1 = node1.Move.Base;
        var s1 = node1.Source;
        var t1 = node1.Target;

        var m2 = node2.Move.Base;
        var s2= node2.Source;
        var t2 = node2.Target;

        if (m1.Priority(s1, t1) > m2.Priority(s1, t1))
        {
            return node1;
        }
        if (m1.Priority(s1, t1) == m2.Priority(s1, t1))
        {
            if (s1.Speed > s2.Speed)
                return node1;
            if (s1.Speed == s2.Speed)
                return Random.value < 0.5f ? node1 : node2;
            else
                return node2;
        }
        else
        {
            return node2;
        }
    }


    public IEnumerator Prepare()
    {
        foreach(var node in list)
        {
            var move = node.Move.Base;
            var source = node.Source;
            var target = node.Target;
            yield return move.Prepare?.Invoke(source, target);
        }
    }
}


public class MoveNode
{
    public Move Move { get; private set; }
    public BattleUnit Source { get; private set; }
    public BattleUnit Target { get; private set; }

    public MoveNode(Move move, BattleUnit source, BattleUnit target)
    {
        Move = move;
        Source = source;
        Target = target;
    }
}
