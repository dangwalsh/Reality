namespace HeadsUp {

    using UnityEngine;

    public class PositionModel : MonoBehaviour {

        public float scaleFactor = 10;

        Bounds bounds;

        void Start() {

            ExpandBounds(transform.parent);

            transform.localScale /= bounds.size.magnitude / scaleFactor;
            transform.localPosition += new Vector3(0, 0, 10);
        }

        private void ExpandBounds(Transform parent) {
            foreach (Transform child in parent) {
                var filter = child.GetComponent<MeshFilter>();
                if (filter != null) {
                    var mesh = filter.mesh;
                    if (mesh == null) continue;
                    this.bounds.Encapsulate(mesh.bounds);
                }
                ExpandBounds(child);
            }
        }
    }
}
