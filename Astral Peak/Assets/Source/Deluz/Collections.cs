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
    Action default_state_call;

    public StateQueue(MonoBehaviour _entity, Action _default_state){
        default_state_call = _default_state;
        states.Enqueue(state_call(default_state_call));
        entity = _entity;
    }

    public void start() => state_switch();
    public void state_switch(){
        if (state != null)
            entity.StopCoroutine(state);
        if (states.Count <= 0)
            states.Enqueue(state_call(default_state_call));
        state = entity.StartCoroutine(states.Dequeue()); // start and deque the state.
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
    public void clear() => states.Clear();
    public void stop(){
        if(state!=null)
            entity.StopCoroutine(state);
    }
    public void clear_and_stop(){
        clear();
        stop();
    }
    IEnumerator state_call(Action action){action(); yield break;}
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
