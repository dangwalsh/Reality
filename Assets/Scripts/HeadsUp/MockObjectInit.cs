using UnityEngine;

public class MockObjectInit: MonoBehaviour {

	void Start () {

        var menuDelegate = GetComponentInParent<HeadsUp.MenuDelegate>();
        var menusManagerObject = GameObject.Find("MenusManager");
        var menusManager = menusManagerObject.GetComponent<HeadsUp.MenusManager>();
        menuDelegate.MenusManager = menusManager;
    }
}
