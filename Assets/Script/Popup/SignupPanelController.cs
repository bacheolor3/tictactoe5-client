using TMPro;
using UnityEngine;

public struct SignupData
{
    public string username;
    public string password;
    public string nickname; 
}

public struct SignupResult
{
    public string message;
}

public class SignupPanelController : PanelController
{
    [SerializeField] private TMP_InputField userNewnameInputfield;
    [SerializeField] private TMP_InputField userNewPwInputfield;
    [SerializeField] private TMP_InputField passwordCkField;
    [SerializeField] private TMP_InputField userNewnickfield;

    public void OnClickConfirmButton()
    {
        string newName = userNewnameInputfield.text;
        string newPw = userNewPwInputfield.text;
        string newNick = userNewnickfield.text;
        string newPwChk = passwordCkField.text;

        if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newPw) || string.IsNullOrEmpty(newNick))
        {
            GameManager.Instance.OpenConfirmPanel("아이디/비밀번호/닉네임은 모두 필요합니다.", () => {});
            Shake();
            return;
        }

        if(newName.Length < 4)
        {
            GameManager.Instance.OpenConfirmPanel("아이디는 4글자 이상이어야 합니다", () =>
            {
                userNewnameInputfield.text = "";
                userNewnameInputfield.Select();
            });
            Shake();
            return;
        }

        if(newPw != newPwChk)
        {
            GameManager.Instance.OpenConfirmPanel("비밀번호가 다릅니다", () => { });
            Shake();
            return;
        }

        var signupData = new SignupData();
        signupData.username = newName;
        signupData.password = newPw;
        signupData.nickname = newNick;

        StartCoroutine(NetworkManager.Instance.Signup(signupData,
            () =>
            {
                GameManager.Instance.OpenConfirmPanel("회원가입이 왼료되었습니다. 로그인 해 주세요.", () =>
                {
                    Hide();
                    GameManager.Instance.OpenSigninPanel();
                });
            },
            (code) =>
            {
                if (code == 3) // 409 중복
                {
                    GameManager.Instance.OpenConfirmPanel("이미 사용중인 아이디입니다",
                   () =>
                   {
                       userNewnameInputfield.Select();
                   });
                }
                else if (code == -2) // 400 or 클라 검증 실패
                {
                    GameManager.Instance.OpenConfirmPanel("입력값을 확인해 주세요.", () => { });
                }
                else if (code == -1) // 네트워크
                {
                    GameManager.Instance.OpenConfirmPanel("네트워크 오류입니다. 잠시 후 다시 시도해 주세요.", () => { });
                }
                else if (code == -3) // 500
                {
                    GameManager.Instance.OpenConfirmPanel("서버 오류입니다. 잠시 후 다시 시도해 주세요.", () => { });
                }
                else
                {
                    GameManager.Instance.OpenConfirmPanel("회원가입에 실패했습니다.", () => { });
                }
                Shake();
            }
        ));
    }

    public void OnClickCancelButton()
    {
        Hide();
    }
}
