using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public void OnClickRightButton()
    {
        GameManager.Instance.rightBClicked = true;
    }

    public void OnRightButtonUp()
    {
        GameManager.Instance.rightBClicked = false;
    }

    public void OnClickLeftButton()
    {
        GameManager.Instance.leftBClicked = true;
    }

    public void OnLeftButtonUp()
    {
        GameManager.Instance.leftBClicked = false;
    }

    public void RestartGame()
    {
        if(!GameManager.Instance.gameOver)
            GameManager.Instance.RestartGame();
    }
}
