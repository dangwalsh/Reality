namespace HeadsUp {

    using UnityEngine;

    public class SliderManager : MonoBehaviour {

        public GameObject ControlKnob;
        public GameObject NegativeSide;
        public GameObject PositiveSide;
        public GameObject Icon;

        private LineRenderer negativeLine;
        private LineRenderer positiveLine;
        private MeshRenderer iconRenderer;

        void Start() {

            negativeLine = NegativeSide.GetComponent<LineRenderer>();
            positiveLine = PositiveSide.GetComponent<LineRenderer>();
            iconRenderer = Icon.GetComponent<MeshRenderer>();
        }

        void Update() {

            var color = iconRenderer.material.color;
            var location = ControlKnob.transform.localPosition;
            var offset = location.x;

            //offset = (offset > 1.0f) ? 1.0f : offset;
            //offset = (offset < 0.0f) ? 0.0f : offset;

            ControlKnob.transform.localPosition = new Vector3(offset, 0, 0);

            //negativeLine.SetPosition(1, new Vector3(offset/* + .07f*/, 0, 0));
            positiveLine.SetPosition(1, new Vector3(offset/* - .07f*/, 0, 0));
            iconRenderer.material.color = new Color(color.r, color.g, color.b, Distribute(offset));
        }

        float Distribute(float x) {

            return .85f * x + .125f;
        }
    }
}
