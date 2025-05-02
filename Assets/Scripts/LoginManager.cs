using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public class LoginManager : MonoBehaviour
{
    public InputField Input_T1;       // 사번 입력
    public InputField Input_T2;       // 패스워드 입력
    public Text error_output;         // 오류 메시지
    public Button Back;               // 뒤로가기 버튼
    public Text CapsLockWarningText;  // 캡스락 경고 텍스트
    public Button Loginbtn;

    private string correctId = "251001";
    private string correctPw = "abc123!!";

    private bool hasFocused = false;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [DllImport("user32.dll")]
    private static extern short GetKeyState(int keyCode);
    private const int VK_CAPITAL = 0x14;
#endif

    private void OnEnable()
    {
        error_output.text = "";

        Back.onClick.RemoveAllListeners();
        Back.onClick.AddListener(GoToStartScene);

        Loginbtn.onClick.RemoveAllListeners();       // 기존 리스너 제거
        Loginbtn.onClick.AddListener(HandleLogin);   // 로그인 버튼에 함수 연결

        Input_T2.contentType = InputField.ContentType.Password;
        Input_T2.ForceLabelUpdate();

        hasFocused = false;

        Input_T1.characterLimit = 6;
        Input_T1.contentType = InputField.ContentType.IntegerNumber;

        Input_T1.onValueChanged.AddListener(ValidateIdInput);
        Input_T2.onValueChanged.AddListener(ValidatePasswordInput);
    }


    private void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        bool isCapsLockOn = (((ushort)GetKeyState(VK_CAPITAL)) & 0xffff) != 0;
        if (CapsLockWarningText != null)
            CapsLockWarningText.gameObject.SetActive(isCapsLockOn);
#endif

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            HandleLogin();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HandleTabNavigation();
        }

        if (!hasFocused)
        {
            Input_T1.Select();  // 시작할 때는 사번 입력창에 포커스
            hasFocused = true;
        }
    }

    /// <summary>
    /// Tab / Shift+Tab 키를 눌러 입력 필드 간 이동 처리
    /// </summary>
    private void HandleTabNavigation()
    {
        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (Input_T1.isFocused && !isShiftHeld)
        {
            // 사번 입력창에서 Tab → 비밀번호 입력창으로 이동
            Input_T2.Select();
        }
        else if (Input_T2.isFocused && isShiftHeld)
        {
            // 비밀번호 입력창에서 Shift+Tab → 사번 입력창으로 이동
            Input_T1.Select();
        }
    }

    private void HandleLogin()
    {
        string id = Input_T1.text.Trim();
        string pw = Input_T2.text;

        // 사번 검사
        if (string.IsNullOrEmpty(id) || id.Length != 6)
        {
            error_output.text = "사번은 숫자 6자리여야 합니다.";
            ResetInputField(Input_T1);
            return;
        }

        // 패스워드 검사
        if (string.IsNullOrEmpty(pw))
        {
            error_output.text = "패스워드를 입력해주세요.";
            ResetInputField(Input_T2);
            return;
        }

        if (id == correctId && pw == correctPw)
        {
            error_output.text = "";
            SceneManager.LoadScene("SelectScene");
        }
        else
        {
            error_output.text = "사번 또는 패스워드가 올바르지 않습니다.";
            ResetInputField(Input_T2);
        }
    }

    private void GoToStartScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudioOnStartButton();
        }

        SceneManager.LoadScene("Start Scene");
    }

    private void ResetInputField(InputField field)
    {
        if (field == null) return;

        field.gameObject.SetActive(false);
        field.gameObject.SetActive(true);
        field.Select();
        field.ActivateInputField();
    }

    /// <summary>
    /// 사번 입력: 숫자만 허용, 6자리 제한
    /// </summary>
    private void ValidateIdInput(string input)
    {
        string onlyNumbers = Regex.Replace(input, "[^0-9]", "");

        if (onlyNumbers.Length > 6)
            onlyNumbers = onlyNumbers.Substring(0, 6);

        if (Input_T1.text != onlyNumbers)
        {
            Input_T1.text = onlyNumbers;
            Input_T1.caretPosition = onlyNumbers.Length;
        }

        if (onlyNumbers.Length != 6)
        {
            error_output.text = "사번은 숫자 6자리여야 합니다.";
        }
        else
        {
            error_output.text = "";
        }
    }

    /// <summary>
    /// 패스워드 입력: 한글 제거, 영문/숫자/특수문자만 허용
    /// </summary>
    private void ValidatePasswordInput(string input)
    {
        string validInput = Regex.Replace(input, @"[^a-zA-Z0-9!@#$%^&*()_\-+=\[{\]};:'"",.<>?/\\|`~]", "");

        if (Input_T2.text != validInput)
        {
            Input_T2.text = validInput;
            Input_T2.caretPosition = validInput.Length;
            error_output.text = "비밀번호는 영문, 숫자, 특수문자만 입력 가능합니다.";
        }
        else
        {
            error_output.text = "";
        }
    }
}








