using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackScript : MonoBehaviour
{
    public Button BackButton; // 뒤로 가기 버튼
    public Button GoButton;   // 앞으로 가기 버튼 (SampleCom으로)

    void OnEnable()
    {
        if (BackButton != null)
        {
            BackButton.onClick.RemoveAllListeners();
            BackButton.onClick.AddListener(GoBackToGuide);
        }

        if (GoButton != null)
        {
            GoButton.onClick.RemoveAllListeners();
            GoButton.onClick.AddListener(GoToSampleCom);
        }
    }

    void GoBackToGuide()
    {
        // 효과음 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        SceneManager.LoadScene("ObjectGuide");
    }

    void GoToSampleCom()
    {
        // 효과음 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        SceneManager.LoadScene("SampleCom");
    }
}



