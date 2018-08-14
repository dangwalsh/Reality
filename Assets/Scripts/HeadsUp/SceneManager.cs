namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class SceneManager : MonoBehaviour, IInputClickHandler
    {
        public MenuManager MenusManager
        {
            get { return menusManager; }
            set
            {
                menusManager = value;
                mainMenuManager = menusManager.MainMenu.GetComponent<MenuManagerMain>();
            }
        }

        public Vector3 HitPoint
        {
            get { return hitPoint; }
            set { hitPoint = value; }
        }

        MenuManager menusManager;
        MenuManagerMain mainMenuManager;
        Vector3 hitPoint;

        #region MonoBehaviour Members
        void Awake()
        {
            MenusManager = GameObject.Find("MenusManager").GetComponent<MenuManager>();
        }

        void OnEnable()
        {
            InputManager.Instance.PushFallbackInputHandler(gameObject);
        }

        void OnDisable()
        {
            InputManager.Instance.PopFallbackInputHandler();
        }
        #endregion

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (eventData.used) return;

            Vector3 headPosition = Camera.main.transform.position;
            Vector3 gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, 30.0f))//, modelAnchorManager.SpatialMappingManager.LayerMask))
            {
                HitPoint = hitInfo.point;
            }

            if (mainMenuManager.isActiveAndEnabled)
                mainMenuManager.DismissMenu();
            else
                mainMenuManager.ShowMenu();

            eventData.Use();
        }

        #region Open from OneDrive
        //[Tooltip("The Prefab that gets instantiated to hold a new model.")]
        //public GameObject ModelManagerPrefab;
        //private string args = "";

        //void Update()
        //{
        //   var arguments = UnityEngine.WSA.Application.arguments;
        //    if (arguments == args || arguments == "") return;
        //    args = UnityEngine.WSA.Application.arguments;
        //    Instantiate(ModelManagerPrefab, Vector3.zero, Quaternion.identity, transform);
        //}
        #endregion
    }
}