using UnityEngine;

public class SegmentParticle : MonoBehaviour
{
    [SerializeField] private GameObject _snakeParticle;
    private void OnDestroy() => Instantiate(_snakeParticle, transform.position, transform.rotation);

}
