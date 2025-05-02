using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // BGM 관련 변수
    public AudioSource bgmSource;
    public AudioClip defaultBGM;         // 기본 BGM
    public AudioClip gameSceneBGM;       // GameScene 전용 BGM
    public AudioClip exampleSceneBGM;    // Example 씬 전용 BGM
    public AudioClip quizChallengeBGM;   // QuizChallenge 씬 전용 BGM

    // 효과음 관련 변수
    public AudioSource sfxSource;
    public AudioClip startButtonSFX;

    public Button startButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 최초 실행 시 기본 BGM 재생
        if (!bgmSource.isPlaying && defaultBGM != null)
        {
            bgmSource.clip = defaultBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (startButton != null)
            startButton.onClick.AddListener(PlayAudioOnStartButton);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (startButton != null)
            startButton.onClick.RemoveListener(PlayAudioOnStartButton);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndBindStartButton();

        // 1) QuizChallenge 씬 전용 BGM
        if (scene.name == "QuizChallenge" && quizChallengeBGM != null)
        {
            if (bgmSource.clip != quizChallengeBGM)
            {
                bgmSource.clip = quizChallengeBGM;
                bgmSource.Play();
            }
            return;
        }

        // 2) GameScene 전용 BGM
        if (scene.name == "GameScene" && gameSceneBGM != null)
        {
            if (bgmSource.clip != gameSceneBGM)
            {
                bgmSource.clip = gameSceneBGM;
                bgmSource.Play();
            }
            return;
        }

        // 3) Example 씬 전용 BGM
        if (scene.name == "Example" && exampleSceneBGM != null)
        {
            if (bgmSource.clip != exampleSceneBGM)
            {
                bgmSource.clip = exampleSceneBGM;
                bgmSource.Play();
            }
            return;
        }

        // 4) 그 외 씬은 기본 BGM
        if (defaultBGM != null && bgmSource.clip != defaultBGM)
        {
            bgmSource.clip = defaultBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayAudioOnStartButton()
    {
        if (startButtonSFX != null)
            sfxSource.PlayOneShot(startButtonSFX);
    }

    private void FindAndBindStartButton()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(PlayAudioOnStartButton);

        GameObject buttonObj = GameObject.Find("startbtn");
        if (buttonObj != null)
        {
            startButton = buttonObj.GetComponent<Button>();
            if (startButton != null)
                startButton.onClick.AddListener(PlayAudioOnStartButton);
        }
        else
        {
            startButton = null;
        }
    }
}










