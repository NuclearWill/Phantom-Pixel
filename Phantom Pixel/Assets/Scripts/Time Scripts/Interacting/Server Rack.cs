using UnityEditor;
using UnityEngine;

public class ServerRack : ButtonConsole
{
    [SerializeField]
    Color normalColor = Color.white, ruinedColor = Color.red;

    private Color thisColor => (!activated) ? normalColor : ruinedColor;
    new Renderer renderer;

    public override void Interact()
    {
        base.Interact();
        GetComponent<Renderer>().material.color = thisColor;
    }

    public override void ApplyRewindData(PointInTime PIT)
    {
        base.ApplyRewindData(PIT);
        GetComponent<Renderer>().material.color = thisColor;
    }

    public override PointInTime CreatePIT()
    {
        return base.CreatePIT();
    }
}
