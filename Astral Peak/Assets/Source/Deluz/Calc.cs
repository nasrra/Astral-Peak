using System;
using System.Collections;
using UnityEngine;

namespace Deluz{
public static class Calc{
    static public IEnumerator lerp_value(System.Action<float> _value, float _start, float _end, float _time, System.Action _on_complete = null){
        float elapsedTime = 0;
        float t = 0;
        while (elapsedTime < _time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / _time;
            _value(Mathf.Lerp(_start,_end,t));
            yield return null;
        }
        _value(_end);
        _on_complete?.Invoke();
        yield break;
    }
    static public IEnumerator lerp_vector2(System.Action<Vector2> _value, Vector2 _start, Vector2 _end, float _time, System.Action _on_complete = null){
        float elapsedTime = 0;
        float t = 0;
        while (elapsedTime < _time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / _time;
            _value(Vector2.Lerp(_start,_end,t));
            yield return null;
        }
        _value(_end);
        _on_complete?.Invoke();
        yield break;
    }
    static public IEnumerator lerp_color(System.Action<Color> _value, Color _start, Color _end, float _time, System.Action _on_complete = null) {
        float elapsedTime = 0;
        float t = 0;
        while (elapsedTime < _time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / _time;
            _value(Color.Lerp(_start,_end,t));
            yield return null;
        }
        _value(_end);
        _on_complete?.Invoke();
        yield break;
    }//
    }
}
