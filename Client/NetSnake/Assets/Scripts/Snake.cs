using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private int _playerLayer = 6;

    public float speed { get { return _speed; } }
    [SerializeField] private float _speed = 2f;

    [field: SerializeField] public Transform head { get; private set; }
    
    [SerializeField] private Tail _tailPrefab;
    private Tail _tail;

    public void Init(int segmentsCount, bool isPlayer = false) {
        if (isPlayer) {
            gameObject.layer = _playerLayer;
            var childrens = GetComponentsInChildren<Transform>();
            foreach(var child in childrens) child.gameObject.layer = _playerLayer;
        }

        _tail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _tail.Init(head, _speed, segmentsCount, _playerLayer, isPlayer);
    }

    public void SetSegmentsCount(int segmentsCount) => _tail.SetSegmentsCount(segmentsCount);

    public void SetRotation(Vector3 pointToLook) => head.LookAt(pointToLook);

    private void Update() {
        Move();
    }

    private void Move() => transform.position += head.forward * Time.deltaTime * _speed;

    public void Destroy(string clientID) {
        var segmentsPositions = _tail.GetSegmentsPositions();
        segmentsPositions.id = clientID;
        string json = JsonUtility.ToJson(segmentsPositions);
        MultiplayerManager.Instance.SendMessage("gameOver", json);
        _tail.Destroy();
        Destroy(gameObject);
    }

    internal void SetSkin(Material skin) {
        GetComponent<SetSkin>().Set(skin);
        _tail.SetSkin(skin);
    }
}
