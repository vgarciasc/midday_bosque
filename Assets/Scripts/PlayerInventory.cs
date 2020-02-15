using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [HideInInspector]
    public int fireballNum = 0;

    public void Reset() {
        fireballNum = 0;
    }

    public void Change(string name, int amount) {
        switch (name) {
            case "fireball":
                fireballNum += amount;
                break;
            default:
                Debug.LogError("This should not be happening.");
                Debug.Break();
                break;
        }
    }
}
