using System;
using System.Collections;
using UnityEngine;

namespace Entropek{
public static class Util{
    public static IEnumerator timer(float time, System.Action start_action = null, System.Action time_out = null){
        start_action?.Invoke();
        yield return new WaitForSeconds(time);
        time_out?.Invoke();
    }
}
}
