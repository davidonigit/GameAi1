using UnityEngine;

public class cameraZoom : MonoBehaviour
{

    public new Camera camera;
    private float targetZoom;
    private float zoomFactor = 3f;
    private float zoomLerpSpeed = 10;

    // Start is called before the first frame update
    void Start(){

        targetZoom = camera.orthographicSize;
        
    }

    // Update is called once per frame
    void Update(){

        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        //Debug.log(scrollData);

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 4.5f, 15f);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
