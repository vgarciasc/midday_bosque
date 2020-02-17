using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int index = -1;
    public Transform startingPos;

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
}
