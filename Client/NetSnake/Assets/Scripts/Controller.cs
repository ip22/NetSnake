using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Transform _cursor;
    [SerializeField] private Snake _snake;
    private Camera _camera;
    private Plane _plane;

    private void Awake() {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            MoveCursor();
            _snake.LookAt(_cursor.position);
        }
    }

    private void MoveCursor() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out float distance);
        Vector3 point = ray.GetPoint(distance);

        _cursor.position = Vector3.Lerp(_cursor.transform.position, point, Time.deltaTime * 30f);
    }
}