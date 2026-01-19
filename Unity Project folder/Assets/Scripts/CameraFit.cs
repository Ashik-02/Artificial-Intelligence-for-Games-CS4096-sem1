using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [Header("Assign your map SpriteRenderer here")]
    public SpriteRenderer mapSprite;

    [Header("Optional padding in world units")]
    public float padding = 0f;

    void Start()
    {
        if (mapSprite == null)
        {
            Debug.LogError("CameraFit: Map SpriteRenderer not assigned!");
            return;
        }
        float mapWidth = mapSprite.bounds.size.x;
        float mapHeight = mapSprite.bounds.size.y;
        Vector3 mapCenter = mapSprite.bounds.center;
        transform.position = new Vector3(mapCenter.x, mapCenter.y, transform.position.z);
        Camera cam = GetComponent<Camera>();
        float screenRatio = (float)Screen.width / Screen.height;
        float mapRatio = mapWidth / mapHeight;

        if (screenRatio >= mapRatio)
        {
            cam.orthographicSize = (mapHeight / 2f) + padding;
        }
        else
        {
            cam.orthographicSize = (mapWidth / screenRatio / 2f) + padding;
        }
    }
}
