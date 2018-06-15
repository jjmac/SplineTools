﻿using CommonStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Curves {

    public class BezierMesh : BaseMesh {

        public Bezier bezier;
        [Tooltip("How wide are the curves away from each other?")]
        public float width = 1f;
        public int resolution = 1;
        [Range(5, 100), Tooltip("How many line segments define the bezier curve?")]
        public int segments = 10;
#if UNITY_EDITOR
        public bool drawGizmos;
        public Color gizmoColor = Color.green;
#endif
        // Store the vertices for the mesh.
        private Tuple<Vector3, Vector3>[] vertices;
        private int[] triangles;

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (drawGizmos) {
                Gizmos.color = gizmoColor;
                GeneratePoints();
                
                foreach (var vertex in vertices) {
                    Gizmos.DrawSphere(vertex.item1, 0.5f);
                    Gizmos.DrawSphere(vertex.item2, 0.5f);
                }
                
                for (int i = 1; i < vertices.Length; i++) {
                    var start = vertices[i - 1];
                    var end = vertices[i];

                    Gizmos.DrawLine(start.item1, end.item1);
                    Gizmos.DrawLine(start.item2, end.item2);
                }
            }
        }
#endif
        private void GeneratePoints() {
            try {
                vertices = bezier.GetCubicBezierPoints(segments, width);
            } catch (System.NullReferenceException) { }
        }
        
        private void GenerateTriangles(int splineCount) {
            var mTriangles = new List<int>();

            for (int ti = 0, vi = 0, y = 0; y < (segments * (splineCount - 1)) + (splineCount - 2); y++, vi++) {
                for (int x = 0; x < resolution; x++, ti += 6, vi++) {
                    mTriangles.Add(vi);
                    mTriangles.Add(vi + resolution + 1);
                    mTriangles.Add(vi + 1);
                    mTriangles.Add(vi + 1);
                    mTriangles.Add(vi + resolution + 1);
                    mTriangles.Add(vi + resolution + 2);
                }
            }

            triangles = mTriangles.ToArray();
        }

        public override void GenerateMesh() {
            GeneratePoints();

            meshFilter = GetComponent<MeshFilter>();
            meshGenerator = meshGenerator?? new MeshGenerator();
            meshGenerator.Clear();

            var mVertices = new List<Vector3>();

            foreach (var tuple in vertices) {
                for (int t = 0; t <= resolution; t++) {
                    var progress = ((float) t) / ((float) resolution);
                    var pt = Vector3.Lerp(tuple.item1, tuple.item2, progress);
                    mVertices.Add(pt);
                }
            }
            GenerateTriangles(bezier.points.Length);

            var mesh = meshGenerator.CreateMesh();
            mesh.SetVertices(mVertices);
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            
            meshFilter.mesh = mesh;
        }
    }
}
