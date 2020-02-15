using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class Slorg : MonoBehaviour
{
    public GameObject slorgSlimePrefab;
    public float speed;

    Tilemap walls;
    Rigidbody2D rb;
    
    List<Vector3Int> tilesToSlime = new List<Vector3Int>();

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        this.walls = TilemapHelper.GetWallsTilemap();
    }

    void Update()
    {
        ClimbWall();
        LeaveSlime();
    }

    void ClimbWall() {
        Vector3Int currentInt = GetCurrentPosInt();
        Vector3Int aboveInt = currentInt + Vector3Int.up;
        Vector3Int belowInt = currentInt + Vector3Int.down;

        if (TilemapHelper.IsWall(currentInt)
            || TilemapHelper.IsWall(aboveInt)
            || TilemapHelper.IsWall(belowInt)) {
            this.rb.velocity = Vector2.up * speed;

            if (!tilesToSlime.Contains(currentInt)) {
                tilesToSlime.Add(currentInt);
            }
        } else {
            this.rb.velocity = Vector2.zero;
        }
    }
    
    void LeaveSlime() {
        Vector3Int currentInt = GetCurrentPosInt();
        Vector3Int belowInt = currentInt + Vector3Int.down;
            
        if (tilesToSlime.Contains(belowInt)) {
            tilesToSlime.Remove(belowInt);
            if (TilemapHelper.IsWall(belowInt)) {
                Instantiate(
                    slorgSlimePrefab,
                    belowInt + new Vector3(0.5f, 0.5f, 0),
                    Quaternion.identity,
                    this.transform.parent);
            }
        }
    }

    Vector3Int GetCurrentPosInt() {
        return HushPuppy.GetVecAsTileVec(new Vector3(
            this.transform.position.x - 0.5f,
            this.transform.position.y - 0.5f,
            this.transform.position.z
        ));
    }
}
