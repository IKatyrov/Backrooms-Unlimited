using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{
    public Camera mainCamera;
    public float RendererDistance = 10f; 

    private void Update()
    {
        // Получаем все объекты с компонентом Renderer в сцене
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        // Проверяем каждый объект
        foreach (Renderer renderer in renderers)
        {

            // Проверяем, видим ли объект из заданной камеры
            if ((GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), renderer.bounds)) && (Vector3.Distance(renderer.bounds.center, mainCamera.transform.position)  < RendererDistance))
            {
                // Если объект видимый, то включаем компонент Renderer
                renderer.enabled = true;
            }
            else
            {
                // Если объект не видимый, то отключаем компонент Renderer
                renderer.enabled = false;
            }
        }
    }
}
