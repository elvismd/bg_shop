using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingleInstance<PlayerController>
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerClothing clothing;

    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private float movementSpeed = 10f;

    public PlayerClothing Clothing => clothing;

    Vector2 movement;
    int direction;

    void Start()
    {
        GameManager.Instance.OnInteractionStart += () => enabled = false;
        GameManager.Instance.OnInteractionEnd += () => enabled = true;
    }

    private void OnEnable()
    {
        rbody.velocity = Vector2.zero;
    }

    private void OnDisable()
    {
        rbody.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        movement = InputManager.Instance.Move;
        rbody.velocity = movement * movementSpeed;
    }

    private void Update()
    {
        const float threshold = 0.01f;
        float speed = 0f;
        if (movement.magnitude > threshold)
        {
            speed = 1f;

            if (Mathf.Abs(movement.y) > threshold)
                direction = movement.y < 0 ? 1 : 0;
            if (Mathf.Abs(movement.x) > threshold)
                direction = movement.x < 0 ? 2 : 3;
        }

        animator.SetFloat("Speed", speed);
        animator.SetFloat("Direction", direction);

        if (clothing.ClothAnimator.runtimeAnimatorController != null)
        {
            clothing.ClothAnimator.SetFloat("Speed", speed);
            clothing.ClothAnimator.SetFloat("Direction", direction);
        }

        if (clothing.HatAnimator.runtimeAnimatorController != null)
        {
            clothing.HatAnimator.SetFloat("Speed", speed);
            clothing.HatAnimator.SetFloat("Direction", direction);
        }
    }
}
