using UnityEngine;

/// <summary>
/// Responsive Camera Scaler
/// </summary>
public class CameraAspectRatioScaler : MonoBehaviour
{

    /// <summary>
    /// Reference Resolution like 1920x1080
    /// </summary>
    public Vector2 ReferenceResolution = new Vector2(1920, 1080);

    /// <summary>
    /// Zoom factor to fit different aspect ratios
    /// </summary>
    public Vector3 ZoomFactor = Vector3.one;

    /// <summary>
    /// Design time position
    /// </summary>
    [HideInInspector]
    public Vector3 OriginPosition;

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        OriginPosition = transform.position;
    }

    /// <summary>
    /// Update per Frame
    /// </summary>
    void Update()
    {

        if (ReferenceResolution.y == 0 || ReferenceResolution.x == 0)
            return;

        var refRatio = ReferenceResolution.x / ReferenceResolution.y;
        var ratio = (float)Screen.width / (float)Screen.height;

        transform.position = OriginPosition + transform.forward * (1f - refRatio / ratio) * ZoomFactor.z
                                            + transform.right * (1f - refRatio / ratio) * ZoomFactor.x
                                            + transform.up * (1f - refRatio / ratio) * ZoomFactor.y;


    }
}
