using UnityEngine;

public class DrawDebugOutline : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = new Color(0.75f, 0.0f, 0.0f, 0.75f);
    [SerializeField] private float cubeSize = 0.5f;
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one * cubeSize);
    }

}
