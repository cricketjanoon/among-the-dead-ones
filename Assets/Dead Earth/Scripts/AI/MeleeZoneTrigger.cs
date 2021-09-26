using UnityEngine;
using System.Collections;

public class MeleeZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        AIStateMachine machine = GameSceneManager.instance.GetAIStateMachine(col.GetInstanceID());
        if (machine)
        {
            machine.inMeleeRange = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        AIStateMachine machine = GameSceneManager.instance.GetAIStateMachine(col.GetInstanceID());
        if (machine)
        {
            machine.inMeleeRange = false;
        }
    }
}