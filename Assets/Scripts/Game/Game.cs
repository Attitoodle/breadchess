using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;
    // Start is called before the first frame update

    private GameObject[,] board = new GameObject[8, 8]; // 8x8 board
    private GameObject[] black = new GameObject[16]; // 16 pieces per player
    private GameObject[] white = new GameObject[16]; // 16 pieces per player

    private string currentPlayer = "white";

    private bool gameOver = false;

    private int turnCounter = -1;

    void Start()
    {
        GameObject.FindGameObjectWithTag("ResignButton").GetComponent<Button>().interactable = true;
        turnCounter = 0;
        white = new GameObject[] {
            CreateChessman("white", "rook", 0, 0),
            CreateChessman("white", "knight", 1, 0),
            CreateChessman("white", "bishop", 2, 0),
            CreateChessman("white", "queen", 3, 0),
            CreateChessman("white", "king", 4, 0),
            CreateChessman("white", "bishop", 5, 0),
            CreateChessman("white", "knight", 6, 0),
            CreateChessman("white", "rook", 7, 0),
            CreateChessman("white", "pawn", 0, 1),
            CreateChessman("white", "pawn", 1, 1),
            CreateChessman("white", "pawn", 2, 1),
            CreateChessman("white", "pawn", 3, 1),
            CreateChessman("white", "pawn", 4, 1),
            CreateChessman("white", "pawn", 5, 1),
            CreateChessman("white", "pawn", 6, 1),
            CreateChessman("white", "pawn", 7, 1)
        };
        black = new GameObject[] {
            CreateChessman("black", "rook", 0, 7),
            CreateChessman("black", "knight", 1, 7),
            CreateChessman("black", "bishop", 2, 7),
            CreateChessman("black", "queen", 3, 7),
            CreateChessman("black", "king", 4, 7),
            CreateChessman("black", "bishop", 5, 7),
            CreateChessman("black", "knight", 6, 7),
            CreateChessman("black", "rook", 7, 7),
            CreateChessman("black", "pawn", 0, 6),
            CreateChessman("black", "pawn", 1, 6),
            CreateChessman("black", "pawn", 2, 6),
            CreateChessman("black", "pawn", 3, 6),
            CreateChessman("black", "pawn", 4, 6),
            CreateChessman("black", "pawn", 5, 6),
            CreateChessman("black", "pawn", 6, 6),
            CreateChessman("black", "pawn", 7, 6)
        };


        for (int i = 0; i < black.Length; i++)
        {
            SetPosition(white[i]);
            SetPosition(black[i]);
        }
    }

    public GameObject CreateChessman(string player, string piece, int x, int y)
    {
        GameObject boardSquare = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman chessman = boardSquare.GetComponent<Chessman>();
        chessman.SetPiece(piece);
        chessman.SetPlayer(player);
        chessman.SetX(x);
        chessman.SetY(y);
        chessman.Activate();

        return boardSquare;
    }

    public void SetPosition(GameObject boardSquare)
    {
        Chessman chessman = boardSquare.GetComponent<Chessman>();

        board[chessman.GetX(), chessman.GetY()] = boardSquare;
    }

    public void SetPositionEmpty(int x, int y)
    {
        board[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return board[x, y];
    }

    public bool IsValidBoardPosition(int x, int y)
    {
        if (x < 0 || y < 0 || x >= board.GetLength(0) || y >= board.GetLength(1))
        {
            return false;
        }
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "black")
        {
            currentPlayer = "white";
            turnCounter++; //increment turn count on every white move
        }
        else
        {
            currentPlayer = "black";
        }
    }

    public void Update()
    {
        // restart game only on a keyboard press, not a mouse click
        if (gameOver == true && Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Escape)))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
    }

    public void Winner(string player)
    {
        gameOver = true;

        string playerName = player == "white" ? "Strawberry" : "Blueberry";

        Text winnerText = GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>();
        winnerText.enabled = true;
        winnerText.text = playerName + " won in " + turnCounter + " moves.";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("ResignButton").GetComponent<Button>().interactable = false;
    }

    public int GetTurnNumber()
    {
        return turnCounter;
    }

    public void QuitGame()
    {
        Utils.QuitGame();
    }
}
