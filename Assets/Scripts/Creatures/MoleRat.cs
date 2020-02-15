﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleRat : MonoBehaviour
{
    public enum State {
        IDLE, MOVING, DIGGING
    };

    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    float distance = 2f;

    int currFloorLevel = -1;

    State state = State.IDLE;
    Vector3 target = Vector2.zero;
    GameObject targetObj = null;

    Sensors sensors;
    Rigidbody2D rb;
    SpriteRenderer sr;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.sr = this.GetComponent<SpriteRenderer>();
        this.sensors = this.GetComponentInChildren<Sensors>();

        UpdateFloor();
        StartCoroutine(Loop());
    }

    void Update() {
        // print(Vector3.Distance(target, this.transform.position));
        this.sr.flipX = (target.x - this.transform.position.x > 0);
    }

    void UpdateFloor() {
        this.currFloorLevel = TilemapHelper.GetFloor(this.transform.position);
    }

    IEnumerator Loop() {
        while (true) {
            var food = sensors.observations.Find((f) => f.CompareTag("MoleRatFood"));
            if (state != State.DIGGING && food != null) {
                targetObj = food;
                target = food.transform.position;
                state = State.MOVING;
            }

            switch (state) {
                case State.IDLE:
                    yield return Idle();
                    state = State.MOVING;
                    break;
                case State.MOVING:
                    yield return Move();
                    if (food == null) {
                        state = State.IDLE;
                    } else {
                        state = State.DIGGING;
                    }
                    break;
                case State.DIGGING:
                    yield return Dig();
                    state = State.IDLE;
                    break;
            }
        }
    }

    IEnumerator Idle() {
        var duration = Random.Range(0.5f, 0.7f);
        yield return new WaitForSeconds(duration);
        
        var dir = HushPuppy.GenerateValidPosition(
            () => new Vector3(
                Random.Range(-distance, distance),
                Random.Range(-distance, distance)),
            (vec) => {
                var hit = Physics2D.BoxCast(
                    this.transform.position,
                    Vector2.one,
                    0f,
                    vec.normalized,
                    vec.magnitude,
                    1 << LayerMask.NameToLayer("Walls")
                        | (1 << LayerMask.NameToLayer("Stairs"))
                        | TilemapHelper.GetLayerMaskCreatureCollision(this.currFloorLevel)
                );
                var outOfBounds = !TilemapHelper.InsideSameRoom(
                    this.transform.position,
                    this.transform.position + vec,
                    RoomManager.Get().dimensions
                );
                return !hit && !outOfBounds;
            });
        
        this.target = this.transform.position + dir;
    }

    IEnumerator Move() {
        this.GetComponent<EasyAnim>().duration /= 4f;
        this.rb.velocity = (this.target - this.transform.position) * this.speed;

        yield return new WaitUntil(
            () => Vector3.Distance(this.target, this.transform.position) < 0.1f);

        this.rb.velocity = Vector2.zero;
        this.GetComponent<EasyAnim>().duration *= 4f;
        UpdateFloor();
    }

    IEnumerator Dig() {
        yield return new WaitForSeconds(0.5f);
        var food = targetObj.GetComponentInChildren<MoleRatFood>();
        if (food == null) {
            Destroy(targetObj);
        } else {
            food.Die();
        }
        yield return new WaitForSeconds(0.5f);
    }
}