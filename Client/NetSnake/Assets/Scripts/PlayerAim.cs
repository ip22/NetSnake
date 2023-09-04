using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private float _radius = .5f;
    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private Transform _snakeHead;
    private Vector3 _targetDirection = Vector3.zero;
    private float _speed;
    //private Transform _transform;
    public void Init(Transform snakeHead, float speed) {
        //_transform = transform;
        _speed = speed;
        _snakeHead = snakeHead;
    }

    public void Update() {
        Rotate();
        Move();
        CheckOut();
    }

    private void FixedUpdate() {
        CheckCollision();
    }

    private void CheckCollision() {
        Collider[] colliders = Physics.OverlapSphere(_snakeHead.position, _radius, _collisionLayer);

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].TryGetComponent<Apple>(out Apple apple)) {
                apple.Collect();
            } else {
                if (colliders[i].GetComponentInParent<Snake>()) {
                    var enemy = colliders[i].transform;
                    float playerAngle = Vector3.Angle(enemy.position - _snakeHead.position, transform.forward);
                    float enemyAngle = Vector3.Angle(_snakeHead.position - enemy.position, enemy.forward);

                    if (playerAngle < enemyAngle + 5f) {
                        GameOver();
                    }
                } else {
                    GameOver();
                }
            }
        }
    }

    private void GameOver() {
        FindObjectOfType<Controller>().Destroy();
        Destroy(gameObject);
        MultiplayerManager.Instance.ViewMenu();
    }

    private void Rotate() {
        Quaternion targetRotaion = Quaternion.LookRotation(_targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotaion, Time.deltaTime * _rotateSpeed);
    }

    private void Move() => transform.position += transform.forward * _speed * Time.deltaTime;

    private void CheckOut() {
        if (Math.Abs(_snakeHead.position.x) > 128 || Math.Abs(_snakeHead.position.z) > 128) GameOver();
    }

    public void SetTargetDirection(Vector3 pointToLook) => _targetDirection = pointToLook - transform.position;

    public void GetMoveInfo(out Vector3 position) => position = transform.position;
}
