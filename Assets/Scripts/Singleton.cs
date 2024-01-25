using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    static T s_Instance;
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<T>();

                if (s_Instance == null)
                {
                    var obj = new GameObject();
                    obj.name = typeof(T).Name;
                    obj.AddComponent<T>();
                    s_Instance = obj as T;
                }
            } 

            return s_Instance;
        }
        private set 
        { 
            s_Instance = value; 
        }
    }

    protected virtual void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }

        s_Instance = this as T;
    }
}