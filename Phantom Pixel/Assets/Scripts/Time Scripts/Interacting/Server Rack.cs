using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerRack : ButtonConsole
{
    public override void Interact()
    {
        base.Interact();
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
