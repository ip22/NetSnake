using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField] private Transform _segmentPrefab;
    [SerializeField] private float _segmentInterval = 1f;

    private Transform _head;
    private float _snakeSpeed = 2f;

    private List<Transform> _segments = new List<Transform>();

    private List<Vector3> _positionHistory = new List<Vector3>();
    private List<Quaternion> _rotationHistory = new List<Quaternion>();

    public void Init(Transform head, float speed, int segmentsCount) {
        _head = head;
        _snakeSpeed = speed;

        _segments.Add(transform);

        _positionHistory.Add(_head.position);
        _positionHistory.Add(transform.position);

        _rotationHistory.Add(_head.rotation);
        _rotationHistory.Add(transform.rotation);

        SetSegmentsCount(segmentsCount);
    }
    private void Update() {
        var distance = (_head.position - _positionHistory[0]).magnitude;

        while (distance > _segmentInterval) {
            Vector3 direction = (_head.position - _positionHistory[0]).normalized;

            _positionHistory.Insert(0, _positionHistory[0] + direction * _segmentInterval);
            _positionHistory.RemoveAt(_positionHistory.Count - 1);


            _rotationHistory.Insert(0, _head.rotation);
            _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

            distance -= _segmentInterval;
        };

        for (int i = 0; i < _segments.Count; i++) {
            var distanceRate = distance / _segmentInterval;
            _segments[i].position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], distanceRate);
            _segments[i].rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], distanceRate);
        }
    }

    private void SetSegmentsCount(int segmentsCount) {
        if (segmentsCount == _segments.Count - 1) return;

        int diff = (_segments.Count - 1) - segmentsCount;

        if (diff < 1) {
            for (int i = 0; i < -diff; i++) {
                AddSegment();
            }
        } else {
            for (int i = 0; i < diff; i++) {
                RemoveSegment();
            }
        }
    }

    private void AddSegment() {
        Vector3 position = _segments[_segments.Count - 1].position;
        Quaternion rotation = _segments[_segments.Count - 1].rotation;
        var segment = Instantiate(_segmentPrefab, position, rotation);

        _segments.Insert(0, segment);

        _positionHistory.Add(position);
        _rotationHistory.Add(rotation);
    }

    private void RemoveSegment() {
        if (_segments.Count <= 1) {
            Debug.LogError("Can't remove. Segment is not exist.");
            return;
        }

        var segment = _segments[0];
        _segments.RemoveAt(0);
        Destroy(segment.gameObject);

        _positionHistory.RemoveAt(_positionHistory.Count - 1);
        _rotationHistory.RemoveAt(_rotationHistory.Count - 1);
    }

    public void Destroy() {
        foreach (var segment in _segments) Destroy(segment.gameObject);
    }

}
