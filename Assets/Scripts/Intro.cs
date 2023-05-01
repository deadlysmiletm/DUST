using UnityEngine;

public class Intro : MonoBehaviour {

    public GameObject[] gameObjectoToActive;
    public PlayerBrain pb;
    public Animator intro;
    public AudioSource musicAudioSource;

    public void End()
    {
        foreach (var item in gameObjectoToActive)
        {
            item.SetActive(true);
        }
        pb.enabled = true;
        this.gameObject.SetActive(false);
    }

    public void StartMusic()
    {
        musicAudioSource.Play();
    }
}
