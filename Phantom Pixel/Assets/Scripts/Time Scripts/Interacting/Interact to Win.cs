using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConsole : ButtonConsole
{
    // interact with this to win the level

    public override void Interact()
    {
        // beat the level
        Debug.Log("You Win!");
        LevelManager.completeLevel(2);
        SceneManager.LoadScene(0);
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        base.ApplyRewindData(PIT);
    }

    public override PointInTime CreatePIT()
    {
        return base.CreatePIT();
    }
}
