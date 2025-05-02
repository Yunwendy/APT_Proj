using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button startButton;  // 시작 버튼 추가
    public Button endButton;   // 종료 버튼

    void OnEnable()
    {
        // 중복 방지를 위해 리스너 제거
        startButton.onClick.RemoveAllListeners();
        endButton.onClick.RemoveAllListeners();

        // 버튼 이벤트 재등록
        startButton.onClick.AddListener(() =>
        {
            Debug.Log("시작 버튼 클릭됨");
            SceneManager.LoadScene("LoginScene");  // 원하는 씬으로 변경
        });

        endButton.onClick.AddListener(() =>
        {
            Debug.Log("종료 버튼 클릭됨");
            SceneManager.LoadScene("EndingScene");
        });
    }
}


  /*  void Update()
    {
        // ESC 키 누르면 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC 누름 - 게임 종료");
            Application.Quit();

            // 에디터에서 테스트할 땐 아래 줄도 같이 있어야 작동 확인됨
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

} */







