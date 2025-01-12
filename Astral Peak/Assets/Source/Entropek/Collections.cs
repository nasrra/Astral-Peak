using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entropek.Collections{
[Serializable]
public class StateQueue{
    Coroutine state;
    [SerializeField] Queue<IEnumerator> states = new Queue<IEnumerator>();
    MonoBehaviour entity;
    Action default_state;

    public StateQueue(MonoBehaviour _entity, Action _default_state){
        default_state = _default_state;
        entity = _entity;
    }

    public void start() => state_switch();
    public void state_switch(){
        if (state != null)
            entity.StopCoroutine(state);
        if (states.Count <= 0)
            default_state();
        else
            state = entity.StartCoroutine(states.Dequeue()); // start and deque the state.
    }
    public void queue_and_start(Action start_action, Action time_out = null, float time = 0){
        queue(time: time, start_action: start_action, time_out: time_out);
        state_switch();
    }
    public void queue(Action start_action, Action time_out = null, float time = 0){
        if (time < 0)
            throw new Exception("time cannot be less than zero!");
        states.Enqueue(Util.timer(
            time: time,
            start_action: () => start_action?.Invoke(),
            time_out: () =>{
                time_out?.Invoke();
                state_switch();
            }
        ));
    }
    public void queue(List<StateQueueItem> items){
        foreach (StateQueueItem item in items)
            queue(time: item.time, start_action: item.start_action, time_out: item.time_out);
    }
    
    public void queue_and_start(List<StateQueueItem> items){
        queue(items);
        state_switch();
    }
    public void clear() => states.Clear();
    public void stop(){
        if(state!=null)
            entity.StopCoroutine(state);
    }
    public void clear_and_stop(){
        clear();
        stop();
    }
}

public struct StateQueueItem{
    public readonly float time;
    public readonly Action start_action;
    public readonly Action time_out;
    public StateQueueItem(Action _start_action, Action _time_out = null, float _time = 0){
        time=_time;
        start_action=_start_action;
        time_out=_time_out;
    }
}

[Serializable]
public class ActionQueue{
    [SerializeField] List<Action> actions = new List<Action>();
    MonoBehaviour entity;
    public ActionQueue(MonoBehaviour _entity, Action _default_action){
        actions.Add(_default_action);
        entity = _entity;
    }
    public void start() => next_action();
    public void next_action() => actions[actions.Count-1]();
    public void queue(Action action, float time = 0){
        if(time<=0)
            throw new Exception("time cannot be 0 or less!");
        actions.Add(()=>entity.StartCoroutine(Util.timer(
            time: time, 
            start_action: () =>{
                action();
                actions.RemoveAt(actions.Count-1);
            }, 
            time_out: ()=>{
                next_action();        
            }
        )));
    }
    public void clear() => actions.Clear();
}

[Serializable]
public sealed class SwapbackArray<T> : List<T>{
    /// <summary>
    /// Removes the element at the specified index after swapping it with the last element.
    /// </summary>
    /// <param name="index">The index of the element to remove.</param>
    public new void RemoveAt(int index){
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

        if (Count <= 1){
            // If the list has 0 or 1 element, simply remove the element.
            base.RemoveAt(index);
            return;
        }

        int lastIndex = Count - 1;

        // Swap the element at the specified index with the last element.
        T temp = this[index];
        this[index] = this[lastIndex];
        this[lastIndex] = temp;

        // Remove the last element (which is now the one originally at `index`).
        base.RemoveAt(lastIndex);
    }

    /// <summary>
    /// Removes the first occurrence of the specified element after swapping it with the last element.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>True if the item was successfully removed; otherwise, false.</returns>
    public new bool Remove(T item){
        int index = IndexOf(item);
        if (index == -1)
            return false;
        RemoveAt(index);
        return true;
    }
}
}
