namespace HeadsUp {

    using UnityEngine;

    public class PositionModel : MonoBehaviour {

        void Start() {

            transform.localPosition += new Vector3(0, 0, 40);
        }
    }
}
