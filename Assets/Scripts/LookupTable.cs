using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LookupTable <TKey, TValue>
{
    private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    private Func<TKey, TValue> _process;
    
    public LookupTable(Func<TKey, TValue> process) 
    {
        _process = process;
    }

    public TValue GetValue(TKey key) 
    {
        if (!_dictionary.ContainsKey(key)) 
            _dictionary[key] = _process(key);

        return _dictionary[key];
    }

    public TValue this[TKey key] 
    {
        get {
            return GetValue(key);
        }
    }
}
