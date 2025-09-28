using UnityEngine;
using UnityEngine.InputSystem; // 1. Adicionar esta linha no topo!

public class CameraZoom : MonoBehaviour
{
    public new Camera camera;
    private float targetZoom;
    private float zoomFactor = 3f;
    private float zoomLerpSpeed = 10;

    void Start()
    {
        targetZoom = camera.orthographicSize;
    }

    void Update()
    {
        // 2. Ler o input do scroll do mouse com o novo sistema
        float scrollInput = Mouse.current.scroll.ReadValue().y;

        // 3. Normalizar o valor para que ele seja -1, 0 ou 1 (para controlar a velocidade)
        float scrollData = Mathf.Sign(scrollInput);

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 4.5f, 15f);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}