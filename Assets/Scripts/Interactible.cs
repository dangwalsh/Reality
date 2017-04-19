using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource), typeof(Collider))]
public class Interactible : MonoBehaviour, IFocusable
{
    [Tooltip("The Unity ID of the material property you would like to affect")]
    public string PropertyName;


    [Tooltip("The interacted value of the property")]
    public Color InteractColor;

    private Material[] defaultMaterials;
    private Color[] defaultColors;


    public void OnFocusEnter()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
            defaultMaterials[i].SetColor(this.PropertyName, this.InteractColor); 
    }

    public void OnFocusExit()
    {
        for (int i = 0; i < defaultMaterials.Length; i++)
            defaultMaterials[i].SetColor(this.PropertyName, this.defaultColors[i]); 
    }

    private void Start()
    {
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            defaultMaterials = rend.materials;
            SetColors();
            return;
        }

        var text = GetComponent<Text>();
        if (text != null)
        {
            defaultMaterials = new Material[] { text.material };
            SetColors();
            return;
        }
    }

    private void SetColors()
    {
        int length = defaultMaterials.Length;
        defaultColors = new Color[length];
        for (int i = 0; i < length; i++)
            defaultColors[i] = defaultMaterials[i].GetColor(this.PropertyName);
    }
}
