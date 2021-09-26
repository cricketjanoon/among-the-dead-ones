using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float amount;
    public float smoothAmount;
    public float maxAmount;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        float movementX = Mathf.Clamp(-Input.GetAxis("Mouse X") * amount, -maxAmount, maxAmount);
        float movementY = Mathf.Clamp(-Input.GetAxis("Mouse Y") * amount, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }
}