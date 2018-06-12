﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Curves.EditorTools {

    [CustomEditor(typeof(QuadMesh))]
    public class QuadMeshEditor : BaseMeshEditor {
        private const float HandleSize = 0.015f; 
        private SerializedProperty points;

        private ReorderableList pointsList;
        private string[] labels = { "Bottom Left", "Bottom Right", "Top Left", "Top Right" };

        protected override void OnEnable() {
            base.OnEnable();

            points = serializedObject.FindProperty("points");
            pointsList = new ReorderableList(serializedObject, points, false, false, false, false);

            onInspectorCallback += pointsList.DoLayoutList;
            onSceneCallback += DrawPointHandles;
            onChangeCallback += RegenerateMesh;

            pointsList.drawElementCallback = DrawPointElement;
            pointsList.drawHeaderCallback = DrawPointHeader;
            pointsList.elementHeightCallback = DrawElementHeight;

            // Undo support
            Undo.undoRedoPerformed = RegenerateMesh;
        }

        protected override void OnDisable() {
            onInspectorCallback -= pointsList.DoLayoutList;
            onSceneCallback -= DrawPointHandles;
            onChangeCallback -= RegenerateMesh;

            pointsList.drawElementCallback -= DrawPointElement;
            pointsList.drawHeaderCallback -= DrawPointHeader;
            pointsList.elementHeightCallback -= DrawElementHeight;

            // Undo deregister
            Undo.undoRedoPerformed -= RegenerateMesh;
        }

        private void DrawPointHandles() {
            Handles.color = Color.green;
            var size = points.arraySize;
            var transform = (target as MonoBehaviour).transform;
            for (int i = 0; i < size; i++) {
                var point = points.GetArrayElementAtIndex(i);
                var position = transform.TransformPoint(point.vector3Value);
                position = Handles.FreeMoveHandle(position, Quaternion.identity, HandleSize, Vector3.one * HandleSize, Handles.DotHandleCap);
                point.vector3Value = transform.InverseTransformPoint(position);
            }
        }

#region PointsCallback
        private void DrawPointElement(Rect r, int i, bool isActive, bool isFocused) {
            var element = points.GetArrayElementAtIndex(i);
            EditorGUI.PropertyField(r, element, new GUIContent(labels[i]));
        }

        private void DrawPointHeader(Rect r) {
            EditorGUI.LabelField(r, new GUIContent("Quad Points", "What points define the the quad?"), EditorStyles.boldLabel);
        }

        private float DrawElementHeight(int i) {
            return EditorGUIUtility.singleLineHeight;
        }
#endregion

        private void ResetHeight() {
            var arraySize = points.arraySize;

            for (int i = 0; i < arraySize; i++) {
                var element = points.GetArrayElementAtIndex(i);
                var position = new Vector3(element.vector3Value.x, 0f, element.vector3Value.z);
                element.vector3Value = position;
            }
        }
    }
}
