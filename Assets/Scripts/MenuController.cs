using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class MenuController : MonoBehaviour
{
    public GameObject menu;

    bool isActive;
    GestureRecognizer recognizer;

	void Start ()
    {
        //recognizer = new GestureRecognizer();
        //recognizer.TappedEvent += OnTapped;
        //recognizer.StartCapturingGestures();
	}
	
	void Update ()
    {
        //menu.SetActive(isActive);
	}

    void OnTapped(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        isActive = !isActive;
    }
}
