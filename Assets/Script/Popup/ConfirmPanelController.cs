using TMPro;
using UnityEngine;

public class ConfirmPanelController : PanelController
{
    [SerializeField] private TMP_Text messageText;

    public delegate void OnConfirmButtonClicked();
    private OnConfirmButtonClicked _onConfirmButtonClicked;

    public void Show(string messsage, OnConfirmButtonClicked onConfirmButtonClicked)
    {
        messageText.text = messsage;
        _onConfirmButtonClicked = onConfirmButtonClicked;
        base.Show();
    }

    public void OnClickCOnfirmButton()
    {
        Hide(() =>
        {
            _onConfirmButtonClicked?.Invoke();
        });
    }

    public void OnClickCloseButton()
    {
        Hide();
    }
}
