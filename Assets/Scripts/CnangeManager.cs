using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    public Button ChangeButton; // ChangeButton 버튼
    public Button Back;         // Back 버튼

    void Start()
    {
        ChangeButton.onClick.AddListener(GoToGuideScene);
        Back.onClick.AddListener(GoToStartScene);
    }

    void GoToGuideScene()
    {
        SceneManager.LoadScene("GuideScene"); // 여기를 수정!
    }

    void GoToStartScene()
    {
        SceneManager.LoadScene("Start Scene");
    }
}

