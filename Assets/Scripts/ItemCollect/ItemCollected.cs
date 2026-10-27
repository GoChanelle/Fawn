using UnityEngine;
using System.Collections.Generic;

public static class CollectedPickups
{
    private static HashSet<string> collected = new HashSet<string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => collected.Clear();

    public static void Reset() => collected.Clear();

    public static void MarkCollected(string id) => collected.Add(id);
    public static bool IsCollected(string id) => collected.Contains(id);
}