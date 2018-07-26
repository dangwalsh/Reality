﻿namespace HeadsUp
{
    using UnityEngine;

    public class MenusManager : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject RotateMenu;
        public GameObject ScaleMenu;
        public GameObject TranslateMenu;
        public GameObject ThisModel;
        public GameObject ModelManagerPrefab;

        public void ReturnHome(GameObject pSender)
        {
            //MainMenu.SetActive(true);
            pSender.SetActive(false);
            //MainMenu.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, 1.0f));
        }

        public void ShowRotateMenu()
        {

            this.RotateMenu.SetActive(true);
        }

        public void ShowScaleMenu()
        {

            this.ScaleMenu.SetActive(true);
        }

        public void ShowPositionMenu()
        {

            this.TranslateMenu.SetActive(true);
        }

        public void RemoveThisModel()
        {

            Destroy(ThisModel);
        }

        public void OpenNewFile()
        {
            Instantiate(ModelManagerPrefab, Vector3.zero, Quaternion.identity, transform);
        }

        #region MonoBehaviour Members
        private void Update()
        {
            SetMenuRotation(this.MainMenu);
        }
        #endregion

        private void SetMenuRotation(GameObject menu)
        {
            var pivotPosition = GetHandPivotPosition();
            var draggingRotation = Quaternion.LookRotation(menu.transform.position - pivotPosition);
            menu.transform.rotation = draggingRotation;
        }

        private Vector3 GetHandPivotPosition()
        {
            var pivot = Camera.main.transform.position + new Vector3(0, -0.2f, 0) - Camera.main.transform.forward * 0.2f;
            return pivot;
        }
    }
}

