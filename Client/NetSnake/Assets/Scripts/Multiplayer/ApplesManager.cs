using System.Collections.Generic;
using UnityEngine;

public class ApplesManager : MonoBehaviour
{
    [SerializeField] private Apple _applePrefab;
    private Dictionary<Vector2float, Apple> _apples = new Dictionary<Vector2float, Apple>();

    public void CreateApple(Vector2float vector2Float) {
        Vector3 position = new Vector3(vector2Float.x, 0, vector2Float.z);
        var apple = Instantiate(_applePrefab, position, Quaternion.identity);
        apple.Init(vector2Float);
        _apples.Add(vector2Float, apple);
    }

    public void RemoveApple(int key, Vector2float vector2Float) {
        if (_apples.ContainsKey(vector2Float) == false) return;

        var apple = _apples[vector2Float];
        _apples.Remove(vector2Float);
        apple.Destroy();
    }
}