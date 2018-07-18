namespace HeadsUp
{

    using UnityEngine;
    using HoloToolkit.Unity.InputModule;

    public class SceneManager : MonoBehaviour, IInputHandler
    {
        public void OnInputDown(InputEventData eventData)
        {
            Debug.Log("Input Down");
        }

        public void OnInputUp(InputEventData eventData)
        {
            Debug.Log("Input Down");
        }

        //[Tooltip("The Prefab that gets instantiated to hold a new model.")]
        //public GameObject ModelManagerPrefab;
        //private string args = "";

        #region MonoBehaviour Members
        void Start()
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
        }

        void Update()
        {
            // TODO: Refactor this or remove it
            //var arguments = UnityEngine.WSA.Application.arguments;
            //if (arguments == args || arguments == "") return;
            //args = UnityEngine.WSA.Application.arguments;
            //Instantiate(ModelManagerPrefab, Vector3.zero, Quaternion.identity, transform);
        }
        #endregion
    }
}