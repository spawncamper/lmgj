using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip gameWonMusic;
    [SerializeField] AudioClip mouseClickSound;
    [SerializeField] AudioClip coinTossSound;
    [SerializeField] float audioBlendTime = 0.5f;
    AudioSource mainAudioSource;

    void OnEnable()
    {
        ClickToMove.MouseClickEvent += PlayMouseClickSound;
    }

    void OnDisable()
    {
        ClickToMove.MouseClickEvent -= PlayMouseClickSound;
    }

    void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
        mainAudioSource.loop = true;

        StartCoroutine(PlayBackgroundMusicCoroutine());
    }

    public IEnumerator PlayBackgroundMusicCoroutine()
    {
        yield return new WaitForSeconds(audioBlendTime);
        mainAudioSource.clip = backgroundMusic;
        mainAudioSource.Play();
    }

    public void PlayBackgroundMusic()
    {
        StartCoroutine(PlayBackgroundMusicCoroutine());
    }

    public IEnumerator PlayAudioClip(AudioClip _audioClip)
    {
        mainAudioSource.PlayOneShot(_audioClip);
        yield return new WaitForSeconds(_audioClip.length);
    }

    public void PlayGameOverMusic()
    {
        StartCoroutine(GameOverMusicCoroutine());
    }

    public void PlayGameWonMusic()
    {
        StartCoroutine(GameWonCoroutine());
    }

    void PlayMouseClickSound()
    {
        StartCoroutine(PlayAudioClip(mouseClickSound));
    }

    IEnumerator GameOverMusicCoroutine()
    {
        mainAudioSource.loop = false;
        mainAudioSource.clip = gameOverMusic;
        mainAudioSource.Play();
        yield return new WaitForSeconds(gameOverMusic.length);
        StopMusic();
    }

    IEnumerator GameWonCoroutine()
    {
        mainAudioSource.PlayOneShot(gameWonMusic, 1f);
        yield return new WaitForSeconds(gameWonMusic.length * 3);
    }

    public void StopMusic()
    {
        mainAudioSource.Stop();
    }

    public void StartMusic()
    {
        mainAudioSource.Play();
    }
}
