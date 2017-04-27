using System;
using UnityEngine;
using UnityEngine.UI;

public class StatsUpdater : MonoBehaviour {
    [Tooltip("The Text object that displays position data")]
    public Text PosX;
    public Text PosY;
    public Text PosZ;

    [Tooltip("The Text object that displays rotation data")]
    public Text RotX;
    public Text RotY;
    public Text RotZ;

    [Tooltip("The Text object that displays scale data")]
    public Text ScaX;
    public Text ScaY;
    public Text ScaZ;

    [Tooltip("The Transform of the model being displayed")]
    public Transform TargetTransform;

    private Vector3 rotation;
    private Vector3 position;
    private Vector3 scale;


    private void Update() {
        var pos = UpdatePosition();
        var rot = UpdateRotation();
        var sca = UpdateScale();

        UpdateColor(pos, rot, sca);

        this.position = pos;
        this.rotation = rot;
        this.scale = sca;

        UpdateText();
    }

    private void UpdateColor(Vector3 pos, Vector3 rot, Vector3 sca) {
        this.PosX.color = (pos.x == this.position.x) ? Color.white : Color.red;
        this.PosY.color = (pos.y == this.position.y) ? Color.white : Color.red;
        this.PosZ.color = (pos.z == this.position.z) ? Color.white : Color.red;

        this.RotX.color = (rot.x == this.rotation.x) ? Color.white : Color.red;
        this.RotY.color = (rot.y == this.rotation.y) ? Color.white : Color.red;
        this.RotZ.color = (rot.z == this.rotation.z) ? Color.white : Color.red;

        this.ScaX.color = (sca.x == this.scale.x)    ? Color.white : Color.red;
        this.ScaY.color = (sca.y == this.scale.y)    ? Color.white : Color.red;
        this.ScaZ.color = (sca.z == this.scale.z)    ? Color.white : Color.red;
    }

    private void UpdateText() {
        this.PosX.text = String.Format("{0:F3}", this.position.x);
        this.PosY.text = String.Format("{0:F3}", this.position.y);
        this.PosZ.text = String.Format("{0:F3}", this.position.z);

        this.RotX.text = String.Format("{0:F3}", this.rotation.x);
        this.RotY.text = String.Format("{0:F3}", this.rotation.y);
        this.RotZ.text = String.Format("{0:F3}", this.rotation.z);

        this.ScaX.text = String.Format("{0:F3}", this.scale.x);
        this.ScaY.text = String.Format("{0:F3}", this.scale.y);
        this.ScaZ.text = String.Format("{0:F3}", this.scale.z);
    }
    
    private Vector3 UpdateScale() {
        if (TargetTransform == null) return Vector3.one;
        return TargetTransform.lossyScale;
    }

    private Vector3 UpdateRotation() {
        if (TargetTransform == null) return Vector3.zero;
        return TargetTransform.rotation.eulerAngles;
    }

    private Vector3 UpdatePosition() {
        if (TargetTransform == null) return Vector3.zero;
        return TargetTransform.position;
    }
}
