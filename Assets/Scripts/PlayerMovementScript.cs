using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    [Header("Gravity Settings")]
    [SerializeField] private float terminalVelocity;
    [SerializeField] private float velocityGrowthRate;
    [SerializeField] private float velocityGrowthOffset;
    [Space(10)]
    [SerializeField] private LayerMask ground;

    private Vector2 moveDirection;

    private float timeSinceFall;
    private float timeSinceJump;
    private bool falling;
    private bool grounded;

    private BoxCollider2D coll;

    
    void Start() {
        this.moveDirection = new Vector2(0, 0);

        this.coll = GetComponent<BoxCollider2D>();
    }
    
    void Update() {
        if(IsGrounded() != Vector3.zero) {
            this.grounded = true;
        } else {
            this.falling = true;
            this.grounded = false;
        }
        
        if (!this.grounded && this.falling) {
            this.timeSinceFall += Time.deltaTime;
            moveDirection.y -= FallForce(timeSinceFall) * Time.deltaTime;
            transform.Translate(moveDirection);
        } else {
            GroundPosition();
            this.falling = false;
        }
    }

    private Vector3 IsGrounded() {
        Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y - coll.size.y/2 + coll.offset.y);
        Debug.DrawRay(raycastStart, -Vector3.up*0.1f, Color.green); 
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, -Vector2.up, 0.1f, ground.value);
        if (hit) {
            return hit.collider.GetComponent<GroundScript>().GetGroundPosition();
        }
        return Vector3.zero;
    }

    private void GroundPosition() {
        this.timeSinceFall = 0.0f;
        this.moveDirection = Vector2.zero;

        this.transform.position = new Vector3(this.transform.position.x, IsGrounded().y + (coll.size.y/2 - coll.offset.y), 0);
    }

    private float FallForce(float fallTime) {
        return (0.1f * terminalVelocity) / (float)(1 + Math.Pow(Math.E, -(velocityGrowthRate*fallTime - velocityGrowthOffset)));
    }
}
