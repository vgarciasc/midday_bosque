using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string objectName = "default";
    public int quantity = 1;

    void OnTriggerEnter2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.tag == "Player") {
            obj.GetComponent<PlayerInventory>().Change(objectName, quantity);
            Destroy(this.gameObject);
        }   
    }
}
