using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public bool dontDestroyOnLoad;

    private static volatile T _instance;
    private static readonly object SyncRoot = new();
    private static bool _applicationIsQuitting;
    public static T Instance
    {
        get
        {
            Initialize();
            return _instance;
        }
    }

    public static bool IsInitialized => _instance != null;

    public static void Initialize()
    {
        if (_instance != null)
        {
            return;
        }
        lock (SyncRoot)
        {
            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                var go = new GameObject(typeof(T).FullName);
                _instance = go.AddComponent<T>();
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"{GetType().Name} Singleton class is already created. Deleting this duplicate instance.");
            Destroy(this.gameObject);
            return;
        }

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        OnAwake();
    }

    private void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    private void OnDestroy()
    {
        if (_applicationIsQuitting)
        {
            return;
        }
    }
    // protected virtual void OnDestroy()
    // {
    //     if (_instance == this)
    //     {
    //         _instance = null;
    //     }
    // }

    protected virtual void OnAwake() { }
}
