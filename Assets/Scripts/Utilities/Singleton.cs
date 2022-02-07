using UnityEngine;


// https://www.youtube.com/watch?v=tE1qH8OxO2Y&t=284s

/// <Summary>
/// A static instance is similar to a singleton, but instead of destroying any new
/// instance, it overrides the current instance. This is handy for resetting the state
// and saves you doing it manually
/// <Summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T instance {get; private set;}
    protected virtual void Awake() {
        instance = this as T;
    }
    protected virtual void OnApplicationQuit() {
        instance = null;
        Destroy(gameObject);
    }
}

/// <Summary>
/// This transforms the static instance into a basic singleton. This will destory any new
/// versions created, leaving the original instance intact
/// <Summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
        }
        base.Awake();
    }
}

/// <Summary>
/// Finnally we have a persistent version of the singleton. This will survive through scene
/// loads. Perfect for system classes which require stateful, persistent data. Or audio sources
/// where music plays through loading screen, etc
/// <Summary>
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour {
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
