using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    void update()
    {
        transform.position = _player.transform.position;
    }
}
