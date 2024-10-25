using System;
using UnityEngine;

public static class GateKeyManager{
    [SerializeField] static int required_keys = 3;
    public static void acquire_key() => required_keys -= 1;
    public static void refresh(int key_amt) => required_keys = key_amt;
    public static bool get_area_cleared() => required_keys <= 0;
    public static int get_required_keys() => required_keys;
}
