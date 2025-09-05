using TMPro;
using UnityEngine;

public struct SigninData
{
    public string username;
    public string password;
}

public struct SigninResult
{
    public int result;
}

public class SigninPanelController : PanelController
{
    [SerializeField] private TMP_InputField usernameInputfield;
    [SerializeField] private TMP_InputField passwordInputfield;
       

    public void OnClickConfirmButton()
    {
        string username = usernameInputfield.text;
        string password = passwordInputfield.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Shake();
            return;
        }

        var signinData = new SigninData();
        signinData.username = username;
        signinData.password = password;

        StartCoroutine(NetworkManager.Instance.Signin(signinData, 
            () =>
            {
                Hide();
            }, 
            (result) =>
            {
                if(result == 0)
                {
                    GameManager.Instance.OpenConfirmPanel("유저네임이 유효하지 않습니다", 
                    () =>
                    {
                        usernameInputfield.text = "";
                        passwordInputfield.text = "";
                    });
                }
                else if(result == 1)
                {
                    GameManager.Instance.OpenConfirmPanel("패스워드가 유효하지 않습니다", 
                    () =>
                    {
                        passwordInputfield.text = "";
                    });
            }
        }));

    }

}
