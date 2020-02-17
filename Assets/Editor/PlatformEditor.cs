using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Platform))]
[CanEditMultipleObjects]
public class PlatformEditor : Editor
{
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Platform platform = (Platform) target;
        Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();

        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal("Box1");
        if (GUILayout.Button("UP", EditorStyles.miniButton, GUILayout.Width(100))) {
			platform.direction = Vector3.up;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		}
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("Box2");
		if (GUILayout.Button("LEFT", EditorStyles.miniButton, GUILayout.Width(50))) {
			platform.direction = Vector3.left;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		}
		if (GUILayout.Button("RIGHT", EditorStyles.miniButton, GUILayout.Width(50))) {
			platform.direction = Vector3.right;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		}
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("Box3");
		if (GUILayout.Button("DOWN", EditorStyles.miniButton, GUILayout.Width(100))) {
			platform.direction = Vector3.down;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		}
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        if (GUI.changed) {
            EditorUtility.SetDirty(platform);
            EditorUtility.SetDirty(rb);
            EditorSceneManager.MarkSceneDirty(platform.gameObject.scene);
        }
	}
}
