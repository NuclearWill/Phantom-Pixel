using UnityEditor;
using UnityEngine;

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
