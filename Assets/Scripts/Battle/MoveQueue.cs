using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveQueue
{
    private readonly List<MoveNode> list = new List<MoveNode>();
    public int Count => list.Count;

    public void Enqueue(Move move, BattleUnit source, BattleUnit target)
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

    private static MoveNode FindFaster(MoveNode node1, MoveNode node2)
    {
        var m1 = node1.Move.Base;
        var s1 = node1.Source;
        var t1 = node1.Target;

        var m2 = node2.Move.Base;
        var s2 = node2.Source;
        var t2 = node2.Target;

        //if (m1.Priority(s1, t1) > m2.Priority(s1, t1))
        //{
        //    return node1;
        //}
        //if (m1.Priority(s1, t1) == m2.Priority(s1, t1))
        //{
        //    if (s1.Speed > s2.Speed)
        //        return node1;
        //    if (s1.Speed == s2.Speed)
        //        return Random.value < 0.5f ? node1 : node2;
        //    else
        //        return node2;
        //}
        //else
        //{
        //    return node2;
        //}
        return node1;
    }
}


public class MoveNode
{
    public Move Move { get; }
    public BattleUnit Source { get; }
    public BattleUnit Target { get; }

    public MoveNode(Move move, BattleUnit source, BattleUnit target)
    {
        Move = move;
        Source = source;
        Target = target;
    }
}