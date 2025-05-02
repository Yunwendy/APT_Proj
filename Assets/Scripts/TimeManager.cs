using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI StandardText;  // 기준 시간
    public TextMeshProUGUI ElapsedText;   // 소요 시간
    public TextMeshProUGUI ResultText;    // 결과 메시지

    public Button Startbtn;               // 시작 버튼
    public Button Completebtn;            // 조립 완료 버튼
    public Button Back;                   // 뒤로 가기 버튼
    public Button RetryButton;            // 다시 도전 버튼 (새로 추가)

    private float elapsedTime = 0f;
    private bool isTiming = false;
    private const float standardTime = 60f; // 기준 시간 1분 (60초)

    void Start()
    {
        Startbtn.onClick.AddListener(OnStartButtonClick);
        Back.onClick.AddListener(OnBackButtonClick);
        Completebtn.onClick.AddListener(OnCompleteButtonClick);
        RetryButton.onClick.AddListener(OnRetryButtonClick); // 다시 도전 버튼 리스너 추가

        StandardText.text = "기준 시간 : 1:00";
        ElapsedText.gameObject.SetActive(false);
        ResultText.gameObject.SetActive(false);
        Completebtn.gameObject.SetActive(false);
        RetryButton.gameObject.SetActive(false); // 초기에는 보이지 않게 설정
    }

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            ElapsedText.text = $"소요 시간 : {minutes}:{seconds:D2}";

            // 기준 시간 초과 시 자동 종료
            if (elapsedTime > standardTime)
            {
                isTiming = false;
                Completebtn.gameObject.SetActive(false);
                ResultText.text = "다시 도전하세요.";
                ResultText.color = Color.red; // 빨간색으로 설정
                RetryButton.gameObject.SetActive(true); // 다시 도전 버튼을 활성화
                StartCoroutine(FadeInText(ResultText, 1.5f));
            }
        }
    }

    void OnStartButtonClick()
    {
        elapsedTime = 0f;
        isTiming = true;

        ElapsedText.gameObject.SetActive(true);
        Startbtn.gameObject.SetActive(false);
        Completebtn.gameObject.SetActive(true);
    }

    void OnCompleteButtonClick()
    {
        isTiming = false;

        if (elapsedTime <= standardTime)
        {
            ResultText.text = "엄청난 스킬이세요!";
            ResultText.color = Color.blue; // 파란색으로 설정
        }
        else
        {
            ResultText.text = "다시 도전하세요.";
            ResultText.color = Color.red; // 빨간색으로 설정
        }

        Completebtn.gameObject.SetActive(false);
        StartCoroutine(FadeInText(ResultText, 1.5f));
    }

    void OnBackButtonClick()
    {
        SceneManager.LoadScene("SelectScene");
    }

    void OnRetryButtonClick()
    {
        // 타이머 초기화 및 다시 시작
        elapsedTime = 0f;
        isTiming = false;
        ElapsedText.gameObject.SetActive(false);
        ResultText.gameObject.SetActive(false);
        RetryButton.gameObject.SetActive(false); // 다시 도전 버튼 숨기기

        Startbtn.gameObject.SetActive(true); // 시작 버튼 다시 활성화
    }

    IEnumerator FadeInText(TextMeshProUGUI textElement, float duration)
    {
        textElement.gameObject.SetActive(true);
        Color originalColor = textElement.color;
        originalColor.a = 0;
        textElement.color = originalColor;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            textElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        textElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}


