using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour, IDataPersistence
{
    public GameObject chesspiece;
    // Start is called before the first frame update

    private GameObject[,] board = new GameObject[8, 8]; // 8x8 board

    private string currentPlayer = "white";

    private bool gameOver = false;

    private int turnCounter = -1;

    void Awake()
    {
        GameObject.FindGameObjectWithTag("ResignButton").GetComponent<Button>().interactable = true;
    }
    void Start()
    {
        turnCounter = 0;
        // TODOS: put this in its own set pieces method
        // need to track the pieces as a list so they can be iterated on and saved
        List<ChessmanData> dataList = new List<ChessmanData> {
            new ChessmanData("white", "rook", 0, 0),
            new ChessmanData("white", "knight", 1, 0),
            new ChessmanData("white", "bishop", 2, 0),
            new ChessmanData("white", "queen", 3, 0),
            new ChessmanData("white", "king", 4, 0),
            new ChessmanData("white", "bishop", 5, 0),
            new ChessmanData("white", "knight", 6, 0),
            new ChessmanData("white", "rook", 7, 0),
            new ChessmanData("white", "pawn", 0, 1),
            new ChessmanData("white", "pawn", 1, 1),
            new ChessmanData("white", "pawn", 2, 1),
            new ChessmanData("white", "pawn", 3, 1),
            new ChessmanData("white", "pawn", 4, 1),
            new ChessmanData("white", "pawn", 5, 1),
            new ChessmanData("white", "pawn", 6, 1),
            new ChessmanData("white", "pawn", 7, 1),
            new ChessmanData("black", "rook", 0, 7),
            new ChessmanData("black", "knight", 1, 7),
            new ChessmanData("black", "bishop", 2, 7),
            new ChessmanData("black", "queen", 3, 7),
            new ChessmanData("black", "king", 4, 7),
            new ChessmanData("black", "bishop", 5, 7),
            new ChessmanData("black", "knight", 6, 7),
            new ChessmanData("black", "rook", 7, 7),
            new ChessmanData("black", "pawn", 0, 6),
            new ChessmanData("black", "pawn", 1, 6),
            new ChessmanData("black", "pawn", 2, 6),
            new ChessmanData("black", "pawn", 3, 6),
            new ChessmanData("black", "pawn", 4, 6),
            new ChessmanData("black", "pawn", 5, 6),
            new ChessmanData("black", "pawn", 6, 6),
            new ChessmanData("black", "pawn", 7, 6)
        };

        InitializeBoard(dataList);
    }

    private void InitializeBoard(List<ChessmanData> dataList)
    {
        foreach (GameObject square in board)
        {
            if (square == null) continue;
            Destroy(square);
        }
        foreach (ChessmanData data in dataList)
        {
            SetPosition(CreateChessman(data.player, data.piece, data.x, data.y));
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

        Debug.Log("next turn " + this.GetTurnNumber());
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

    public void LoadData(GameData data)
    {
        turnCounter = data.turnCounter;
        currentPlayer = data.currentPlayer;
        gameOver = data.gameOver;

        Debug.Log(data.currentPlayer);
        InitializeBoard(data.pieces);

        if (gameOver)
        {
            Winner(currentPlayer);
        }
    }

    public void SaveData(GameData data)
    {
        data.turnCounter = turnCounter;
        data.currentPlayer = currentPlayer;
        data.gameOver = gameOver;

        List<ChessmanData> pieces = new List<ChessmanData>();

        foreach (GameObject boardSquare in board)
        {
            if (boardSquare == null) continue;
            Chessman chessman = boardSquare.GetComponent<Chessman>();
            pieces.Add(new ChessmanData(chessman.GetPlayer(), chessman.GetPiece(), chessman.GetX(), chessman.GetY()));
        }

        data.pieces = pieces;



        Debug.Log("Save Data " + data.turnCounter + " " + turnCounter + " " + this.GetTurnNumber());
    }
}

public struct ChessmanData
{
    public int x;
    public int y;
    public string player;
    public string piece;

    public ChessmanData(string player, string piece, int x, int y)
    {
        this.player = player;
        this.piece = piece;
        this.x = x;
        this.y = y;
    }
}