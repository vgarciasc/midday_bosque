using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    [HideInInspector]
    public bool active = false;

    public List<RoomButtonObject> linked = new List<RoomButtonObject>();

    List<GameObject> onTop = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.tag == "Walls" || obj.tag == "Windows") {
            return;   
        }
        onTop.Add(obj);
        
        if (!active) {
            active = true;
            ToggleButton(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (onTop.Contains(obj)) {
            onTop.Remove(obj);
        
            if (onTop.Count == 0 && active) {
                active = false;
                ToggleButton(false);
            }
        }
    }

    void ToggleButton(bool value) {
        foreach (var obj in linked) {
            if (!obj.gameObject.activeSelf) {
                obj.gameObject.SetActive(true);
            }

            obj.Toggle(value);
        }
    }
}
