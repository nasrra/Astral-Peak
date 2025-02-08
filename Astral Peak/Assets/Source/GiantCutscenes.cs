using System.Collections;
using Entropek;
using UnityEngine;

namespace Cutscenes{
public class GiantOpening : Cutscene{
    public override IEnumerator get_coroutine(){
        Debug.Log("giant opening cutscene not implemented!");
        end();
        yield break;
    }
}

public class GiantPhaseTransition : Cutscene{
    public override IEnumerator get_coroutine(){
        Debug.Log("giant phase transition cutscene not implemented!");
        end();
        yield break;
    }
}

}
