using UnityEngine;

public class SkinSelectButton : MonoBehaviour
{
    public void SelectSkin(int value) {
        MultiplayerManager.Instance.Connection(value);
    }
}
