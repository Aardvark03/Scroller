using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float accX = 1f;
    public float gravity = -1f;
    public float jumpAcc = 10f;
    public float vMax = 2;
    public float drag = 0.1f;

    public bool onGround;

    Vector2 velocity;

    Bounds playerBounds;

    void Start () {
        velocity = new Vector2(0, 0);
        playerBounds = renderer.bounds;
        onGround = false;
    }

    bool isHit(RaycastHit2D hit) {
        return hit.collider != null;
    }

    void checkDown() {
            Vector2 leftSideVec = new Vector2(transform.position.x - playerBounds.extents.x, transform.position.y);
            Vector2 rightSideVec = new Vector2(transform.position.x + playerBounds.extents.x, transform.position.y);

            RaycastHit2D leftSide = Physics2D.Raycast(leftSideVec, -Vector2.up, playerBounds.extents.y, LayerMask.GetMask("Level"));
            RaycastHit2D rightSide = Physics2D.Raycast(rightSideVec, -Vector2.up, playerBounds.extents.y, LayerMask.GetMask("Level"));

            if (isHit(leftSide) || isHit(rightSide)) {
                    velocity.y = 0f;
                    onGround = true;
            } else {
                onGround = false;
            }
    }

    void checkLeft() {

    }

    void checkRight() {

    }

    void checkUp() {

    }
    
    void Update () {
        Vector3 targetPos = transform.position;

        Vector2 acceleration = new Vector2(0f, 0f);

        checkDown();

        if (!onGround) {
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
            if (onGround) {
                acceleration.y += jumpAcc; 
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

        transform.position = targetPos;
    }
}
