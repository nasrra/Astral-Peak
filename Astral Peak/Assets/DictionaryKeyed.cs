using System;
using System.Collections.Generic;

public class KeyedDictionary<TKey, TValue> : Dictionary<TKey, Func<TValue>>{
    public void Add(TKey key, Func<TKey, TValue> factory)
    {
        // Store a lambda that calls the factory with the key.
        base[key] = () => factory(key);
    }
}
