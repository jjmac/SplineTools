﻿using UnityEngine;

namespace Curves {

    // TODO: Add looping functionality of Bezier curves
    
    [CreateAssetMenu(menuName = "Curves/Bezier", fileName = "Bezier Curve")]
    public class BezierTrack : ScriptableObject {

        [SerializeField, HideInInspector]
        private Vector3[] points = { Vector3.zero, Vector3.forward * 10f };
        [SerializeField, HideInInspector]
        private Vector3[] controlPoints = { new Vector3(-2.5f, 0f, 2.5f), new Vector3(2.5f, 0f, 7.5f) };
        
        /// <summary>
        /// Gets a point along the tangent of the cubic bezier curve.
        /// </summary>
        /// <param name="p0">Start point</param>
        /// <param name="p1">First control point</param>
        /// <param name="p2">Second control point</param>
        /// <param name="p3">End point</param>
        /// <returns>A point along the cubic bezier curve</returns>
        public static Vector3 GetCubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets a point along the tangent of the quadratic bezier curve.
        /// </summary>
        /// <param name="p0">Start point</param>
        /// <param name="p1">Control point</param>
        /// <param name="p2">End point</param>
        /// <returns>A point along the quadratic bezier curve</returns>
        public static Vector3 GetQuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
            throw new System.NotImplementedException();
        }
    }
}