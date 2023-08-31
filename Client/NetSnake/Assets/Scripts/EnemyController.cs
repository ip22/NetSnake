using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _player;
    private Snake _snake;

    public void Init(Player player, Snake snake) {
        _player = player;
        _snake = snake;
        _player.OnChange += OnChange;
    }

    private void OnChange(List<DataChange> changes) {
        Vector3 position = _snake.transform.position;

        for (int i = 0; i < changes.Count; i++) {
            switch (changes[i].Field) {
                case "x":
                    position.x = (float)changes[i].Value;
                    break;
                case "z":
                    position.z = (float)changes[i].Value;
                    break;
                case "sg":
                    _snake.SetSegmentsCount((byte)changes[i].Value);
                    break;
                default:
                    Debug.LogWarning("Can,t read field changes:" + changes[i].Value);
                    break;
            }
        }

        _snake.SetRotation(position);
    }

    public void Destroy() {
        _player.OnChange -= OnChange;
        _snake.Destroy();
    }
}
