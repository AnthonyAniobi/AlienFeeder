using UnityEngine;

public class DebugExtension
{
    public static void DrawSphereCast(Vector3 origin, float radius, Vector3 direction, float distance, Color color)
    {
        Vector3 normalizeDir = direction.normalized;
        Vector3 endPoint = origin + normalizeDir * distance;

        // Draw the start and end center points
        Debug.DrawLine(origin, endPoint, color);

        // Find orthogonal vectors to draw the capsule body outer lines
        Vector3 orthoX = Vector3.Cross(normalizeDir, Vector3.up).normalized * radius;
        if (orthoX == Vector3.zero) 
            orthoX = Vector3.Cross(normalizeDir, Vector3.right).normalized * radius;
            
        Vector3 orthoY = Vector3.Cross(normalizeDir, orthoX).normalized * radius;

        // Draw 4 side connecting lines
        Debug.DrawLine(origin + orthoX, endPoint + orthoX, color);
        Debug.DrawLine(origin - orthoX, endPoint - orthoX, color);
        Debug.DrawLine(origin + orthoY, endPoint + orthoY, color);
        Debug.DrawLine(origin - orthoY, endPoint - orthoY, color);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0f)
    {
        Debug.DrawLine(start, end, color, duration);
    }
}
