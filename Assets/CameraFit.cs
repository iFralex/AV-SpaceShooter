using UnityEngine;
using UnityEngine.UI;

public class CameraFit : MonoBehaviour
{
    public Vector2 dimensioni;
    public CanvasScaler[] canvas;

    private void Awake() => Camera.main.rect = new Rect(0, 0, 1, 1);

    void Start()
    {
        float newAspectRatio = dimensioni.x / dimensioni.y;
        float variance = newAspectRatio / Camera.main.aspect;
        if (Camera.main.aspect > newAspectRatio)
            for (int i = 0; i < canvas.Length; i++)
                canvas[i].matchWidthOrHeight = 1;
        if (variance < 1.0f)
            Camera.main.rect = new Rect((1.0f - variance) / 2.0f, 0, variance, 1.0f);
        else
        {
            variance = 1.0f / variance;
            Camera.main.rect = new Rect(0, (1.0f - variance) / 2.0f, 1.0f, variance);
        }
    }
}