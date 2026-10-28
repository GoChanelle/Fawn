using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    static PersistentObject instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}