using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Stair : MonoBehaviour
{
    int topLevel = 1;
    int baseLevel = 0;

    List<Tilemap> tilemaps;
    Tilemap walls;

    public GameObject blockLeft;
    public GameObject blockRight;

    void Start() {
        this.tilemaps = new List<Tilemap>(FindObjectsOfType<Tilemap>());
        this.walls = TilemapHelper.GetWallsTilemap();
    }

    void Update() {
        HandleBlocks();
    }

    bool IsHorizontal() {
        var rotation = this.transform.rotation.eulerAngles.z;
        return rotation == 90f || rotation == -90f;
    }

    public virtual int GetTopLevel() {
        var stairVec = HushPuppy.GetVecAsTileVec(this.transform.position);
        var stairTopVec = stairVec - Vector3Int.up;
        return GetLevel(stairTopVec);
    }

    public virtual int GetBaseLevel() {
        var stairVec = HushPuppy.GetVecAsTileVec(this.transform.position);
        var stairBaseVec = stairVec - Vector3Int.down;
        return GetLevel(stairBaseVec);
    }

    public virtual bool IsObjGoingUp(GameObject obj) {
        var diff = obj.transform.position - this.transform.position;
        return diff.y < 0;
    }

    public virtual bool IsObjGoingDown(GameObject obj) {
        var diff = obj.transform.position - this.transform.position;
        return diff.y > 0;
    }

    public int GetLevel(Vector3Int nextTileInt) {
        foreach (var tilemap in tilemaps) {
            string tilemapLayerName = LayerMask.LayerToName(tilemap.gameObject.layer);
            if (!tilemapLayerName.Contains("Floor")) {
                continue;
            }

            int tilemapLevel = int.Parse(tilemapLayerName.Split('_')[1]);
            tilemapLevel -= 1; //in the layers, we start counting on 1
            var nextTile = tilemap.GetTile(nextTileInt);

            if (nextTile != null) {
                return tilemapLevel;
            }
        }

        // Debug.LogError("This should not be happening.");
        // Debug.Break();
        return -1; 
    }

    void HandleBlocks() {
        if (blockRight == null || blockLeft == null) return;
        blockRight.SetActive(!ShouldIgnoreBlock(Vector2.right));
        blockLeft.SetActive(!ShouldIgnoreBlock(Vector2.left));
    }

    bool ShouldIgnoreBlock(Vector2 dir) {
        Vector3Int currentInt = HushPuppy.GetVecAsTileVec(this.transform.position);
        Vector3Int posInt = currentInt + HushPuppy.GetVecAsTileVec(dir);

        var hits = Physics2D.RaycastAll(
            new Vector2(
                currentInt.x + 0.5f,
                currentInt.y + 0.5f),
            dir, 1f);
        foreach (var hit in hits) {
            if (hit.transform.gameObject.CompareTag("Stair")) {
                return true;
            }
        }

        return false;
    }
}
