using UnityEngine;

public static class GameProgress
{
    public static bool LockedRoomCutsceneSeen = false;
    public static bool BuckChasing = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        Reset();
    }

    public static void Reset()
    {
        LockedRoomCutsceneSeen = false;
        BuckChasing = false;
    }
}