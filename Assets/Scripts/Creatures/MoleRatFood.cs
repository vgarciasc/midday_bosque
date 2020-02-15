using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleRatFood : MonoBehaviour
{
    public GameObject under;
    
    void Start() {
        under.SetActive(false);
    }

    public void Die() {
        foreach (var collider in this.GetComponents<Collider2D>()) {
            collider.enabled = false;
        }

        under.SetActive(true);
    }
}
