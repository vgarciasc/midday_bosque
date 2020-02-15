using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapHelper : MonoBehaviour
{
    static int maxFloorLevel = 2;

    public static Tilemap GetWallsTilemap() {
        foreach (var tilemap in GameObject.FindObjectsOfType<Tilemap>()) {
            if (tilemap.gameObject.layer == LayerMask.NameToLayer("Walls")) {
                return tilemap;
            }
        }
        return null;
    }

    public static bool IsWall(Vector3Int position) {
        var walls = GetWallsTilemap();
        return walls.GetTile(position) != null;
    }

    public static int GetFloor(Vector3 position) {
        var posInt = HushPuppy.GetVecAsTileVec(position);
        int maxLevel = -1;
        
        var tilemaps = new List<Tilemap>(FindObjectsOfType<Tilemap>());
        foreach (var tilemap in tilemaps) {
            string tilemapLayerName = LayerMask.LayerToName(tilemap.gameObject.layer);
            if (!tilemapLayerName.Contains("Floor")) {
                continue;
            }

            int tilemapLevel = int.Parse(tilemapLayerName.Split('_')[1]);
            tilemapLevel -= 1; //in the layers, we start counting on 1
            
            var targetTile = tilemap.GetTile(posInt);

            if (targetTile != null) {
                maxLevel = tilemapLevel > maxLevel ? tilemapLevel : maxLevel;
            }
        }

        return maxLevel;
    }

    public static Vector3 TilePosInFrontOfObj(GameObject obj, Vector3 direction) {
        Vector3 target = obj.transform.localPosition;
        target = target + direction * 0.5f;
        target = HushPuppy.GetVecAsTileVec(target, false, true);
        return target; //SHOULD BE localPosition!
    }

    public static Vector3 GetRoomCenter(Vector3 obj_pos, Vector2 dim) {
        var roomIdx = new Vector2(
            (Mathf.Abs(obj_pos.x) + (dim.x / 2f)) / dim.x,
            (Mathf.Abs(obj_pos.y) + (dim.y / 2f)) / dim.y);

        roomIdx = new Vector2(
            Mathf.Floor(roomIdx.x),
            Mathf.Floor(roomIdx.y));

        var roomCenter = new Vector3(
            roomIdx.x * dim.x * Mathf.Sign(obj_pos.x),
            roomIdx.y * dim.y * Mathf.Sign(obj_pos.y));

        return roomCenter;
    }

    public static bool InsideSameRoom(Vector3 vec1, Vector3 vec2, Vector2 dim) {
        return GetRoomCenter(vec1, dim) == GetRoomCenter(vec2, dim);
    }

    public static int GetLayerMaskCreatureCollision(int level) {
        int k = 0;
        for (int i = 0; i < maxFloorLevel; i++) {
            if (i == level) continue;
            k |= 1 << LayerMask.NameToLayer("Floor_" + level.ToString());
        }
        return k;
    }
}
