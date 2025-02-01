using UnityEngine;

    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;
        
        [SerializeField] private AudioSource musicObject;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        
        public void PlayRandomMusic(AudioClip[] audioClip, Transform spawnTransform, float volume)
        {
            int rand = Random.Range(0, audioClip.Length);
            AudioSource audioSource = Instantiate(musicObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip[rand];
            audioSource.volume = volume;
            audioSource.Play();
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }   
    }