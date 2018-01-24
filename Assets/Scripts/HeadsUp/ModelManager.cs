namespace HeadsUp {

    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ModelManager : MonoBehaviour {

        public float scaleFactor = 10;

        Bounds aggregateBounds;

        void Start() {

            AggregateMeshes();
            CenterOnPivot();

            transform.localScale /= aggregateBounds.size.magnitude / scaleFactor;
            transform.localPosition += new Vector3(0, 0, 10);
        }

        private void CenterOnPivot() {
            
            foreach (Transform child in transform)
                child.localPosition -= aggregateBounds.center;
        }

        private void AggregateMeshes() {

            var meshFilters = GetComponentsInChildren<MeshFilter>();
            if (meshFilters == null || meshFilters.Length == 0) return;
            var bounds = meshFilters[0].mesh.bounds;
            foreach (var meshFilter in meshFilters) {
                bounds.Encapsulate(meshFilter.mesh.bounds);
                meshFilter.mesh.RecalculateNormals(60);
            }

            aggregateBounds = bounds;
        }
    }
}