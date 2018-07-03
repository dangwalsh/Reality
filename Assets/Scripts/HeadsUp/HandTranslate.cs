namespace HeadsUp {

    using UnityEngine;

    public class HandTranslate : HandTransformation {

        protected override void ApplyTransformation(Vector3 pivotPosition, Vector3 newPosition, Vector3 delta, bool isPerpetual = true) {

            // If the user is in placing mode,
            // update the placement to match the user's gaze.
            if (IsBeingPlaced) {
                // Do a raycast into the world that will only hit the Spatial Mapping mesh.
                Vector3 headPosition = Camera.main.transform.position;
                Vector3 gazeDirection = Camera.main.transform.forward;

                RaycastHit hitInfo;
                if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, 30.0f, modelAnchorManager.SpatialMappingManager.LayerMask)) {
                    // Rotate this object to face the user.
                    Quaternion toQuat = Camera.main.transform.localRotation;
                    toQuat.x = 0;
                    toQuat.z = 0;

                    // Move this object to where the raycast
                    // hit the Spatial Mapping mesh.
                    // Here is where you might consider adding intelligence
                    // to how the object is placed.  For example, consider
                    // placing based on the bottom of the object's
                    // collider so it sits properly on surfaces.
                    //if (PlaceParentOnTap) {
                    //    // Place the parent object as well but keep the focus on the current game object
                    //    Vector3 currentMovement = hitInfo.point - gameObject.transform.position;
                    //    ParentGameObjectToPlace.transform.position += currentMovement;
                    //    ParentGameObjectToPlace.transform.rotation = toQuat;
                    //}
                    //else {
                        gameObject.transform.position = hitInfo.point;
                        gameObject.transform.rotation = toQuat;
                    //}
                }
            }
        }

    }
}
