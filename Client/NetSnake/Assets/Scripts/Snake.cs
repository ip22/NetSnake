using UnityEngine;

public class Snake : MonoBehaviour
{
    public float speed { get { return _speed; } }

    [SerializeField] private Transform _head;
    [SerializeField] private Tail _tailPrefab;

    [SerializeField] private float _speed = 2f;

    private Tail _tail;

    public void Init(int segmentsCount) {
        _tail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _tail.Init(_head, _speed, segmentsCount);
    }

    public void SetSegmentsCount(int segmentsCount) => _tail.SetSegmentsCount(segmentsCount);

    public void SetRotation(Vector3 pointToLook) => _head.LookAt(pointToLook);

    private void Update() {
        Move();
    }

    private void Move() => transform.position += _head.forward * Time.deltaTime * _speed;

    public void Destroy() {
        _tail.Destroy();
        Destroy(gameObject);
    }

    internal void SetSkin(Material skin) {
        GetComponent<SetSkin>().Set(skin);
        _tail.SetSkin(skin);
    }
}
