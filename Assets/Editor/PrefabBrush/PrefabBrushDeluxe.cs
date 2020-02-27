using System;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor.Tilemaps;

namespace UnityEditor.Tilemaps
{
    /// <summary>
    /// This Brush instances and places a randomly selected Prefabs onto the targeted location and parents the instanced object to the paint target. Use this as an example to quickly place an assorted type of GameObjects onto structured locations.
    /// </summary>
    [CreateAssetMenu(fileName = "Prefab Brush Deluxe", menuName = "Brushes/Prefab Brush Deluxe")]
    [CustomGridBrush(false, true, false, "Prefab Brush Deluxe")]
    public class PrefabBrushDeluxe : GridBrush
    {
        private const float k_PerlinOffset = 100000f;
        /// <summary>
        /// The selection of Prefabs to paint from
        /// </summary>
        public GameObject[] m_Prefabs;
        /// <summary>
        /// Factor for distribution of choice of Prefabs to paint
        /// </summary>
        public float m_PerlinScale = 0.5f;
        /// <summary>
        /// Anchor Point of the Instantiated Prefab in the cell when painting
        /// </summary>
        public Vector3 m_Anchor = new Vector3(0.5f, 0.5f, 0.5f);

        private GameObject prev_brushTarget;
        private Vector3Int prev_position = Vector3Int.one * Int32.MaxValue;

        /// <summary>
        /// Paints Prefabs into a given position within the selected layers.
        /// The PrefabBrush overrides this to provide Prefab painting functionality.
        /// </summary>
        /// <param name="gridLayout">Grid used for layout.</param>
        /// <param name="brushTarget">Target of the paint operation. By default the currently selected GameObject.</param>
        /// <param name="position">The coordinates of the cell to paint data to.</param>
        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            if (position == prev_position)
            {
                return;
            }
            prev_position = position;
            if (brushTarget) {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;

            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, k_PerlinOffset) * m_Prefabs.Length), 0, m_Prefabs.Length - 1);
            GameObject prefab = m_Prefabs[index];
            GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            if (instance != null)
            {
                Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
                Undo.RegisterCreatedObjectUndo((Object) instance, "Paint Prefabs");
                Transform objParent = GetCurrentRoom(grid, brushTarget, position);
                instance.transform.SetParent(objParent);
                instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + m_Anchor));
            }
        }

        /// <summary>
        /// Erases Prefabs in a given position within the selected layers.
        /// The PrefabBrush overrides this to provide Prefab erasing functionality.
        /// </summary>
        /// <param name="gridLayout">Grid used for layout.</param>
        /// <param name="brushTarget">Target of the erase operation. By default the currently selected GameObject.</param>
        /// <param name="position">The coordinates of the cell to erase data from.</param>
        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            if (brushTarget)
            {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            Transform objParent = GetCurrentRoom(grid, brushTarget, position);
            Transform erased = GetObjectInCell(grid, objParent, position);
            if (erased != null)
                Undo.DestroyObjectImmediate(erased.gameObject);
        }

        private Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
        {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Bounds bounds = new Bounds((max + min)*.5f, max - min);

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position) && IsOurPrefab(child.gameObject))
                    return child;
            }
            return null;
        }

        private bool IsOurPrefab(GameObject obj) {
            for (int i = 0; i < m_Prefabs.Length; i++) {
                if (m_Prefabs[i].name == obj.name) {
                    return true;
                }
            }

            return false;
        }

        private static float GetPerlinValue(Vector3Int position, float scale, float offset)
        {
            return Mathf.PerlinNoise((position.x + offset)*scale, (position.y + offset)*scale);
        }

        private static Transform GetCurrentRoom(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Vector3 center = (max + min) * 0.5f;

            Transform roomContainer = GameObject.FindGameObjectWithTag("RoomContainer").transform;
            for (int i = 0; i < roomContainer.childCount; i++) {
                Transform room = roomContainer.GetChild(i);
                Bounds roomBounds = new Bounds(room.position, new Vector3(9f, 9f));

                if (roomBounds.Contains(center)) {
                    return room;
                }
            }

            return brushTarget.transform;
        }
    }

    /// <summary>
    /// The Brush Editor for a Prefab Brush.
    /// </summary>
    [CustomEditor(typeof(PrefabBrushDeluxe))]
    public class PrefabBrushDeluxeEditor : GridBrushEditor
    {
        private PrefabBrushDeluxe prefabBrush { get { return target as PrefabBrushDeluxe; } }

        private SerializedProperty m_Prefabs;
        private SerializedProperty m_Anchor;
        private SerializedObject m_SerializedObject;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_SerializedObject = new SerializedObject(target);
            m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
            m_Anchor = m_SerializedObject.FindProperty("m_Anchor");
        }

        /// <summary>
        /// Callback for painting the inspector GUI for the PrefabBrush in the Tile Palette.
        /// The PrefabBrush Editor overrides this to have a custom inspector for this Brush.
        /// </summary>
        public override void OnPaintInspectorGUI()
        {
            m_SerializedObject.UpdateIfRequiredOrScript();
            prefabBrush.m_PerlinScale = EditorGUILayout.Slider("Perlin Scale", prefabBrush.m_PerlinScale, 0.001f, 0.999f);
            EditorGUILayout.PropertyField(m_Prefabs, true);
            EditorGUILayout.PropertyField(m_Anchor);
            m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}