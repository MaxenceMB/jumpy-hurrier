using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    public static Vector3 SPAWN_POSITION = new Vector3(2, 5, 0);

    [Header("Gravity Settings")]
    [SerializeField] private float terminalVelocity;
    [SerializeField] private float velocityGrowthRate;
    [SerializeField] private float velocityGrowthOffset;
    [Space(10)]
    [SerializeField] private LayerMask ground;
    [SerializeField] [Range(-1, 1)] private float groundCheckSidesOffset;
    [SerializeField] [Range(0, 1)]  private float groundCheckLength;

    [Header("Jump Settings")]
    [SerializeField] private float maximalJumpForce;
    [SerializeField] private float jumpChargeGrowth;

    [Header("UI Settings")]
    [SerializeField] private UIManagerScript UIManager;

    private Vector2 moveDirection;

    private float timeInAir;

    private float jumpCharge;
    private float jumpingForce;
    private bool jumping;

    private bool falling = true;

    private bool grounded;
    private Vector2 groundPosition;

    private BoxCollider2D coll;

    
    void Start() {
        this.moveDirection = Vector2.zero;
        this.timeInAir = 0;
        this.jumpCharge = 0;

        this.coll = GetComponent<BoxCollider2D>();
    }
    
    void Update() {
        this.moveDirection = Vector2.zero;

        // Jump
        JumpHandler();

        // Gravity
        this.groundPosition = IsGrounded();
        if(this.falling) {
            Fall();

            if(this.grounded && this.moveDirection.y < 0.0f) { 
                GroundPosition();
                this.falling = false;
                this.jumping = false;
            } 
        }        
    }

    void LateUpdate() {
        this.transform.Translate(this.moveDirection);
    }



    //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\ JUMP //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
    
    private void JumpHandler() {
        // If holding space key -> charges the jump power
        if(Input.GetKey(KeyCode.Space) && this.grounded) {
            this.jumpCharge += jumpChargeGrowth * Time.deltaTime;

            float chargeAmount = Math.Clamp(this.jumpCharge/maximalJumpForce, 0, 1);
            UIManager.ChangeChargeValue(chargeAmount);
        }

        // If letting go -> Jumps
        if(Input.GetKeyUp(KeyCode.Space)) {
            if(this.grounded) {
                this.jumpingForce = this.jumpCharge;
                this.jumping = true;
            }
            
            UIManager.ChangeChargeValue(0);
            this.jumpCharge = 0.0f;
        }

        // If player is jumping, move it
        if(this.jumping) {
            this.moveDirection.y += JumpForce() * Time.deltaTime;
        }
    }

    private float JumpForce() {
        return Math.Clamp(this.jumpingForce, 0, maximalJumpForce);
    }



    //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\ GRAVITY \\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\

    private Vector3 IsGrounded() {

        float xPos = (this.coll.size.x/2 + this.coll.offset.x) * groundCheckSidesOffset;
        float yPos = this.transform.position.y - this.coll.size.y/2 + this.coll.offset.y;

        Vector2 backRaycastStart  = new Vector2(this.transform.position.x - xPos, yPos);
        Vector2 frontRaycastStart = new Vector2(this.transform.position.x + xPos, yPos);

        Debug.DrawRay(backRaycastStart,  -Vector3.up*groundCheckLength, Color.green); 
        Debug.DrawRay(frontRaycastStart, -Vector3.up*groundCheckLength, Color.green); 

        Vector3 groundPos = Vector3.zero;

        RaycastHit2D backHit  = Physics2D.Raycast(backRaycastStart,  -Vector2.up, groundCheckLength, this.ground.value);
        RaycastHit2D frontHit = Physics2D.Raycast(frontRaycastStart, -Vector2.up, groundCheckLength, this.ground.value);
        if (backHit || frontHit) {
            Collider2D collider = (frontHit.collider == null) ? backHit.collider : frontHit.collider;
            groundPos = collider.GetComponent<GroundScript>().GetGroundPosition();
        }

        if(groundPos != Vector3.zero) {
            this.grounded = true;
        } else {
            this.grounded = false;
            this.falling = true;
        }
        
        return groundPos;
    }

    private void GroundPosition() {
        this.timeInAir = 0.0f;
        this.moveDirection = Vector2.zero;
        this.transform.position = new Vector3(this.transform.position.x, this.groundPosition.y + (this.coll.size.y/2 - this.coll.offset.y), 0);
    }
    
    private void Fall() {
        this.timeInAir += Time.deltaTime;
        this.moveDirection.y -= FallForce() * Time.deltaTime;
    }

    private float FallForce() {
        float force = velocityGrowthRate * this.timeInAir + velocityGrowthOffset;
        return Math.Clamp(force, 0, terminalVelocity);
    }
}
