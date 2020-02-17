using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [HideInInspector]
    public Vector3 lastDirection = new Vector3(1, 0, 0);
    [HideInInspector]
    public bool isWalking;

    public InputAction moveAction;

    [Header("Walking")]
    [SerializeField]
    float speed = 2;

    [Header("Rolling")]
    [HideInInspector]
    public bool isRolling = false;
    [SerializeField]
    float rollDuration = 0.2f;
    [SerializeField]
    float rollSpeedBonus = 2f;

    bool canMove = true;

    void Start() {
        rb = this.GetComponentInChildren<Rigidbody2D>();
        animator = this.GetComponentInChildren<Animator>();

        // moveAction.performed += OnMove;
        // moveAction.Enable();
    }

    void Update() {
        if (!canMove) {
            this.rb.velocity = Vector2.zero;
            this.animator.SetInteger("horizontal", 0);
            this.animator.SetInteger("vertical", 0);
            return;
        }

        OnMove();
    }

    bool firstHor = false;
    bool firstVer = false;

    void OnMove(/*InputAction.CallbackContext ctx*/) {
        // Vector2 motion = ctx.ReadValue<Vector2>();
        // float hor = motion.x;
        // float ver = motion.y;

        float hor = Input.GetAxisRaw("Horizontal");
        hor = (int) hor > 0 ? 1 : (hor < 0 ? -1 : 0);
        float ver = Input.GetAxisRaw("Vertical");
        ver = (int) ver > 0 ? 1 : (ver < 0 ? -1 : 0);

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
            lastDirection = new Vector3(hor, ver);
        }

        this.isWalking = ((hor + ver) != 0) && !this.isRolling;

        this.animator.SetBool("walking", this.isWalking);
        this.animator.SetInteger("horizontal", (int) hor);
        this.animator.SetInteger("vertical", (int) ver);
    }

    public void Roll() {
        if (!this.canMove || this.isRolling) return;
        StartCoroutine(RollTimer());
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

    public void SetActive(bool value) {
        this.canMove = value;
    }
}
