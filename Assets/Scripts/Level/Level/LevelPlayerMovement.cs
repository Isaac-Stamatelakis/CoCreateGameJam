using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool onGround = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        move();
        //detectGround();
        

    }

    private void move() {
        Vector2 velocity = Vector2.zero;
        velocity.y = rb.velocity.y;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            velocity.x = -8f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            velocity.x = +8f;
        }
        
        if (rb.velocity.y == 0 && Input.GetKey(KeyCode.Space)) {
            velocity.y += 6;
        }
        rb.velocity = velocity;
    }

    private void detectGround() {
        if (onGround) {
            return;
        }
        Vector2 origin = new Vector2(transform.position.x,transform.position.y - spriteRenderer.bounds.extents.y);
        Debug.Log(origin);
        RaycastHit2D hit2D = Physics2D.Raycast(origin,Vector2.down,0.2f,1 << LayerMask.NameToLayer("Ground"));
        if (hit2D.collider != null) {
            onGround = true;
        }
    }
}
