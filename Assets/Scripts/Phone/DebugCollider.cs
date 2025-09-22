using UnityEngine;

public class DebugCollider : MonoBehaviour
{
    public Color gizmoColor = new Color(1, 0, 0, 0.25f); // semi-transparent red

    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Gizmos.color = gizmoColor;

        if (col is BoxCollider box)
        {
            // Match local offset & size of BoxCollider
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);
        }
        else if (col is SphereCollider sphere)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(sphere.center, sphere.radius);
        }
        else if (col is CapsuleCollider capsule)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(capsule.center, capsule.radius);
        }

        // Reset matrix to avoid affecting other gizmos
        Gizmos.matrix = Matrix4x4.identity;
    }
}
