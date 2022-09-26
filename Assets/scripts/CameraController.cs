using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _Xform_Camera;
    private Transform _Xform_Parent;

    private Vector3 _LocalRotation;
    private float _CameraDistance = 10f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitiviy = 2f;
    public float OrbitDampering = 10f;
    public float ScrollDampering = 6f;

    public bool CameraDisable = false;

    void Start()
    {
        this._Xform_Camera = this.transform;
        this._Xform_Parent = this.transform.parent;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
            CameraDisable = !CameraDisable;

        if (!CameraDisable)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY= Input.GetAxis("Mouse Y");

            if (mouseX != 0 || mouseY != 0)
            {
                _LocalRotation.x += mouseX * MouseSensitivity;
                _LocalRotation.y += mouseY * MouseSensitivity;

                _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitiviy;

            scrollAmount *= (this._CameraDistance * 0.3f);

            this._CameraDistance += scrollAmount * -1f;

            this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);
        }

        Quaternion qt = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._Xform_Parent.rotation = Quaternion.Lerp(this._Xform_Parent.rotation, qt, Time.deltaTime * OrbitDampering);

        if(this._Xform_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._Xform_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._Xform_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampering));
        }
    }
}
