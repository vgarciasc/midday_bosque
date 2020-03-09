using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoleRat : MonoBehaviour
{
    public enum State {
        IDLE, MOVING, DIGGING
    };


    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    float distance = 2f;
    [SerializeField]
    SpriteRenderer exclamationMark;

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
        this.exclamationMark.color = Color.clear;

        UpdateFloor();
        StartCoroutine(Loop());
    }

    void Update() {
        // print(Vector3.Distance(target, this.transform.position));
        this.sr.flipX = (target.x - this.transform.position.x > 0);
        // this.sr.flipX = (rb.velocity.x > 0);
        this.rb.velocity = this.rb.velocity.magnitude > this.speed ? 
            (this.rb.velocity.normalized) * this.speed
            : this.rb.velocity;
    }

    void UpdateFloor() {
        this.currFloorLevel = TilemapHelper.GetFloor(this.transform.position);
    }

    IEnumerator Loop() {
        while (true) {
            var food = sensors.observations.Find((f) => f.CompareTag("MoleRatFood"));
            if (state != State.DIGGING && food != null 
                && TilemapHelper.InPlayerRoom(this.gameObject)) {
                targetObj = food;
                target = food.transform.position;
                state = State.MOVING;
                // yield return ShowExclamation();
            }

            switch (state) {
                case State.IDLE:
                    yield return Idle();
                    state = State.MOVING;
                    break;
                case State.MOVING:
                    yield return Move(food != null);
                    if (food != null
                        && Vector3.Distance(this.transform.position, food.transform.position) < 0.1f) {
                        state = State.DIGGING;
                    } else {
                        state = State.IDLE;
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

    IEnumerator Move(bool goingToFood) {
        this.GetComponent<EasyAnim>().duration /= goingToFood ? 4f : 1.5f;
        this.rb.velocity = (this.target - this.transform.position).normalized * this.speed;

        float startTime = Time.time;
        Vector3 startPos = this.transform.position;
        float initialDistance = Vector3.Distance(this.target, this.transform.position);
        
        yield return new WaitUntil(
            () => {
                float distanceToTarget = Vector3.Distance(this.target, this.transform.position);
                float velocity = Vector3.Distance(startPos, this.transform.position) / (Time.time - startTime);
                return distanceToTarget < 0.1f
                    || distanceToTarget > initialDistance //wrong way
                    || velocity < this.speed * 0.8f; //got stuck
            });

        this.rb.velocity = Vector2.zero;
        this.GetComponent<EasyAnim>().duration *= goingToFood ? 4f : 1.5f;
        UpdateFloor();
    }

    IEnumerator Dig() {
        if (targetObj == null) {
            yield break;
        }

        yield return new WaitForSeconds(0.5f);
        var food = targetObj.GetComponentInChildren<MoleRatFood>();
        if (food == null) {
            Destroy(targetObj);
        } else {
            food.Die();
        }
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator ShowExclamation() {
        exclamationMark.transform.localScale = new Vector3(1.3f, 0.7f, 1f);
        exclamationMark.DOColor(Color.white, 0.05f);
        exclamationMark.transform.DOScale(new Vector3(0.7f, 1.3f, 1f), 0.1f);
        yield return new WaitForSeconds(0.1f);
        exclamationMark.transform.DOScale(Vector3.one, 0.1f);
        yield return new WaitForSeconds(0.3f);
        exclamationMark.DOColor(Color.clear, 0.05f);
        yield return new WaitForSeconds(0.5f);
    }
}
