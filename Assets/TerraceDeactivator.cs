using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraceDeactivator : MonoBehaviour
{
    public GameObject terrace = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            terrace.SetActive(false);
    }
}