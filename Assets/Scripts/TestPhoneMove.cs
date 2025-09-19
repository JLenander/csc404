using UnityEngine;

public class TestPhoneMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 50f;

    void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Rotate"); // map to keys or controller axis

        transform.position += new Vector3(h, v, 0) * moveSpeed;
        transform.Rotate(Vector3.right, r * rotateSpeed);
    }

}
