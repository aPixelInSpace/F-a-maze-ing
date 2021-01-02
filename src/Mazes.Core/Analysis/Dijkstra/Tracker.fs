// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Analysis.Dijkstra.Tracker

open System
open System.Collections.Generic
open Priority_Queue

type Tracker<'Key, 'Priority when 'Key : equality and 'Priority :> IComparable<'Priority>> =
    abstract member Add : 'Key -> 'Priority -> unit
    abstract member HasItems : bool
    abstract member Pop : 'Key * 'Priority
    

type PriorityQueueTracker<'Key, 'Priority when 'Key : equality and 'Priority :> IComparable<'Priority>> =
    private
        {
            Queue : SimplePriorityQueue<'Key, 'Priority>
        }

    interface Tracker<'Key, 'Priority> with
        member this.Add key priority =
            if this.Queue.Contains(key) then
                this.Queue.UpdatePriority(key, priority)
            else
                this.Queue.Enqueue(key, priority)

        member this.HasItems =
            this.Queue.Count > 0

        member this.Pop =
            let key = this.Queue.First
            let priority = this.Queue.GetPriority(key)
            this.Queue.Dequeue() |> ignore
            (key, priority)

    static member createEmpty =
        { Queue = SimplePriorityQueue<'Key, 'Priority>() }

type SimpleTracker<'Key, 'Priority when 'Key : equality and 'Priority :> IComparable<'Priority>> =
    private
        {
            Queue : Dictionary<'Key, 'Priority>
        }

    interface Tracker<'Key, 'Priority> with
        member this.Add key priority =
            if this.Queue.ContainsKey(key) then
                ()
            else
                this.Queue.Add(key, priority)

        member this.HasItems =
            this.Queue.Count > 0

        member this.Pop =
            let key = this.Queue |> Seq.head
            this.Queue.Remove(key.Key) |> ignore
            (key.Key, key.Value)

    static member createEmpty =
        { Queue = Dictionary<'Key, 'Priority>() }