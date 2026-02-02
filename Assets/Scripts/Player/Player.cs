using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private float inputX;
    private float inputY;
    private Vector2 movementInput;

    private Animator[] animators;

    private bool isMoving;
    // Start is called before the first frame update
    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        PlayerInput();
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        MoveMent();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.4f;
            inputY *= 0.4f;
        }

        movementInput=new Vector2(inputX, inputY);

        isMoving = movementInput != Vector2.zero;
    }
    private void MoveMent()
    {
        rb.MovePosition(rb.position + movementInput * speed* Time.deltaTime);
    }

    private void SwitchAnimation()
    {
        foreach (var animator in animators)
        {
            animator.SetBool("IsMoving", isMoving);
            if (!isMoving) continue; 
            animator.SetFloat("InputX", inputX);
            animator.SetFloat("InputY", inputY);
            
        }
    }
}
