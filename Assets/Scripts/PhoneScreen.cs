using UnityEngine;

public class PhoneScreen : MonoBehaviour
{
    public Camera playerCamera;   // assign in Inspector, make angle relative to player view
    private readonly float _maxAngle = 7f;  // tolerance (CHANGEABLE)
    // Note 10f leaves a bit of room, 7f needs some adjusting but not too much
    private bool _faceIDDone;
    private PhoneUIController _phoneUI;

    private void Start()
    {
        _phoneUI = GetComponent<PhoneUIController>();
        _phoneUI.ShowFaceID();
    }

    private void Update()
    {
        if (_faceIDDone) return;

        // Phone's top face (screen) points along transform.up
        Vector3 toCamera = (playerCamera.transform.position - transform.position).normalized;
        float screenAngle = Vector3.Angle(transform.up, toCamera);

        if (screenAngle < _maxAngle)
        {
            _faceIDDone = true;
            _phoneUI.ShowHome();
            Debug.Log("FaceID success! Phone screen facing camera.");
        }
    }

}
