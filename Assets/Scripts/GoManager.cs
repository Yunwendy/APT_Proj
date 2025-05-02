using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoManager : MonoBehaviour
{
    public Button GoButton;   // 인스펙터에서 Go 버튼 연결
    public Button BackButton; // 인스펙터에서 Back 버튼 연결

    void OnEnable()
    {
        if (GoButton != null)
        {
            GoButton.onClick.RemoveAllListeners();
            GoButton.onClick.AddListener(GoToGameScene);
        }

        if (BackButton != null)
        {
            BackButton.onClick.RemoveAllListeners();
            BackButton.onClick.AddListener(GoToBackScene);
        }
    }

    void GoToGameScene()
    {
        // 효과음 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        // 게임 씬으로 이동
        SceneManager.LoadScene("GameScene");
    }

    void GoToBackScene()
    {
        // 효과음 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        // 2ObjectGuide 씬으로 이동
        SceneManager.LoadScene("2ObjectGuide");
    }
}


