using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates and returns a singleton
/// </summary>
/// <typeparam name="T">The class the singleton manifests</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    // https://www.reddit.com/r/Unity3D/comments/v3j2hf/i_made_a_generic_monobehaviour_singleton_class_to/
    /// <summary>
    /// Gets a singleton of the given type. Creates one if it doesn't exist.
    /// </summary>
    /// <returns>An instance of a singleton.</returns>
    public static T get()
    {
        if (!instance)
        {
            // https://docs.unity3d.com/ScriptReference/Object.FindObjectsByType.html
            // Replaces the obsolete FindObjectOfType<T>()
            // Find object of the type which the singleton was initialised as and set it to not be destroyed.
            set(FindObjectsByType<T>(FindObjectsSortMode.None)[0]);
        }

        return instance;
    }

    // https://github.com/Ben-Keev/Tower/blob/main/Assets/Scripts/GameManager.cs.cs
    /// <summary>
    /// Sets up a singleton which can't be destroyed.
    /// </summary>
    /// <param name="instance">Static instance of a class</param>
    /// <returns>Whether a singleton was created</returns>
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
