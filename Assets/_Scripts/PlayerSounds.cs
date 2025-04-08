using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip AudioClipPowerUp;
    public AudioClip AudioClipHitMonster;
    public AudioClip AudioClipDie;
    public AudioClip AudioClipJump;
    public AudioClip AudioClipAttack;

    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    protected void PlayClip(AudioClip clip)
    {
        AudioSource.Stop();
        AudioSource.clip = clip;
        AudioSource.Play();
    }

    public void PlayCollectPowerUp()
    {
        PlayClip(AudioClipPowerUp);
    }

    public void HitByEnemy()
    {
        PlayClip(AudioClipHitMonster);
    }

    public void Dead()
    {
        PlayClip(AudioClipDie);
    }

    public void Attack()
    {
        PlayClip(AudioClipAttack);
    }

    public void Jump()
    {
        PlayClip(AudioClipJump);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
