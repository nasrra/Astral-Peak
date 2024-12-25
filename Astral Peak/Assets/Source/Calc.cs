using System.Collections;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using UnityEngine;

namespace Calc{
public static class Coroutines{
    static public IEnumerator lerp_value(System.Action<float> _value, float _start, float _end, float _time, System.Action _onComplete = null){
        float elapsedTime = 0;
        float t = 0;
        while (t < _time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / _time;
            _value(Mathf.Lerp(_start,_end,1/_time * t));
            yield return null;
        }
        _value(_end);
        _onComplete?.Invoke();
        yield break;
    }
    static public IEnumerator lerp_color(System.Action<Color> _value, Color _start, Color _end, float _time, System.Action _onComplete = null) {
        float elapsedTime = 0;
        float t = 0;
        while (t < _time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / _time;
            _value(Color.Lerp(_start,_end,1/_time * t));
            yield return null;
        }
        _value(_end);
        _onComplete?.Invoke();
        yield break;
    }
}
}
