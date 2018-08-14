namespace HeadsUp
{
    using System;
    using UnityEngine;

    [Obsolete("Use HandTranslate instead", true)]
    public class GazeTranslate : GazeTransformation
    {

        protected override void ApplyTransformation(Camera camera, float width, float height)
        {
            //HostTransform.position = camera.ScreenToWorldPoint(new Vector3(width / 2, height / 2, objDistance));
        }
    }
}