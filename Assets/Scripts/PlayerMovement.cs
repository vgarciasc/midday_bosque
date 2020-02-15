using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2;

    Rigidbody2D rb;
    Animator animator;

    [HideInInspector]
    public Vector3 lastDirection = new Vector3(1, 0, 0);

    [Header("Rolling")]
    bool isRolling = false;
    [SerializeField]
    float rollDuration = 0.2f;
    [SerializeField]
    float rollSpeedBonus = 2f;

    PlayerActions playerActions;

    void Start() {
        rb = this.GetComponentInChildren<Rigidbody2D>();
        animator = this.GetComponentInChildren<Animator>();
        playerActions = this.GetComponent<PlayerActions>();
    }

    void Update() {
        if (playerActions.frozen) {
            this.rb.velocity = Vector2.zero;
            return;
        }

        HandleRoll();
        HandleMove();
    }

    bool firstHor = false;
    bool firstVer = false;

    void HandleMove() {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        if (isRolling) {
            hor = lastDirection.x;
            ver = lastDirection.y;
        }

        if (ver == 0) {
            firstVer = false;
        } else if (hor == 0) {
            firstHor = false;
        }

        if (hor == 0 && ver != 0) {
            firstVer = true;
        } else if (ver == 0 && hor != 0) {
            firstHor = true;
        }

        if (firstVer) {
            hor = 0;
        } else if (firstHor) {
            ver = 0;
        }

        float speed = this.speed * (this.isRolling ? this.rollSpeedBonus : 1f);
        this.rb.velocity = (new Vector2(hor, ver)).normalized * speed;

        if (hor + ver != 0) {
            lastDirection = new Vector3((int) hor, (int) ver);
        }

        bool isWalking = ((hor + ver) != 0) && !isRolling;

        this.animator.SetBool("walking", isWalking);
        this.animator.SetInteger("horizontal", (int) hor);
        this.animator.SetInteger("vertical", (int) ver);
    }

    void HandleRoll() {
        if (Input.GetKeyDown(KeyCode.E) && !isRolling) {
            StartCoroutine(RollTimer());
        }
    }

    IEnumerator RollTimer() {
        isRolling = true;
        this.animator.SetBool("rolling", isRolling);
        yield return new WaitForSeconds(this.rollDuration);
        isRolling = false;
        this.animator.SetBool("rolling", isRolling);
    }

    public Vector3 GetDirection() {
        return lastDirection;
    }
}
