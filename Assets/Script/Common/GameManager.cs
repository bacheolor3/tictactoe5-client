using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject signinPanel;
    [SerializeField] private GameObject signupPanel;

    private Constants.GameType _gameType;

    private Canvas _canvas;

    private GameObject _signinPanelInstance;
    private GameObject _signupPanelInstance;

    public bool IsAuthenticated { get; private set; }

    private GameLogic _gameLogic;

    private GameUIController _gameUIController;

    private void Start()
    {
        _canvas = FindFirstObjectByType<Canvas>();
    }

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

    public void OpenSigninPanel()
    {

        if (IsAuthenticated) return; // 이미 로그인 했음 열지 않음
        if (_canvas == null) _canvas = FindFirstObjectByType<Canvas>();
        if (_canvas == null) return;

        if (_signinPanelInstance) return; // 중복 생성 방지
        _signinPanelInstance = Instantiate(signinPanel, _canvas.transform);

        var ctrl = _signinPanelInstance.GetComponent<SigninPanelController>();
        ctrl.Show();

        // 패널이 닫힐 때 추적 변수 비우기

        ctrl.onClosed = () => { _signinPanelInstance = null; };

        //if (_canvas != null)
        //{
        //    var siginPanelObject = Instantiate(signinPanel, _canvas.transform);
        //    siginPanelObject.GetComponent<SigninPanelController>().Show();
        //}
    }

    public void OpenSignupPanel()
    {
        if (_canvas == null) _canvas = FindFirstObjectByType<Canvas>();
        if(_canvas == null) return;

        if (_signupPanelInstance) return;
        _signupPanelInstance = Instantiate(signupPanel, _canvas.transform);
        _signupPanelInstance.GetComponent<SignupPanelController>().Show();
    
        //if (_canvas != null)
        //{
        //    var signupPanelObject = Instantiate(signupPanel, _canvas.transform);
        //    signupPanelObject.GetComponent<SignupPanelController>().Show();
        //}
    }

    public void MarkSignedIn()
    {
        IsAuthenticated = true;
        if (_signinPanelInstance)
        {
            Destroy(_signinPanelInstance);
            _signinPanelInstance = null;
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
