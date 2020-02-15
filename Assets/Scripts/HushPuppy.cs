﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using System;

static class HushPuppy {
    public static GameObject safeFind(string name) {
        GameObject go = GameObject.FindGameObjectWithTag(name);
        if (go == null) {
            Debug.Log("'" + name + "' not found.");
            Debug.Break(); }

        return go;
    }

    public static Component safeComponent(GameObject go, string componentName) {
        Component c = go.GetComponent(componentName);
        if (c == null) {
            Debug.Log("'" + componentName + "' component not found in GameObject '" + go.name + "'.");
            Debug.Break(); }

        return c;
    }

    public static Component safeFindComponent(string gameObjectName, string componentName) {
        GameObject go = safeFind(gameObjectName);
        if (go == null) {
            Debug.Log("'" + gameObjectName + "' not found.");
            Debug.Break();
            return null; }
        Component c = safeComponent(go, componentName);
        return c;
    }

    //Static methods that should exist in unity
    #region Unity Methods
    public static bool IsAtTile(Vector3 beingChecked, Vector3 position) {
        return (beingChecked.x > (position.x - 1)
                && beingChecked.x < (position.x + 1)
                && beingChecked.y > (position.y - 1)
                && beingChecked.y < (position.y + 1));
    }

    public static List<Vector3> GetAdjacentTiles(Vector3 pos) {
        return new List<Vector3>() {
            new Vector3(pos.x - 1, pos.y, pos.z),
            new Vector3(pos.x + 1, pos.y, pos.z),
            new Vector3(pos.x, pos.y - 1, pos.z),
            new Vector3(pos.x, pos.y + 1, pos.z)
        };
    }

    public static List<Vector3Int> GetTilemapTiles(Tilemap tilemap) {
        var output = new List<Vector3Int>();
        var bounds = tilemap.localBounds;
        for (int i = (int) bounds.min.x; i < bounds.max.x; i++) {
            for (int j = (int) bounds.min.y; j < bounds.max.y; j++) {
                Vector3Int posInt = new Vector3Int(i, j, 0);
                output.Add(posInt);
            }
        }
        return output;
    }

    public static Vector3Int GetVecAsTileVec(Vector3 position, bool floor = true, bool round = false) {
        if (floor) {
            return new Vector3Int(
                Mathf.FloorToInt(position.x),
                Mathf.FloorToInt(position.y),
                0);
        } else if (round) {
            return new Vector3Int(
                Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.y),
                0);
        } else {
            return new Vector3Int(
                (int) position.x,
                (int) position.y,
                0);
        }
    }

    public static Vector3 GenerateValidPosition(
        Func<Vector3> generate,
        Func<Vector3, bool> validate) 
    {
        Vector3 output;
        do {
            output = generate();
        } while (!validate(output));
        
        return output;
    }

    public static Vector3 RandomDir() {
        var dice = UnityEngine.Random.Range(0, 4);
        switch (dice) {
            case 0: return Vector2.right;
            case 1: return Vector2.left;
            case 2: return Vector2.up;
            case 3: return Vector2.down;
        }
        return Vector2.zero;
    }

    public static float RandomFloat(Vector3 vector) {
        return UnityEngine.Random.Range(vector.x, vector.y);
    }

    public static int RandomInt(Vector3 vector) {
        return UnityEngine.Random.Range((int) vector.x, (int) vector.y);
    }

	public static void BroadcastAll(string fun, System.Object msg) {
		GameObject[] gos = (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
			}
    	}
	}
	
    public static void BroadcastAll(string fun) {
		GameObject[] gos = (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(fun, SendMessageOptions.DontRequireReceiver);
			}
    	}
	}

    public static void destroyChildren(GameObject go) {
        foreach (Transform child in go.transform)
            GameObject.Destroy(child.gameObject);
    }

    public static IEnumerator WaitForEndOfFrames(int frames) {
        for (int i = 0; i < frames; i++) {
            yield return new WaitForEndOfFrame();
        }
    }

    public static void switchVisibilityOfChildren(GameObject g, bool active) {
        foreach (Image i in g.GetComponentsInChildren<Image>())
            i.enabled = active;
        foreach (Text t in g.GetComponentsInChildren<Text>())
            t.enabled = active;
    }

    public static void switchRaycastsOfChildren(GameObject g, bool active) {
        foreach (Transform child in g.transform)
            child.GetComponent<CanvasGroup>().blocksRaycasts = active;
    }

    public static void fadeImgIn(GameObject g, float duration) {
        g.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        g.GetComponent<Image>().CrossFadeAlpha(1f, duration, false);
    }

    public static void fadeImgOut(GameObject g, float duration) {
        g.GetComponent<Image>().canvasRenderer.SetAlpha(1.0f);
        g.GetComponent<Image>().CrossFadeAlpha(0f, duration, false);
    }

    public static void fadeIn(GameObject g, float duration) {
        foreach (Image i in g.GetComponentsInChildren<Image>()) {
            i.canvasRenderer.SetAlpha(0.0f);
            i.CrossFadeAlpha(1f, duration, false);
        }
        foreach (Text t in g.GetComponentsInChildren<Text>()) {
            t.canvasRenderer.SetAlpha(0.0f);
            t.CrossFadeAlpha(1f, duration, false);
        }
    }

    public static void fadeOut(GameObject g, float duration) {
        foreach (Image i in g.GetComponentsInChildren<Image>()) {
            i.canvasRenderer.SetAlpha(1.0f);
            i.CrossFadeAlpha(1f, duration, false);
        }
        foreach (Text t in g.GetComponentsInChildren<Text>()) {
            t.canvasRenderer.SetAlpha(1.0f);
            t.CrossFadeAlpha(1f, duration, false);
        }
    }

    public static void acrobaticSilhouette(GameObject g, bool zh) {
        if (zh) {
            foreach (Image i in g.GetComponentsInChildren<Image>())
                i.color = new Color(0f, 0f, 0f);
        } else {
            foreach (Image i in g.GetComponentsInChildren<Image>())
                i.color = new Color(1f, 1f, 1f);
        }
    }

    public static void acrobaticSilhouetteFade(GameObject g, float duration, bool zh) {
        if (zh) {
            foreach (Image i in g.GetComponentsInChildren<Image>()) {
                i.canvasRenderer.SetAlpha(1.0f);
                i.CrossFadeColor(new Color(0f, 0f, 0f), duration, false, false);
            }
        } else {
            foreach (Image i in g.GetComponentsInChildren<Image>()) {
                i.canvasRenderer.SetAlpha(1.0f);
                i.CrossFadeColor(new Color(1f, 1f, 1f), duration, false, false);
            }
        }
    }

    public static void changeOpacity(GameObject g, float value) {
        foreach (Image aux in g.GetComponentsInChildren<Image>())
            aux.color = new Color(aux.color.r, aux.color.g, aux.color.b, value);

        foreach (Text aux in g.GetComponentsInChildren<Text>())
            aux.color = new Color(aux.color.r, aux.color.g, aux.color.b, value);
    }

    public static Color getColorWithOpacity(Color color, float opacity) {
        if (opacity < 0f || opacity > 1f) return color;
        return new Color(color.r, color.g, color.b, opacity);
    }

    public static Color invertColor(Color color) {
        return (new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a));
    }

    //from unify community wiki
    public static string ColorToHex(Color32 color) {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
    
    //from unify community wiki
    public static Color HexToColor(string hex) {
        byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r,g,b, 255);
    }
    #endregion
}