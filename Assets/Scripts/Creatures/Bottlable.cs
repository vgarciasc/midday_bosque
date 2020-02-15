using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BottlableData {
    public string prefabName;
    public Sprite sprite;

    public BottlableData(string prefabName, Sprite sprite) {
        this.prefabName = prefabName;
        this.sprite = sprite;
    }
}

public class Bottlable : MonoBehaviour
{
    public bool useOwnSprite = true;
    public BottlableData data;

    void Start() {
        if (useOwnSprite) {
            data.sprite = this.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
