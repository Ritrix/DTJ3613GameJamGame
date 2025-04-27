using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class backGroundMusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip loopSong;
    public string targetSceneName;
    public float fadeDuration = 2f; // How long it takes to fade in (seconds)

    private bool isFadingOut = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == targetSceneName)
        {
            if (musicSource != null && loopSong != null)
            {
                musicSource.clip = loopSong;
                musicSource.loop = true;
                musicSource.volume = 0f; // Start muted
                musicSource.Play();
                StartCoroutine(FadeInMusic());
            }
        }
    }

    IEnumerator FadeInMusic()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, 0.4f, timer / fadeDuration);
            yield return null;
        }
        musicSource.volume = 0.4f; // Ensure it's exactly 1 at the end
    }

    IEnumerator FadeOutMusic()
    {
        isFadingOut = true;
        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = 0.4f; // Reset for next time
        isFadingOut = false;
    }

    void OnSceneUnloaded(Scene current)
    {
        if (!isFadingOut && musicSource.isPlaying)
        {
            StartCoroutine(FadeOutMusic());
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded; // Unsubscribe when destroyed
    }
}
