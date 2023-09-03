using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum AppleType { Red = 1, Green = 2, Bad = 3 }

public class Apple : MonoBehaviour
{
    [SerializeField] private sbyte _points = 1;
    [SerializeField] private Renderer _renderer;

    private AppleType _type;
    private Vector2float _apple;

    public void Init(Vector2float apple) {
        _apple = apple;
        _type = (AppleType)apple.type;

        switch (_type) {
            case AppleType.Red: Setup(AppleType.Red, 1); break;
            case AppleType.Green: Setup(AppleType.Green, 2); break;
            case AppleType.Bad: Setup(AppleType.Bad, -2); break;
            default: break;
        }

        _apple.OnChange += OnChange;
    }

    private void Setup(AppleType type, sbyte points) {
        _points = points;
        _renderer.material = MultiplayerManager.Instance.skins.GetAppleMaterial(Array.IndexOf(Enum.GetValues(type.GetType()), type));
    }

    private void OnChange(List<DataChange> changes) {
        Vector3 position = transform.position;

        foreach (var change in changes) {
            switch (change.Field) {
                case "x":
                    position.x = (float)change.Value;
                    break;
                case "z":
                    position.z = (float)change.Value;
                    break;
                default:
                    Debug.LogWarning("Apple don't react on changes of fields: " + change.Field);
                    break;
            }
        }

        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Collect() {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "id", _apple.id },
            { "pts", _points }
        };

        MultiplayerManager.Instance.SendMessage("collect", data);

        gameObject.SetActive(false);
    }

    public void Destroy() {
        if (_apple != null) _apple.OnChange -= OnChange;
        Destroy(gameObject);
    }
}
