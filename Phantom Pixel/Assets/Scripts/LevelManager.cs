using UnityEngine;

public static class LevelManager
{
    private static bool[] levelCompleted = { false, false, false };


    public static void completeLevel(int level)
    {
        levelCompleted[level - 1] = true;
    }

    public static void LoseLevel()
    {
        Debug.Log("You lost the level!");
        TimeManager.RestartTime();
    }
}
