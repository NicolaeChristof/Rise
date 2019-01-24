using System;

public static class Extensions
{

    public static T Next<T>(this T inputEnum) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] values = (T[])Enum.GetValues(inputEnum.GetType());
        int index = Array.IndexOf<T>(values, inputEnum) + 1;
        return (values.Length == index) ? values[0] : values[index];
    }

    public static T Previous<T>(this T inputEnum) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] values = (T[])Enum.GetValues(inputEnum.GetType());
        int index = Array.IndexOf<T>(values, inputEnum) - 1;
        return (index < 0) ? values[values.Length-1] : values[index];
    }
}