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

    [Header("References")]
    [SerializeField]
    private SpriteRenderer fakeWall;

    void Start() {
        this.tilemaps = new List<Tilemap>(FindObjectsOfType<Tilemap>());
        this.walls = TilemapHelper.GetWallsTilemap();
    }

    void OnEnable() {
        ClearPath();
    }

    bool IsHorizontal() {
        var rotation = this.transform.rotation.eulerAngles.z;
        return rotation == 90f || rotation == -90f;
    }

    public virtual int GetTopLevel() {
        var stairVec = HushPuppy.GetVecAsTileVec(this.transform.position);
        var stairTopVec = stairVec + Vector3Int.up;
        return GetLevel(stairTopVec);
    }

    public virtual int GetBaseLevel() {
        var stairVec = HushPuppy.GetVecAsTileVec(this.transform.position);
        var stairBaseVec = stairVec + Vector3Int.down;
        return GetLevel(stairBaseVec);
    }

    public virtual bool IsObjGoingUp(GameObject obj) {
        var diff = obj.transform.position - this.transform.position;
        return diff.y > 0;
    }

    public virtual bool IsObjGoingDown(GameObject obj) {
        var diff = obj.transform.position - this.transform.position;
        return diff.y < 0;
    }

    public int GetLevel(Vector3Int nextTileInt) {
        foreach (var tilemap in tilemaps) {
            string tilemapLayerName = LayerMask.LayerToName(tilemap.gameObject.layer);
            if (!tilemapLayerName.Contains("Floor")) {
                continue;
            }
            
            int tilemapLevel = int.Parse(tilemapLayerName.Split('_')[1]);
            var nextTile = tilemap.GetTile(nextTileInt);

            if (nextTile != null) {
                return tilemapLevel;
            }
        }

        // Debug.LogError("This should not be happening.");
        // Debug.Break();
        return -1; 
    }

    void ClearPath() {
        var walls = TilemapHelper.GetWallsTilemap();
        var posInt = HushPuppy.GetVecAsTileVec(this.transform.position);

        var tile = walls.GetTile(posInt);
        if (tile != null) {
            var sprite = ((Tile) tile).sprite;
            fakeWall.sprite = sprite;
            walls.SetTile(posInt, null);
        }
    }
}
