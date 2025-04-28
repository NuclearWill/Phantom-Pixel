using UnityEngine;

public class MarchOfTime : MonoBehaviour
{
    // script to trigger TimeManager to keep time updated each frame
    void Update()
    {
        // doesn't advance time during dialogue

        if(!DialogueManager.isDialogueActive)
        {
            TimeManager.UpdateTime();
        }
    }
}
