using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjective : MonoBehaviour
{
    //TODO: Check the conditions that mission objectives are complete

    private void OnTriggerEnter(Collider other)
    {
        if (GameSceneManager.instance)
        {
            PlayerInfo playerInfo = GameSceneManager.instance.GetPlayerInfo(other.GetInstanceID());
            if (playerInfo != null)
                playerInfo.characterManager.DoLevelComplete();
        }
    }
}