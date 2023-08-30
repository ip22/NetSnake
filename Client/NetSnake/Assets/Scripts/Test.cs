using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private int _segmentsCount;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    private Controller _controller;
    private Snake _snake;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (_snake) _snake.Destroy();
            if (_controller) Destroy(_controller.gameObject);

            _snake = Instantiate(_snakePrefab);
            _snake.Init(_segmentsCount);

            _controller = Instantiate(_controllerPrefab);
            _controller.Init(_snake);
        }
    }
}
