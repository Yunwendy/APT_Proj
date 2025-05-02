using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelector : MonoBehaviour
{
    public Button Car;
    public Button Back;
    public Button Quiz;

    public static bool CarButtonUnlock = false; // 처음부터 비활성화!

    void Start()
    {
        Car.interactable = CarButtonUnlock; // 상태에 따라 버튼 설정

        // 리스너 등록
        Car.onClick.RemoveAllListeners();
        Back.onClick.RemoveAllListeners();
        Quiz.onClick.RemoveAllListeners();

        Car.onClick.AddListener(GoToGameScene);
        Back.onClick.AddListener(GoToStartScene);
        Quiz.onClick.AddListener(GoToQuizScene);
    }

    void GoToGameScene()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAudioOnStartButton();

        SceneManager.LoadScene("GuideCarScene");
    }

    void GoToStartScene()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAudioOnStartButton();

        SceneManager.LoadScene("LoginScene");
    }

    void GoToQuizScene()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAudioOnStartButton();

        SceneManager.LoadScene("QuizChallenge");
    }
}





