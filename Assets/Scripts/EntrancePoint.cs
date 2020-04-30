using UnityEngine;

public class EntrancePoint : MonoBehaviour
{
    private int dist = 2;
    void OnDrawGizmosSelected()
    {
        float zRot = -transform.rotation.eulerAngles.z;
        float sina = Mathf.Sin(zRot * Mathf.Deg2Rad);
        float cosa = Mathf.Cos(zRot * Mathf.Deg2Rad);
        float newX = (sina * dist) + transform.position.x;
        float newY = (cosa * dist) + transform.position.y;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(newX, newY));
    }
}
