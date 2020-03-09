using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Sensors : MonoBehaviour
{
    public List<string> everSeenMB = new List<string>();
    public List<GameObject> observations = new List<GameObject>();

    public UnityEvent explosionEvent;
    
    void OnTriggerEnter2D(Collider2D collider) {
        observations.Add(collider.gameObject);
        AddEverSeen(collider.gameObject);
    }

    void OnTriggerExit2D(Collider2D collider) {
        int index = observations.FindIndex((f) => f == collider.gameObject);
        observations.RemoveAt(index);
    }

    void AddEverSeen(GameObject obj) {
        foreach (MonoBehaviour mb in obj.GetComponents<MonoBehaviour>()) {
            string mbstr = mb.GetType().ToString();
            if (!everSeenMB.Contains(mbstr)) {
                everSeenMB.Add(mbstr);

                switch (mbstr) {
                    case "ChemExplosion": explosionEvent.Invoke(); break;
                }
            }
        }
    }
}
