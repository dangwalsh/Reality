namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class ModelManager : MonoBehaviour
    {
        public float scaleFactor = 1;
        public float xOffset = 0;
        public float yOffset = 0;
        public float zOffset = 0;

        Bounds aggregateBounds;
        HoloToolkit.Unity.InputModule.Cursor defaultCursor;
        //HandTransformation handTransformation;
        ModelAnchorManager modelAnchorManager;

        #region MonoBehaviour Members
        void Start()
        {

            //handTransformation = GetComponent<HandTransformation>();
            //defaultCursor = GameObject.Find("DefaultCursor").GetComponent<AnimatedCursor>();
            //transform.parent.position = Camera.main.transform.TransformDirection(defaultCursor.Position);
            //transform.parent.position = Camera.main.transform.InverseTransformDirection(defaultCursor.Position);
            //transform.parent.localPosition = defaultCursor.Position;
            var sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
            transform.position = sceneManager.HitPoint;
        }
        #endregion

        public void OrientModel()
        {
            AggregateMeshes();
            PositionOnPivot();
            transform.localScale /= aggregateBounds.size.magnitude / 1;
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