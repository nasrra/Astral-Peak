using System;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Deluz{
public static class Log{

    public static void MethodNotImplemented(object instance, [CallerMemberName] string methodName = "") 
        => throw new NotImplementedException($"'{instance.GetType().Name}' class has not implemented: '{methodName}'.");

    // used when outside of Unity Engine.
    //public static void MethodCall(object instance, [CallerMemberName] string methodName = "")
    //    => Console.WriteLine($"[Class]: '{instance.GetType().Name}' [Method]: '{methodName}'.");


    public static void MethodCall(object instance, [CallerMemberName] string methodName = "")
        => Debug.Log($"[Class]: '{instance.GetType().Name}' [Method]: '{methodName}'.");

}
}

