using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour
{
    public List<GameObject> observations = new List<GameObject>();
    
    void OnTriggerEnter2D(Collider2D collider) {
        observations.Add(collider.gameObject);
    }

    void OnTriggerExit2D(Collider2D collider) {
        int index = observations.FindIndex((f) => f == collider.gameObject);
        observations.RemoveAt(index);
    }
}
