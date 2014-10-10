using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float accX = 1f;
    public float gravity = -1f;
    public float jumpAcc = 10f;
    public float vMax = 2;
    public float drag = 0.1f;

    public bool onGround;
    public bool facingRight;
    public bool walking;
    public bool inputMovedPlayer;

    public Vector2 acceleration;
    public Vector2 velocity;
    public Vector2 position;

    Animator playerAnimator;

    Bounds playerBounds;

    void Start () {
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        playerBounds = GetComponent<BoxCollider2D>().bounds;
        onGround = false;
        inputMovedPlayer = false;
        facingRight = true;
        walking = false;
        playerAnimator = GetComponent<Animator>();
    }

    void updateAnimator() {
        walking = velocity.x != 0f; 
        
        float rotation = 0f;

        if (!facingRight) {
            rotation = 180f;
        }
        
        transform.eulerAngles = new Vector3(transform.rotation.x, rotation, transform.rotation.z);

        playerAnimator.SetBool("onGround", onGround);
        playerAnimator.SetBool("walking", walking);
        playerAnimator.SetBool("facingRight", facingRight);
    }

    bool isHit(RaycastHit2D hit) {
        return hit.collider != null;
    }

    void moveOutOfCollider(Collider2D collider, Vector2 dir) {
        if (dir.x != 0) {
            float distanceX = collider.bounds.extents.x + playerBounds.extents.x;
            position.x = collider.transform.position.x + distanceX * dir.x;
        }

        if (dir.y != 0) {
            float distanceY = collider.bounds.extents.y + playerBounds.extents.y;
            position.y = collider.transform.position.y + distanceY * dir.y;
        }
        
    }

    void checkDown() {
            Vector2 leftSideVec = new Vector2(position.x - playerBounds.extents.x * 0.8f, position.y);
            Vector2 rightSideVec = new Vector2(position.x + playerBounds.extents.x * 0.8f, position.y);

            RaycastHit2D leftSide = Physics2D.Raycast(leftSideVec, -Vector2.up, playerBounds.extents.y, LayerMask.GetMask("Level"));
            RaycastHit2D rightSide = Physics2D.Raycast(rightSideVec, -Vector2.up, playerBounds.extents.y, LayerMask.GetMask("Level"));

            if (isHit(leftSide)) {
                onGround = true;
                velocity.y = 0f;
                moveOutOfCollider(leftSide.collider, Vector2.up);
            }
            else if (isHit(rightSide)) {
                onGround = true;
                velocity.y = 0f;
                moveOutOfCollider(rightSide.collider, Vector2.up);
            } else {
                onGround = false;
            }
    }

    Collider2D checkSide(Vector2 dir) {
        Vector2 headVec = new Vector2(position.x, position.y + playerBounds.extents.y);
        Vector2 bellyVec = new Vector2(position.x, position.y);

        RaycastHit2D headHit = Physics2D.Raycast(headVec, dir, playerBounds.extents.x, LayerMask.GetMask("Level"));
        RaycastHit2D bellyHit = Physics2D.Raycast(bellyVec, dir, playerBounds.extents.x, LayerMask.GetMask("Level"));

        if (isHit(headHit)) {
            return headHit.collider;
        }

        if (isHit(bellyHit)) {
            return bellyHit.collider;
        }

        return null;
    }

    void checkLeft() {
        Collider2D collider = checkSide(-Vector2.right);
        
        if (collider != null) {
            velocity.x = 0f;
            moveOutOfCollider(collider, Vector2.right); 
        }
    }

    void checkRight() {
        Collider2D collider = checkSide(Vector2.right);
        
        if (collider != null) {
            velocity.x = 0f;
            moveOutOfCollider(collider, -Vector2.right);
        }
    }

    void checkUp() {

    }

    void applyGravity() {
        if (!onGround)
            acceleration.y = gravity;
    }

    void processInput() {
        inputMovedPlayer = false;

        if (Input.GetKey("a")) {
            if (velocity.x > 0)
                velocity.x = 0;
            inputMovedPlayer = true;
            facingRight = false;
            acceleration.x -= accX;
        }

        if (Input.GetKey("d")) {
            if (velocity.x < 0) 
                velocity.x = 0;
            facingRight = true;
            inputMovedPlayer = true;
            acceleration.x += accX; 
        }

        if (Input.GetKey("w")) {
            if (onGround) {
                acceleration.y += jumpAcc; 
            } 
        }
    }

    void applyDrag() {
        if (!inputMovedPlayer) {
            if (velocity.x > drag) {
                velocity.x -= drag;
            } else if (velocity.x < -drag) {
                velocity.x += drag;
            } else {
                velocity.x = 0;
            }
        }
    }

    void calculateMovement() {
        float dt = Time.deltaTime;

        velocity += acceleration * dt;
        
        if (velocity.x > vMax) {
            velocity.x = vMax;
        }

        if (velocity.x < -vMax) {
            velocity.x = -vMax;
        }

        Vector2 distanceToMove = dt * velocity;

        position += distanceToMove;

    }

    void checkCollisions() {
        checkDown();
        checkLeft();
        checkRight();
    }
    
    void Update () {
        acceleration = Vector2.zero;

        processInput();
        
        applyGravity();
        applyDrag();

        calculateMovement();

        checkCollisions();

        updateAnimator();     
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
