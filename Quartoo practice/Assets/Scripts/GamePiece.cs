using UnityEngine;

public class GamePiece : MonoBehaviour
{
    private GameController gameController;
    public string id;

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

    private void OnMouseDown()
    {
        if (GameInfo.gameType != 'T')
            gameController.SetSelectedPiece(this);
        else
        {
            gameController.TutorialSetPiece(this);
            gameController.HighlightClicked();
        }
    }
}
