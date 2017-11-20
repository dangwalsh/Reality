namespace HeadsUp {

    using UnityEngine;

    public class HandScale : HandTransformation {

        private void Awake() {

            this.TransformValue = HostTransform.localScale.x;
        }

        protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newDraggingPosition, Vector3 delta, bool isPerpetual = true) {
            // simply sum all of the components of the delta vector to get a unified scale factor

            this.HandValue = delta.x + delta.y + delta.z;
            var currentScale = HostTransform.localScale;

            if (isPerpetual) {

                HostTransform.localScale += this.HandValue * currentScale * Time.deltaTime;
            }
            else {

                HostTransform.localScale += this.HandValue * currentScale;
                draggingPosition = newDraggingPosition;
            }

            this.TransformValue = HostTransform.localScale.x;
        }
    }
}
