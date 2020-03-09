using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapHelper : MonoBehaviour
{
    [System.Serializable]
    public class FloorTilemap {
        public Tilemap map;
        public int level;

        public FloorTilemap(Tilemap map, int level) {
            this.map = map;
            this.level = level;
        }
    }

    static int maxFloorLevel = 3;

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
        
        foreach (var tilemap in GetAllFloors()) {
            var targetTile = tilemap.map.GetTile(posInt);
            if (targetTile != null) {
                maxLevel = tilemap.level > maxLevel ? tilemap.level : maxLevel;
            }
        }

        return maxLevel;
    }

    public static List<FloorTilemap> GetAllFloors() {
        var output = new List<FloorTilemap>();
        new List<Tilemap>(FindObjectsOfType<Tilemap>())
            .FindAll((tilemap) => {
                string tilemapLayerName = LayerMask.LayerToName(tilemap.gameObject.layer);
                return tilemapLayerName.Contains("Floor");
            })
            .ForEach((tilemap) => {
                output.Add(new FloorTilemap(tilemap, GetFloorLevel(tilemap)));
            });
        return output;
    }

    private static int GetFloorLevel(Tilemap tilemap) {
        string tilemapLayerName = LayerMask.LayerToName(tilemap.gameObject.layer);
        return int.Parse(tilemapLayerName.Split('_')[1]);
    }

    public static Vector3 TilePosInFrontOfObj(GameObject obj, Vector3 direction, bool local = true) {
        Vector3 target = local ? obj.transform.localPosition : obj.transform.position;
        target = target + direction * 0.5f;
        target = HushPuppy.GetVecAsTileVec(target, false, true);
        return target;
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

    public static bool InPlayerRoom(GameObject obj) {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;
        return TilemapHelper.InsideSameRoom(
            obj.transform.position,
            player.transform.position,
            RoomManager.Get().dimensions);
    }  
}
