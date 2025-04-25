using UnityEngine;

public static class LevelManager
{
    private static bool[] levelCompleted = { false, false, false };


    public static void completeLevel(int level)
    {
        levelCompleted[level - 1] = true;
    }
}
