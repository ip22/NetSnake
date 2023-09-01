using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private Vector2 _rnd;
    private Transform _transform;

    private void Start() {
        _transform = transform;
        _rnd.x = Random.value;
        _rnd.y = Random.value;
    }

    void Update() => _transform.Rotate(_rnd.x * _speed * Time.deltaTime, _rnd.y * _speed * Time.deltaTime, 0f);
}
