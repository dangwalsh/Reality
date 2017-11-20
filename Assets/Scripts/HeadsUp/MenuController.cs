namespace HeadsUp {

    using UnityEngine;

    public class MenuController : MonoBehaviour {

        public GameObject MainMenu;
        public GameObject RotateMenu;
        public GameObject ScaleMenu;
        public GameObject TranslateMenu;


        public void ReturnHome(GameObject pSender) {

            MainMenu.SetActive(true);
            pSender.SetActive(false);
            MainMenu.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, 2.0f));
        }

        public void ShowRotateMenu() {

            this.RotateMenu.SetActive(true);
        }

        public void ShowScaleMenu() {

            this.ScaleMenu.SetActive(true);
        }

        public void ShowPositionMenu() {

            this.TranslateMenu.SetActive(true);
        }

        private void Update() {

            SetMenuRotation(this.MainMenu);
        }

        private void SetMenuRotation(GameObject menu) {

            var pivotPosition = GetHandPivotPosition();
            var draggingRotation = Quaternion.LookRotation(menu.transform.position - pivotPosition);
            menu.transform.rotation = draggingRotation;
        }

        private Vector3 GetHandPivotPosition() {

            var pivot = Camera.main.transform.position + new Vector3(0, -0.2f, 0) - Camera.main.transform.forward * 0.2f;
            return pivot;
        }
    }
}
