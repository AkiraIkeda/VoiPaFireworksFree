using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour {
    // Singleton Instance
    private static T _instance;
    // Accesessor
    public static T instance {
        get {
            if(_instance == null) {
                // Searh Object
                _instance = FindObjectOfType<T>();
                // Not Found
                if (_instance == null) {
                    Debug.LogError("Can't find " + typeof(T) + "!");
                }
            }
            return _instance;
        }
    }
}
