using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject confirmPanel;

    private Constants.GameType _gameType;

    private Canvas _canvas;

    private GameLogic _gameLogic;

    private GameUIController _gameUIController;

    public void ChangeToGameScene(Constants.GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene("Game");
    }

    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void OpenConfirmPanel(string message, ConfirmPanelController.OnConfirmButtonClicked onConfirmButtonClicked)
    {
        if(_canvas != null)
        {
            var confirmPanelObject = Instantiate(confirmPanel, _canvas.transform);
            confirmPanelObject.GetComponent<ConfirmPanelController>().Show(message, onConfirmButtonClicked);
        }
    }

    public void SetGameTurnPanel(GameUIController.GameTurnPanelType gameTurnPanelType)
    {
        _gameUIController.SetGameTurnPanel(gameTurnPanelType);
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _canvas = FindFirstObjectByType<Canvas>();

        if(scene.name == "Game")
        {
            var blockController = FindFirstObjectByType<BlockController>();

            if(_gameLogic != null)
            {
                blockController.InitBlocks();
            }

            _gameUIController = FindFirstObjectByType<GameUIController>();
            if(_gameUIController != null)
            {
                _gameUIController.SetGameTurnPanel(GameUIController.GameTurnPanelType.None);
            }

            _gameLogic = new GameLogic(blockController, _gameType);
        }

    }    
}
