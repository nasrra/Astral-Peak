using UnityEngine;
using UnityEngine.Events;

public class GateKey : MonoBehaviour{
    public UnityEvent key_acquired;
    public void acquire_key(){
        GateKeyManager.acquire_key();
        key_acquired?.Invoke();
        Debug.Log(GateKeyManager.get_required_keys());
    }
}
