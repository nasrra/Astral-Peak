using System;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Entropek{
public static class Log{

    public static NotImplementedException MethodNotImplemented(object instance, [CallerMemberName] string methodName = "") 
        => new NotImplementedException($"'{instance.GetType().Name}' class has not implemented: '{methodName}'.");

    // used when outside of Unity Engine.
    //public static void MethodCall(object instance, [CallerMemberName] string methodName = "")
    //    => Console.WriteLine($"[Class]: '{instance.GetType().Name}' [Method]: '{methodName}'.");


    public static void MethodCall([CallerFilePath] string filePath = "", [CallerMemberName] string methodName = ""){
        string class_name = System.IO.Path.GetFileNameWithoutExtension(filePath);
        Debug.Log($"[MethodCall]: {class_name} : {methodName}");
    }

    public static void MethodCall(Action methodToCall, [CallerFilePath] string filePath = ""){
        string method_name = methodToCall.Method.Name;
        string class_name = System.IO.Path.GetFileNameWithoutExtension(filePath);
        Debug.Log($"[MethodCall]: {class_name} : {method_name}");
        // Call the passed method
        methodToCall?.Invoke();
    }

}
}

