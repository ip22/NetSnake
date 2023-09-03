using UnityEngine;

public class SegmentParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystemRenderer _snakeParticles;
    
    private MultiplayerManager _multiplayerManager;

    private void Awake() => _multiplayerManager = MultiplayerManager.Instance;

    private void OnDestroy() {
        _snakeParticles.material = _multiplayerManager.skins.GetSnakeMaterial(_multiplayerManager.PlayerSkin());
        Instantiate(_snakeParticles.gameObject, transform.position, transform.rotation);
    }
}
