using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{
    public Camera mainCamera;
    public float RendererDistance = 10f; 

    private void Update()
    {
        // �������� ��� ������� � ����������� Renderer � �����
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        // ��������� ������ ������
        foreach (Renderer renderer in renderers)
        {

            // ���������, ����� �� ������ �� �������� ������
            if ((GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), renderer.bounds)) && (Vector3.Distance(renderer.bounds.center, mainCamera.transform.position)  < RendererDistance))
            {
                // ���� ������ �������, �� �������� ��������� Renderer
                renderer.enabled = true;
            }
            else
            {
                // ���� ������ �� �������, �� ��������� ��������� Renderer
                renderer.enabled = false;
            }
        }
    }
}
