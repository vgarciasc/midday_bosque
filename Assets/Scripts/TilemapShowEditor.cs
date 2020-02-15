using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[ExecuteInEditMode]
public class TilemapShowEditor : MonoBehaviour
{
    public bool ShouldAppearInEditMode = false;
    public bool ShouldAppearInPlayMode = false;

    void Update()
    {
        if (ShouldAppearInEditMode) {
            this.GetComponent<TilemapRenderer>().enabled = !Application.isPlaying;
        } else if (ShouldAppearInPlayMode) {
            this.GetComponent<TilemapRenderer>().enabled = Application.isPlaying;
        }
    }
}
