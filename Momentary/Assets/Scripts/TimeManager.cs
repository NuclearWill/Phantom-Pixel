using UnityEngine;
using UnityEngine.SceneManagement;

public static class TimeManager
{
    private static float gameTime = 0f;

    private static bool reversingTime = false;

    private static bool timePaused = false;

    public static void UpdateTime()
    {
        if (!reversingTime && !timePaused)
        {
            gameTime += Time.deltaTime;
        }
        else if (reversingTime)
        {
            gameTime -= Time.deltaTime;
            if(gameTime < 0f)
            {
                reversingTime = false;
                gameTime = 0f;
            }
        }
    }

    public static void BeginReverseTime()
    {
        reversingTime = true;
    }

    public static void StopReversingTime()
    {
        reversingTime = false;
    }

    public static bool isRewinding()
    {
        return reversingTime;
    }

    public static void PauseTime()
    {
        timePaused = true;
    }

    public static void ResumeTime()
    {
        timePaused = false;
    }

    public static void TogglePause()
    {
        timePaused = !timePaused;
    }

    public static bool isPaused()
    {
        return timePaused;
    }

    public static void RestartTime()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameTime = 0f;
    }

    public static float GetGameTime()
    {
        return gameTime;
    }

    public static void resetGameTime()
    {
        gameTime = 0f;
    }
}
