namespace HeadsUp {

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;
    using System;
    using System.Collections;

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
        protected float snapValue;
        protected MenusManager menusManager;
        protected HandTransformation handController;
        protected Vector3 startVector;
        protected Vector3 endVector;

        void OnEnable() {

            length = Snaps.Length;

            parentMenu = transform.parent.gameObject;
            if (parentMenu == null) return;
            menusManager = parentMenu.GetComponentInParent<MenusManager>();
            if (menusManager == null) return;
            handController = menusManager.ThisModel.GetComponent<HandTransformation>();
            if (menusManager == null) return;
            
            foreach (var snap in Snaps)
                snap.SnapLabel.SetActive(false);

            Snaps[count].SnapLabel.SetActive(true);
        }

        public void OnInputClicked(InputEventData eventData) {

            Snaps[count].SnapLabel.SetActive(false);
            count++;
            if (count >= length) count = 0;
            Snaps[count].SnapLabel.SetActive(true);
            snapValue = Snaps[count].SnapValue;
            if (snapValue == 0) return;
            
            SetSnapVector();
            StartCoroutine(LerpToSnap(menusManager.ThisModel.transform, 0.5f));
        }

        protected abstract void SetSnapVector();
        protected abstract IEnumerator LerpToSnap(Transform thisModel, float duration);
    }
}
