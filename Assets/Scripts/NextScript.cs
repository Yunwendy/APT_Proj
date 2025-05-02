using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScript : MonoBehaviour
{
    public Button nextButton;  // Next 버튼을 연결할 변수
    public Button backButton;  // Back 버튼을 연결할 변수

    void Start()
    {
        // Next 버튼에 클릭 이벤트 추가
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }

        // Back 버튼에 클릭 이벤트 추가
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
    }

    // Next 버튼 클릭 시 씬 전환
    private void OnNextButtonClicked()
    {
        // 효과음 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        // "2ObjectGuide" 씬으로 전환
        SceneManager.LoadScene("2ObjectGuide");
    }

    // Back 버튼 클릭 시 씬 전환
    private void OnBackButtonClicked()
    {
        // 효과음 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        // "GuideCarScene" 씬으로 전환
        SceneManager.LoadScene("GuideCarScene");
    }
}



