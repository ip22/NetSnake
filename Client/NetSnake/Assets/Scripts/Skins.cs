using System.Collections;
using UnityEngine;

public class Skins : MonoBehaviour
{
    [SerializeField] private Material[] _sankeMaterials;
    [SerializeField] private Material[] _appleMaterials;
    public int length { get { return _sankeMaterials.Length; } }

    public Material GetSnakeMaterial(int index) {
        if (_sankeMaterials.Length <= index) return _sankeMaterials[0];
        return _sankeMaterials[index];
    }

    public Material GetAppleMaterial(int index) {
        return _appleMaterials[index];
    }
}
