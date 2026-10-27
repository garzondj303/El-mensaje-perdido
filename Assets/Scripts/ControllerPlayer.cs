using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class ControllerPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 moveInput;

    private Rigidbody2D rb;
    
    private Animator animator;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnMove (InputValue Value)
    {
        moveInput = Value.Get<Vector2>();
    }

    void Update()
    {
        if(moveInput != Vector2.zero)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY" , moveInput.y);
            animator.SetBool("IsMoving",true);
        }
        else
        { 
            animator.SetBool("IsMoving",false);
        }
    }



    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;   
        }
}
