using UnityEngine;

namespace Assets.Scripts
{
    public class CamerController : MonoBehaviour
    {
        [SerializeField] private float smoothValue = 0.3f;
        private GameObject _player;

        private void Start()
        {
            _player = FindObjectOfType<Player>().gameObject;
        }

        private void Update()
        {
            MoveCamera();
        }
        private void MoveCamera()
        {
            var tempVector = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z - 10);
            tempVector = Vector3.Lerp(transform.position, tempVector, smoothValue);
            transform.position = tempVector;
        }
    }
}