namespace HeadsUp
{

    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ModelManager : MonoBehaviour
    {

        public float scaleFactor = 10;
        public float distanceFrom = 10;

        Bounds aggregateBounds;

        void Start()
        {
            transform.localPosition += new Vector3(0, 0, distanceFrom);
        }

        public void OrientModel()
        {
            AggregateMeshes();
            PositionOnPivot();
            transform.localScale /= aggregateBounds.size.magnitude / scaleFactor;
        }

        private void PositionOnPivot()
        {
            var objectBase = aggregateBounds.center - new Vector3(0, aggregateBounds.extents.y, 0);
            foreach (Transform child in transform)
                child.localPosition -= objectBase;
        }

        private void AggregateMeshes()
        {
            var meshFilters = GetComponentsInChildren<MeshFilter>();
            if (meshFilters == null || meshFilters.Length == 0) return;

            var bounds = meshFilters[0].mesh.bounds;
            foreach (var meshFilter in meshFilters)
            {
                bounds.Encapsulate(meshFilter.mesh.bounds);
                meshFilter.mesh.RecalculateNormals(60);
            }

            aggregateBounds = bounds;
        }
    }
}