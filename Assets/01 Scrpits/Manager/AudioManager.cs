using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Slider m_musicSlider;
    [SerializeField] private Slider m_SfxSlider;

    [SerializeField] private AudioSource m_backgroundMusicSource;
    [SerializeField] private AudioSource m_sfx;

    [SerializeField] private AudioClip m_shotgun;
    [SerializeField] private AudioClip m_rifles;
    [SerializeField] private AudioClip m_sword;
    [SerializeField] private AudioClip m_coin;
    [SerializeField] private AudioClip m_heart;
    [SerializeField] private AudioClip m_dash;
    [SerializeField] private AudioClip m_enemyDie;
    [SerializeField] private AudioClip m_playerHit;
    [SerializeField] private AudioClip m_crateDestruct;


    private void Start()
    {

        if (!PlayerPrefs.HasKey("SfxVolume")) PlayerPrefs.SetFloat("SfxVolume", 0.5f);

        Load();

        m_sfx.volume = m_SfxSlider.value;
    }

    public void SetMusicVolume()
    {
        m_backgroundMusicSource.volume = m_musicSlider.value;
        Save();
    }

    public void SetSFXVolume()
    {
        m_sfx.volume = m_SfxSlider.value;
        Save();
    }

    private void Load()
    {

        m_SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
    }

    private void Save()
    {

        PlayerPrefs.SetFloat("SfxVolume", m_SfxSlider.value);
        PlayerPrefs.Save();
    }
    
    public void PlayHomeMusic()
    {
        m_backgroundMusicSource.Play();
    }

    public void PlayShotgunSound()
    {
        m_sfx.PlayOneShot(m_shotgun);
    }

    public void PlayRifleSound()
    {
        m_sfx.PlayOneShot(m_rifles);
    }

    public void PlaySwordSound()
    {
        m_sfx.PlayOneShot(m_sword);
    }

    public void PlayCoinSound()
    {
        m_sfx.PlayOneShot(m_coin);
    }

    public void PlayHeartSound()
    {
        m_sfx.PlayOneShot(m_heart);
    }

    public void PlayDashSound()
    {
        m_sfx.PlayOneShot(m_dash);
    }

    public void PlayEnemyDieSound()
    {
        m_sfx.PlayOneShot(m_enemyDie);
    }

    public void PlayCrateDestructSound()
    {
        m_sfx.PlayOneShot(m_crateDestruct);
    }

    public void PlayPlayerHitSound()
    {
        m_sfx.PlayOneShot(m_playerHit);
    }

}
