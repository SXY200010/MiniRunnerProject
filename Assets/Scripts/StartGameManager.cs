using UnityEngine;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    public static StartGameManager instance;

    public GameObject mainMenuPanel;
    public GameObject gameUIPanel;
    public Transform cameraTransform;
    public Transform cameraStartPosition;
    public Transform cameraGameplayPosition;
    public float cameraTransitionTime = 2f;

    public LaneMovementInput playerControl; 
    private bool gameStarted = false;

    public GameObject settingsPanel;

    public AudioClip clickSound;
    public AudioClip cancelSound;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null) instance = this;

        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        playerControl.enabled = false;
        mainMenuPanel.SetActive(true);
        gameUIPanel.SetActive(false);

        cameraTransform.position = cameraStartPosition.position;
        cameraTransform.rotation = cameraStartPosition.rotation;
    }

    public void OnStartButtonClicked()
    {
        if (clickSound) audioSource.PlayOneShot(clickSound);

        if (!gameStarted)
        {
            gameStarted = true;
            mainMenuPanel.SetActive(false);
            StartCoroutine(StartGameSequence());
        }
    }

    private System.Collections.IEnumerator StartGameSequence()
    {
        float elapsed = 0f;
        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;
        Vector3 targetPos = cameraGameplayPosition.position;
        Quaternion targetRot = cameraGameplayPosition.rotation;

        while (elapsed < cameraTransitionTime)
        {
            cameraTransform.position = Vector3.Lerp(startPos, targetPos, elapsed / cameraTransitionTime);
            cameraTransform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / cameraTransitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = targetPos;
        cameraTransform.rotation = targetRot;

        gameUIPanel.SetActive(true);
        playerControl.enabled = true;
    }

    public void OnSettingsButtonClicked()
    {
        if (clickSound) audioSource.PlayOneShot(clickSound);
        settingsPanel.SetActive(true);
    }

    public void OnCloseSettingsButtonClicked()
    {
        if (cancelSound) audioSource.PlayOneShot(cancelSound);
        settingsPanel.SetActive(false);
    }
}
