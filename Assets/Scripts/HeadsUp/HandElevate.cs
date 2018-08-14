namespace HeadsUp
{
    using UnityEngine;

    public class HandElevate : HandTransformation
    {
        private void Awake()
        {
            this.TransformValue = HostTransform.localPosition.y;
        }

        protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newPosition, Vector3 delta, bool isPerpetual = true)
        {
            this.HandValue = delta.x + delta.y + delta.z;

            this.HostTransform.localPosition += new Vector3(0, this.HandValue, 0);
        }
    }
}
