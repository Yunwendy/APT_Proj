using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // 씬 전환에 필요

public class ControllPanelManager : MonoBehaviour
{
    public Button controllButton;
    public GameObject controllImage;
    public Button closeButton;

    public Button Back; // Back 버튼 추가

    private void Start()
    {
        controllImage.SetActive(false);
        closeButton.gameObject.SetActive(false);

        controllButton.onClick.AddListener(ShowControllImage);
        closeButton.onClick.AddListener(HideControllImage);
        Back.onClick.AddListener(GoBackToSelectScene); // 리스너 추가

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void ShowControllImage()
    {
        controllImage.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    private void HideControllImage()
    {
        Debug.Log("닫기 버튼 눌림");
        controllImage.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }

    private void GoBackToSelectScene()
    {
        SceneManager.LoadScene("SampleCom"); // 씬 이름은 정확히 입력
    }
}




