using UnityEngine;

public class SimpleCreditScroll : MonoBehaviour
{
    public RectTransform creditText;
    public float speed = 50f;
    public float endY = 2000f; // 텍스트가 이 위치 이상 올라가면 종료

    private bool hasQuit = false;

    void Update()
    {
        if (creditText != null)
        {
            creditText.anchoredPosition += Vector2.up * speed * Time.deltaTime;

            if (!hasQuit && creditText.anchoredPosition.y >= endY)
            {
                hasQuit = true;
                Debug.Log("엔딩 크레딧 종료. 게임을 종료합니다.");
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // 에디터에서만 작동
#endif
            }
        }
    }
}

