using UnityEngine;

public class GameUIController : MonoBehaviour
 {
    [SerializeField] private GameObject playerATurnPanel;
    [SerializeField] private GameObject playerBTurnPanel;
    public enum GameTurnPanelType { None, ATurn, BTurn }

    
    public void OnClickBackButton()
    {
        GameManager.Instance.OpenConfirmPanel(message: "게임을 종료하시겠습니까?",
        onConfirmButtonClicked: () =>
        {
            GameManager.Instance.ChangeToMainScene();
        });
    }
    
    public void SetGameTurnPanel(GameTurnPanelType gameTurnPanelType)
    {
        switch (gameTurnPanelType)
        {
            case GameTurnPanelType.None:
                playerATurnPanel.SetActive(false); 
                playerBTurnPanel.SetActive(false);
                break;
            case GameTurnPanelType.ATurn:
                playerATurnPanel.SetActive(true);
                playerBTurnPanel.SetActive(false);
                break;
            case GameTurnPanelType.BTurn:
                playerATurnPanel.SetActive(false);
                playerBTurnPanel.SetActive(true);
                break;
        }
    }
}
