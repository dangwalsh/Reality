namespace HeadsUp {

    using UnityEngine;

    public class HandRotate : HandTransformation {

        float cumRotationFactor;

        private void Awake() {
            this.TransformValue = HostTransform.rotation.eulerAngles.y;
        }

        protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newDraggingPosition, Vector3 delta, bool isPerpetual = true) {
            // sum the components of the delta vector to get a unified rotation magnitude and convert to degrees
            this.HandValue = delta.x + delta.y + delta.z;
            var rotationFactor = Mathf.Rad2Deg * this.HandValue;
            
            if (isPerpetual) {
                cumRotationFactor += rotationFactor * Time.deltaTime;
                var quantized = Quantize(cumRotationFactor, QuantizeValue);
                if (quantized != 0) {
                    HostTransform.Rotate(Vector3.up, quantized);
                    cumRotationFactor = 0;
                }
            }
            else {
                cumRotationFactor += rotationFactor;
                var quantized = Quantize(cumRotationFactor, QuantizeValue);
                if (quantized != 0) {
                    HostTransform.Rotate(new Vector3(0, quantized, 0));
                    cumRotationFactor = 0;
                    draggingPosition = newDraggingPosition;
                }
            }

            this.TransformValue = HostTransform.rotation.eulerAngles.y;
        }

        private float Quantize(float val, float quant) {

            return Mathf.Round(val / quant) * quant;
        }
    }
}

