using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CurvedCanvas : MonoBehaviour
{
    public float curveAmount = 50f; // Adjust this to control the curvature

    void Start()
    {
        ApplyCurvature();
    }

    void ApplyCurvature()
    {
        foreach (Graphic graphic in GetComponentsInChildren<Graphic>())
        {
            RectTransform rt = graphic.rectTransform;
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            Vector3[] newCorners = new Vector3[4];
            for (int i = 0; i < corners.Length; i++)
            {
                Vector3 corner = corners[i];
                float curveValue = Mathf.Sin(corner.x / curveAmount) * curveAmount;
                newCorners[i] = new Vector3(corner.x, corner.y + curveValue, corner.z);
            }

            Mesh mesh = new Mesh();
            rt.GetLocalCorners(newCorners);
            mesh.vertices = newCorners;
            mesh.uv = graphic.mainTexture != null ? new Vector2[newCorners.Length] : null;
            mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };

            CanvasRenderer renderer = rt.GetComponent<CanvasRenderer>();
            renderer.SetMesh(mesh);
        }
    }
}
