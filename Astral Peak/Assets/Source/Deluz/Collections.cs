using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deluz.Collections{
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
    public void queue_and_start(float time, Action start_action, Action time_out = null){
        queue(time, start_action, time_out);
        state_switch();
    }
    public void queue(float time, Action start_action, Action time_out = null){
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
            queue(item.time, item.start_action, item.time_out);
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
    public StateQueueItem(float _time, Action _start_action, Action _time_out = null){
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

}
