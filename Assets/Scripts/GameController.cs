using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player {
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor {
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public Text gameOverText;
    private string playerSide;
    private int moveCount;
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public GameObject startInfo;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    public void SetGameControllerReferenceOnButtons() {
        foreach(var button in buttonList) {
            button.GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    void Awake() {
        moveCount = 0;
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
    }

    public string GetPlayerSide() {
        return this.playerSide;
    }

    public void EndTurn() {
        moveCount++;
        // Brute force determination for winning

        if(buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide) {
            // Top row
            GameOver(playerSide);
        }
        else if(buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide) {
            // Middle row
            GameOver(playerSide);
        }
        else if(buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide) {
            // Bottom row
            GameOver(playerSide);
        }
        else if(buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide) {
            // Left column
            GameOver(playerSide);
        }
        else if(buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide) {
            // Middle column
            GameOver(playerSide);
        }
        else if(buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide) {
            // Right column
            GameOver(playerSide);
        }
        else if(buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide) {
            // Top left to bottom right diagonal
            GameOver(playerSide);
        }
        else if(buttonList[6].text == playerSide && buttonList[4].text == playerSide && buttonList[2].text == playerSide) {
            // Bottom left to top right diagonal
            GameOver(playerSide);
        }
        else if(moveCount >= 9) {
            GameOver("draw");
        }
        else {
            ChangeSides();
        }
    }

    void GameOver(string winningPlayer) {
        if(winningPlayer == "draw") {
            SetGameOverText("It's a draw!");
            SetPlayerColorsInactive();
        }
        else {
            SetGameOverText(winningPlayer + " Wins!");
        }
        SetBoardInteractable(false);
        restartButton.SetActive(true);
    }

    void ChangeSides() {
        if(playerSide == "X") {
            SetPlayerColors(playerO, playerX);
            playerSide = "O";
        }
        else {
            SetPlayerColors(playerX, playerO);
            playerSide = "X";
        }
    }

    void SetGameOverText(string text) {
        gameOverPanel.SetActive(true);
        gameOverText.text = text;
    }

    void SetBoardInteractable(bool toggle) {
        foreach(var button in buttonList) {
            button.GetComponentInParent<Button>().interactable = toggle;
        }
    }

    public void RestartGame() {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        foreach(var button in buttonList) {
            button.text = "";
        }
        restartButton.SetActive(false);
        startInfo.SetActive(true);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer) {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    public void SetStartingSide(string startingSide) {
        if(startingSide == "X") {
            SetPlayerColors(playerX, playerO);
        }
        else {
            SetPlayerColors(playerO, playerX);
        }
        playerSide = startingSide;
        StartGame();
    }

    void StartGame() {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    void SetPlayerButtons(bool toggle) {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive() {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
}
