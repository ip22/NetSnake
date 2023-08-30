using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Transform _cursor;

    private Snake _snake;
    private Camera _camera;
    private Plane _plane;

    private MultiplayerManager _multiplayerManager;

    public void Init(Snake snake) {
        _multiplayerManager = MultiplayerManager.Instance;
        _snake = snake;
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            MoveCursor();
            _snake.LookAt(_cursor.position);
            SendMove();
        }
    }

    private void SendMove() {
        _snake.GetMoveInfo(out Vector3 position);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"x", position.x},
            {"z", position.z}
        };

        _multiplayerManager.SendMessage("move", data);
    }

    private void MoveCursor() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out float distance);
        Vector3 point = ray.GetPoint(distance);

        _cursor.position = Vector3.Lerp(_cursor.transform.position, point, Time.deltaTime * 30f);
    }
}
