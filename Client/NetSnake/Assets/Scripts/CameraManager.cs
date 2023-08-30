using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void Start() {
        var camera = Camera.main.transform;
        camera.parent = transform;
        camera.localPosition = Vector3.zero;
    }

    private void OnDestroy() {
        if (Camera.main == null) return;
        var camera = Camera.main.transform;
        camera.parent = null;
    }
}
