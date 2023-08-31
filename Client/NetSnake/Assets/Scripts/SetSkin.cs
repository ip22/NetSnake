using UnityEngine;

public class SetSkin : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _meshRenderers;

    public void Set(Material material) {
        foreach (var renderer in _meshRenderers) { renderer.material = material; }
    }
}