using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuideCarScene : MonoBehaviour
{
    public Button Back;              // "뒤로가기" 버튼
    public Button confirmButton;     // "확인했어요" 버튼

    void Start()
    {
        if (confirmButton != null)
            confirmButton.gameObject.SetActive(true);

        if (Back != null)
            Back.onClick.AddListener(GoToCustomerScene);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(GoToTierScene);
    }

    void GoToCustomerScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        SceneManager.LoadScene("SelectScene");
    }

    void GoToTierScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        SceneManager.LoadScene("ObjectGuide");
    }
}


