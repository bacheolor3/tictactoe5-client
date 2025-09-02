using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public void OnClickBackButton()
    {
        GameManager.Instance.ChangeToMainScene();
    }
}
