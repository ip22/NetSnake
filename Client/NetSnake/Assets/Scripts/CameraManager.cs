using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _target;
    private Camera _camera;
    private Transform _cameraTransform;
    private Vector3 _cameraRotationOffSet;
    private float _rate = 5f;
    private float _hight;

    private void Awake() {
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
    }

    public void Init(float hight, Vector3 cameraRotationOffSet, Transform target, float lerpRate) {
        _target = target;
        _hight = hight;
        _cameraRotationOffSet = cameraRotationOffSet;
        _rate = lerpRate;
    }

    private void Update() {
        _camera.orthographicSize = _hight;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _target.position, Time.deltaTime * _rate);
        _cameraTransform.localEulerAngles = _cameraRotationOffSet;

        //var lerpY = Mathf.Lerp(_camera.localEulerAngles.y, _target.localEulerAngles.y, Time.deltaTime * _rate);
        //_cameraTransform.localEulerAngles = new Vector3(60f, lerpY, 0f);
    }
}
