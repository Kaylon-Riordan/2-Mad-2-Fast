using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    // https://www.reddit.com/r/Unity3D/comments/v3j2hf/i_made_a_generic_monobehaviour_singleton_class_to/
    public static T get()
    {
        if (!instance)
        {
            // https://docs.unity3d.com/ScriptReference/Object.FindObjectsByType.html
            // Replaces the obsolete FindObjectOfType<T>()
            set(FindObjectsByType<T>(FindObjectsSortMode.None)[0]);
        }

        return instance;
    }

    // https://github.com/Ben-Keev/Tower/blob/main/Assets/Scripts/GameManager.cs.cs
    public static bool set(T instance)
    {
        if (Singleton<T>.instance) // Instance already exists
        {
            Destroy(instance.gameObject);
            return false;
        }
        else // Instance doesn't yet exist
        {
            Singleton<T>.instance = instance;
            DontDestroyOnLoad(instance.gameObject);

            return true;
        }
    }
}
