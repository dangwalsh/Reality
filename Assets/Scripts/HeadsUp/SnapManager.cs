namespace HeadsUp {

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;
    using System;

    public abstract class SnapManager : MonoBehaviour, IInputClickHandler {

        [Serializable]
        public struct Snap {

            public GameObject SnapLabel;
            public float SnapValue;
        }

        [SerializeField]
        public Snap[] Snaps;

        protected GameObject parentMenu;
        protected int count;
        protected int length;

        void OnEnable() {

            count = 0;
            length = Snaps.Length;
            parentMenu = transform.parent.gameObject;
            foreach (var snap in Snaps)
                snap.SnapLabel.SetActive(false);
            Snaps[count].SnapLabel.SetActive(true);
            SetSnapValue();
        }

        public void OnInputClicked(InputEventData eventData) {

            Snaps[count].SnapLabel.SetActive(false);
            if (++count >= length) count = 0;
            Snaps[count].SnapLabel.SetActive(true);
            SetSnapValue();
        }

        protected abstract void SetSnapValue();
        protected abstract void InitializeSnap(Transform thisTransform, float snapValue, HandTransformation handController);
    }
}
