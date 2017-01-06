using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine;


public class ControlManager : MonoBehaviour, IInputClickHandler
{
    bool controlling;
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnInputClicked(InputEventData eventData)
    {
        controlling = !controlling;
        var bounding = transform.Find("Bounding").gameObject;
        bounding.SetActive(controlling);
    }

}

