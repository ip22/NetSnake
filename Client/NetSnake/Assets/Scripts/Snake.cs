using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Transform _head;

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rotateSpeed = 90f;

    private void Update() {
        Move();
        Rotate();
    }

    private void Rotate() {
        Quaternion targetRotaion = Quaternion.LookRotation(_targeDirection);
        _head.rotation = Quaternion.RotateTowards(_head.rotation, targetRotaion, Time.deltaTime * _rotateSpeed);
    }

    private void Move() {
        transform.position += _head.forward * Time.deltaTime * _speed;
    }

    private Vector3 _targeDirection = Vector3.zero;

    public void LookAt(Vector3 cursorPosition) {
        _targeDirection = cursorPosition - _head.position;
    }
}
