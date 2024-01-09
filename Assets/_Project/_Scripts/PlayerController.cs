using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager input;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private float movementSpeed = 10f;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        rbody.velocity = input.Move * movementSpeed;
    }
}
