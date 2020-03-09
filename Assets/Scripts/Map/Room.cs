using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int index = -1;
    public Transform startingPos;
    public bool volumeToneDown;

    public UnityEvent onFirstEnter;
    
    bool playerHasEntered = false;

    public List<GameObject> GetGameObjectsAtTile(Vector3 position, bool onlyActive = true) {
        var output = new List<GameObject>();
        foreach (Transform child in this.transform) {
            if (child.position.x > (position.x - 1)
                && child.position.x < (position.x + 1)
                && child.position.y > (position.y - 1)
                && child.position.y < (position.y + 1)) {
                    output.Add(child.gameObject);
                }
        }
        if (onlyActive) {
            output = output.FindAll((f) => f.gameObject.activeSelf);
        }

        return output;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.CompareTag("Player")) {
            if (!playerHasEntered) {
                playerHasEntered = true;
                OnFirstEnter();
            }
        }   
    }

    public void OnFirstEnter() {
        onFirstEnter.Invoke();
    }
}
