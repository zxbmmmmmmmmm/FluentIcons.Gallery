using System;

namespace FluentIcons.Gallery;

public static class Utilities
{
    public static T Let<T>(this T value, Action<T> action)
    {
        action(value);
        return value;
    }
}
