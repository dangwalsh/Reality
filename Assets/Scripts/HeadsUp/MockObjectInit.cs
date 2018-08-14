using UnityEngine;

public class MockObjectInit: MonoBehaviour {

	void Start () {

        var menuDelegate = GetComponentInParent<HeadsUp.ModelMenuDelegate>();
        var menusManagerObject = GameObject.Find("MenusManager");
        var menusManager = menusManagerObject.GetComponent<HeadsUp.MenuManager>();
        menuDelegate.MenusManager = menusManager;
    }
}
