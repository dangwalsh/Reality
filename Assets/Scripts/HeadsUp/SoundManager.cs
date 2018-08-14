namespace HeadsUp
{
    using UnityEngine;
    using HoloToolkit.Unity;

    public class SoundManager : MonoBehaviour
    {
        public GameObject[] Emitters;

        private void Start()
        {
            foreach (var emitter in Emitters)
            {
                var audioSource = emitter.GetComponent<AudioSource>();
                if (audioSource != null)
                    SpatialSoundSettings.SetRoomSize(audioSource, SpatialSoundRoomSizes.Large);
            }
        }
    }
}
