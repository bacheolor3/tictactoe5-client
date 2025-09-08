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
            GameManager.Instance.OpenConfirmPanel("���̵�/��й�ȣ/�г����� ��� �ʿ��մϴ�.", () => {});
            Shake();
            return;
        }

        if(newName.Length < 4)
        {
            GameManager.Instance.OpenConfirmPanel("���̵�� 4���� �̻��̾�� �մϴ�", () =>
            {
                userNewnameInputfield.text = "";
                userNewnameInputfield.Select();
            });
            Shake();
            return;
        }

        if(newPw != newPwChk)
        {
            GameManager.Instance.OpenConfirmPanel("��й�ȣ�� �ٸ��ϴ�", () => { });
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
                GameManager.Instance.OpenConfirmPanel("ȸ�������� �޷�Ǿ����ϴ�. �α��� �� �ּ���.", () =>
                {
                    Hide();
                    GameManager.Instance.OpenSigninPanel();
                });
            },
            (code) =>
            {
                if (code == 3) // 409 �ߺ�
                {
                    GameManager.Instance.OpenConfirmPanel("�̹� ������� ���̵��Դϴ�",
                   () =>
                   {
                       userNewnameInputfield.Select();
                   });
                }
                else if (code == -2) // 400 or Ŭ�� ���� ����
                {
                    GameManager.Instance.OpenConfirmPanel("�Է°��� Ȯ���� �ּ���.", () => { });
                }
                else if (code == -1) // ��Ʈ��ũ
                {
                    GameManager.Instance.OpenConfirmPanel("��Ʈ��ũ �����Դϴ�. ��� �� �ٽ� �õ��� �ּ���.", () => { });
                }
                else if (code == -3) // 500
                {
                    GameManager.Instance.OpenConfirmPanel("���� �����Դϴ�. ��� �� �ٽ� �õ��� �ּ���.", () => { });
                }
                else
                {
                    GameManager.Instance.OpenConfirmPanel("ȸ�����Կ� �����߽��ϴ�.", () => { });
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
