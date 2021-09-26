using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayActive : MonoBehaviour
{
    [SerializeField]
    private GameObject firstFloor;

    [SerializeField]
    private GameObject FloorMarque;

    // Use this for initialization
    private void Start()
    {
        firstFloor.SetActive(true);
        FloorMarque.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        FloorMarque.SetActive(true);
        firstFloor.SetActive(true);
     //   Debug.Log("Active");
    }
}