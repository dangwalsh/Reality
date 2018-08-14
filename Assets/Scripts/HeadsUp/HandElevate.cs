namespace HeadsUp
{
    using System.Collections;
    using UnityEngine;

    public class HandElevate : HandBase
    {
        public float maxOffset =  1;
        public float minOffset = -1;

        float totalOffset;

        private void Awake()
        {
            this.TransformValue = HostTransform.localPosition.y;
        }

        protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newPosition, Vector3 delta, bool isPerpetual = true)
        { 
            if (IsBeingPlaced)
            {
                var offset = (delta.x + delta.y + delta.z) * 0.1f;
                var nextOffset = totalOffset + offset;
                if (nextOffset < maxOffset && nextOffset > minOffset)
                {
                    foreach (Transform t in transform)
                        t.localPosition += new Vector3(0, offset, 0);
                    totalOffset = nextOffset;
                }
            }
        }
    }
}
