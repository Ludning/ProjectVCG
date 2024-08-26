using System;
using System.Collections.Generic;
using Frameworks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

// A Manager Class for Addressable Assets
public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<Type, Dictionary<string, Object>> _resourceDictionary;
    
    private T LoadResource<T>(string resourceName) where T : Object
    {
        string resourcePath = DataManager.Instance.GetAssetAddress<T>(resourceName);
        
        T resource = Addressables.LoadAssetAsync<T>(resourcePath).WaitForCompletion();
        
        if (resource == null)
            throw new System.NotImplementedException();
        
        return resource;
    }
    
    public T LoadResourceWithCaching<T>(string resourceName) where T : Object
    {
        Debug.Log($"resourceName: {resourceName}");
        
        if (_resourceDictionary == null)
            _resourceDictionary = new Dictionary<Type, Dictionary<string, Object>>();
        
        if (!_resourceDictionary.ContainsKey(typeof(T)))
            _resourceDictionary.Add(typeof(T), new Dictionary<string, Object>());
        
        return LoadResourceWithCaching<T>(resourceName, _resourceDictionary[typeof(T)]);
    }
    
    private T LoadResourceWithCaching<T>(string resourceName, Dictionary<string, Object> resourceDict) where T : Object
    {
        if(!resourceDict.ContainsKey(resourceName))
            resourceDict.Add(resourceName, LoadResource<T>(resourceName));
        
        return (T)resourceDict[resourceName];
    }
}
