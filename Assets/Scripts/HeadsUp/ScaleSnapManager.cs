namespace HeadsUp {

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class ScaleSnapManager : SnapManager {

        protected override void SetSnapValue() {

            var menusManager = parentMenu.GetComponentInParent<MenusManager>();
            if (menusManager == null) return;
            var handController = menusManager.ThisModel.GetComponent<HandScale>();
            if (handController == null) return;

            var snapValue = Snaps[count].SnapValue;
            //handTransformation.QuantizeValue = snapValue;
            InitializeSnap(menusManager.ThisModel.transform, snapValue, handController);
        }

        protected override void InitializeSnap(Transform thisTransform, float snapValue, HandTransformation handController) {

            if (snapValue == 0) return;
            var scaleFactor = 1 / snapValue;
            thisTransform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            handController.TransformValue = scaleFactor;
        }
    }
}
