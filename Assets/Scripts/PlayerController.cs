using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float accX = 1f;
    public float gravity = -1f;
    public float jumpAcc = 10f;
    public float vMax = 2;
    public float drag = 0.1f;

    Vector2 velocity; 
    bool jumping;

    void Start () {
        jumping = true;
        velocity = new Vector2(0, 0); 
    }
    
    void Update () {
        Vector3 targetPos = rigidbody2D.position;

        Vector2 acceleration = new Vector2(0f, 0f);

        if (jumping) {
            acceleration.y = gravity; 
        }

        bool moved = false;

        if (Input.GetKey("a")) {
            if (velocity.x > 0)
                velocity.x = 0;
            moved = true; 
            acceleration.x -= accX;
        }

        if (Input.GetKey("d")) {
            if (velocity.x < 0) 
                velocity.x = 0;
            moved = true;
            acceleration.x += accX; 
        }

        if (!moved) {
            if (velocity.x > drag) {
                velocity.x -= drag;
            } else if (velocity.x < -drag) {
                velocity.x += drag;
            } else {
                velocity.x = 0;
            }
        }

        if (Input.GetKey("w")) {
            if (!jumping) {
                acceleration.y += jumpAcc; 
                jumping = true; 
            } 
        }

        float dt = Time.deltaTime;

        velocity += acceleration * dt;
        
        if (velocity.x > vMax) {
            velocity.x = vMax;
        }

        if (velocity.x < -vMax) {
            velocity.x = -vMax;
        }

        Vector2 distanceToMove = dt * velocity;

        targetPos += (Vector3) distanceToMove;

        rigidbody2D.MovePosition(targetPos);
    }

    void OnTriggerEnter2D(Collider2D enteringCollider) {
        Debug.Log("Entering Trigger!");
        jumping = false;
        velocity.y = 0f;
    }
}
