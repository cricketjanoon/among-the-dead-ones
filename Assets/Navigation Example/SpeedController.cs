using UnityEngine;
using System.Collections;

public class SpeedController : MonoBehaviour
{
    // Public Variables
    public float Speed = 0.0f;

    // Private
    private Animator _controller = null;

    // Use this for initialization
    private void Start()
    {
        _controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        _controller.SetFloat("Speed", Speed);
    }
}