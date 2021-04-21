// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Analysis.Dijkstra

open System
open System.Collections.Generic
open Priority_Queue

type PriorityQueueTracker<'Key, 'Priority when 'Key : equality and 'Priority :> IComparable<'Priority>> =
    private
        {
            Queue : SimplePriorityQueue<'Key, 'Priority>
        }

module PriorityQueueTracker =

    let add q key priority =
        if q.Queue.Contains(key) then
            q.Queue.UpdatePriority(key, priority)
        else
            q.Queue.Enqueue(key, priority)

    let hasItems q () =
        q.Queue.Count > 0

    let pop q () =
        let key = q.Queue.First
        let priority = q.Queue.GetPriority(key)
        q.Queue.Dequeue() |> ignore
        (key, priority)

    let createEmpty<'Key, 'Priority when 'Key : equality and 'Priority :> IComparable<'Priority>> =
        let queue = { Queue = SimplePriorityQueue<'Key, 'Priority>() }
        (add queue, hasItems queue, pop queue)

type SimpleTracker<'Key, 'Priority when 'Key : equality and 'Priority :> IComparable<'Priority>> =
    private
        {
            Queue : Dictionary<'Key, 'Priority>
        }

module SimpleTracker =

    let add q key priority =
        if q.Queue.ContainsKey(key) then
            ()
        else
            q.Queue.Add(key, priority)

    let hasItems q () =
        q.Queue.Count > 0

    let pop q () =
        let key = q.Queue |> Seq.head
        q.Queue.Remove(key.Key) |> ignore
        (key.Key, key.Value)

    let createEmpty<'Key, 'Priority when 'Key : equality and 'Priority :> IComparable<'Priority>> =
        let queue = { Queue = Dictionary<'Key, 'Priority>() }
        
        (add queue, hasItems queue, pop queue)