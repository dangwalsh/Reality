namespace HeadsUp {

    using UnityEngine;
    using System.Collections;

    public class MainMenuManager : MonoBehaviour {

        public AudioSource PlayOnEnable;
        public AudioSource PlayOnDisable;
        public Camera SceneCamera;
        private Quaternion draggingRotation;

        void OnEnable() {

            SetPosition();
            if (PlayOnEnable == null) return;
            PlayOnEnable.Play();
        }

        private void SetPosition() {

            var pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, 2.0f));
            gameObject.transform.position = pos;
        }

        void OnDisable() {

            if (PlayOnDisable == null) return;
            PlayOnDisable.Play();
        }

        public void DismissMenu() {

            if (!gameObject.activeSelf) return;

            StartCoroutine(DisableObject());

            foreach (Transform lChild in transform) {
                var lAnimator = lChild.GetComponent<Animator>();
                lAnimator.Play("End");
            }
        }

        private IEnumerator DisableObject() {

            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }

        private void Update() {
            SetRotation(gameObject);
        }

        private void SetRotation(GameObject menu) {

            var pivotPosition = GetHandPivotPosition();
            draggingRotation = Quaternion.LookRotation(menu.transform.position - pivotPosition);
        }

        private Vector3 GetHandPivotPosition() {

            var pivot = Camera.main.transform.position + new Vector3(0, -0.2f, 0) - Camera.main.transform.forward * 0.2f;
            return pivot;
        }
    }
}