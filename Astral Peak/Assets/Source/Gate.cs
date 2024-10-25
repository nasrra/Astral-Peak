using UnityEngine;

public class Gate : Door{
    public override void enter(){
        if(GateKeyManager.get_area_cleared() == true)
            base.enter();
        else
            Debug.Log("Required Keys:"+GateKeyManager.get_required_keys());
    }
}
