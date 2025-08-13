using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public GameObject pauseMenuUI;
    private bool isPaused = false;

    public AudioClip clickSound;
    public AudioClip cancelSound;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause();
            else Resume();
        }
    }

    public void Pause()
    {
        if (clickSound) audioSource.PlayOneShot(clickSound);
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        if (clickSound) audioSource.PlayOneShot(clickSound);
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        if (clickSound) audioSource.PlayOneShot(clickSound);
        Application.Quit();
    }
}
