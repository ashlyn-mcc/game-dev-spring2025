using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    //AudioSource effectSound;
    [SerializeField] private AudioClip explodeSoundEffect;
    [SerializeField] private AudioClip throwSoundEffect;
    
    public void explodeEffect()
    {
        GameObject explodeSoundObject = new GameObject("ExplodeSoundEffect");
        explodeSoundObject.transform.position = transform.position;
        explodeSoundObject.transform.rotation = transform.rotation;
        AudioSource explodeSound = explodeSoundObject.AddComponent<AudioSource>();
        explodeSound.clip = explodeSoundEffect;
        explodeSound.Play();
        print("explode");
        Destroy(explodeSoundObject, 1.0f);
    }

    public void throwEffect()
    {
        GameObject throwSoundObject = new GameObject("ThrowSoundEffect");
        throwSoundObject.transform.position = transform.position;
        throwSoundObject.transform.rotation = transform.rotation;
        AudioSource throwSound = throwSoundObject.AddComponent<AudioSource>();
        throwSound.clip = throwSoundEffect;
        throwSound.Play();
        print("throw");
        Destroy(throwSoundObject, 1.0f);
    }
}
