namespace HeadsUp {

    using UnityEngine;

    public class RotateSnapManager : SnapManager {

        protected override void SetSnapValue() {

            var menusManager = parentMenu.GetComponentInParent<MenusManager>();
            if (menusManager == null) return;
            var handController = menusManager.ThisModel.GetComponent<HandRotate>();
            if (handController == null) return;

            var snapValue = Snaps[count].SnapValue;
            handController.QuantizeValue = snapValue;
            InitializeSnap(menusManager.ThisModel.transform, snapValue, handController);
        }

        protected override void InitializeSnap(Transform thisTransform, float snapValue, HandTransformation handController) {

            var initVal = thisTransform.rotation.eulerAngles.y;
            var snapVal = Mathf.Round(initVal / snapValue) * snapValue;
            var snapQuat = Quaternion.Euler(0, snapVal, 0);

            thisTransform.rotation = snapQuat;
            handController.TransformValue = snapQuat.eulerAngles.y;
        }
    }
}
