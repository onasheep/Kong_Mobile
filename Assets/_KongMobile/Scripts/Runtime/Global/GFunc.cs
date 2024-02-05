using System.Collections.Generic;
using UnityEngine;

public static partial class GFunc
{

    public static bool IsValid(this GameObject gameObject_)
    {
        bool isValid = gameObject_ == null || gameObject_ == default;
        return !isValid;
    }

    public static bool IsValid<T>(this T component_) where T : Component
    {
        Component convert = (Component)(component_ as Component);
        bool isValid = convert == null || convert == default;
        return !isValid;
    }

    public static bool IsValid<T>(this List<T> list_)
    {
        bool isValid = list_ == null || list_.Count < 1;
        return !isValid;
    }

    public static bool IsValid<T>(this T[] array_)
    {
        bool isValid = array_ == null || array_.Length < 1;
        return !isValid;
    }

    public static bool IsValid<T>(this Queue<T> queue_)
    {
        bool isValid = queue_ == null || queue_.Count < 1;
        return !isValid;
    }

}