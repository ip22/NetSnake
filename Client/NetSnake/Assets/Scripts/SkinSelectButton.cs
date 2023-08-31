using UnityEngine;

public class SkinSelectButton : MonoBehaviour
{
    // TODO disable selected buttons from other clients menu
    public void SelectSkin(int value) {
        MultiplayerManager.Instance.Connection(value);
    }
}
