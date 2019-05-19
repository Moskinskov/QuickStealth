using UnityEngine;

namespace Assets.Scripts
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _mainAudioSource;

        private void Update()
        {
            if (!_mainAudioSource.isPlaying)
                _mainAudioSource.Play();
        }

    }
}