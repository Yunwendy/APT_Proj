using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GuideTextFadeIn : MonoBehaviour
{
    public TextMeshProUGUI guideText;
    [TextArea(3, 10)]
    public string[] steps;
    public float delayBetweenLines = 1.5f;
    public float fadeDuration = 1.0f;

    public Button backButton;         // "뒤로가기" 버튼
    public Button confirmButton;      // "확인했어요" 버튼

    void Start()
    {
        confirmButton.gameObject.SetActive(false); // 처음에는 숨기기
        StartCoroutine(FadeInLines());

        if (backButton != null)
            backButton.onClick.AddListener(GoToCustomerScene);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(GoToTierScene);
    }

    IEnumerator FadeInLines()
    {
        guideText.text = "";

        for (int i = 0; i < steps.Length; i++)
        {
            string line = steps[i];
            string previousText = guideText.text;
            yield return StartCoroutine(FadeInLine(line, previousText));
            yield return new WaitForSeconds(delayBetweenLines);
        }

        // 모든 줄 출력 후 "확인했어요" 버튼 보이기
        if (confirmButton != null)
            confirmButton.gameObject.SetActive(true);
    }

    IEnumerator FadeInLine(string line, string previousText)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            int alpha = Mathf.RoundToInt(Mathf.Lerp(0, 255, t / fadeDuration));
            string alphaCode = $"<alpha=#{alpha:X2}>";
            guideText.text = previousText + "\n" + alphaCode + line;
            yield return null;
        }

        guideText.text = previousText + "\n" + line;
    }

    void GoToCustomerScene()
    {
        SceneManager.LoadScene("CustomerScene");
    }

    void GoToTierScene()
    {
        SceneManager.LoadScene("TierScene");
    }
}
