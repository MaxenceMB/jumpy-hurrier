using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    [Header("Jump Settings")]
    [SerializeField] private float gravityForce;
    [SerializeField] private LayerMask ground;

    private Vector2 moveDirection;

    private BoxCollider2D coll;

    void Start () {
        this.moveDirection = new Vector2(0, 0);
        this.coll = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        bool grounded = IsTouchingGround();

        if (!grounded) {
            moveDirection.y -= 0.1f*gravityForce * Time.deltaTime;
            transform.Translate(moveDirection);
        } else {
            this.moveDirection = Vector2.zero;
        }
    }

    private bool IsTouchingGround() {
        Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y - coll.size.y/2 - coll.offset.y);

        if (Physics2D.Raycast(raycastStart, -Vector2.up, 0.01f, ground.value)) {
            return true;
        }
        return false;
    }
}
