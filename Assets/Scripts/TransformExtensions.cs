using UnityEngine;

public static class TransformExtensions
{
    public static T GetComponentInChildrenOnly<T>(this Transform parent) where T : Component
    {
        foreach (Transform child in parent)
        {
            T comp = child.GetComponentInChildren<T>();
            if (comp != null)
                return comp;
        }
        return null;
    }
}