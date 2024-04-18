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
    private bool pristine = true;

    private int turnCounter = 0;

    void Awake()
    {
        PauseMenu.isPaused = false;
        // its not the pause menu. something is keeping the move plates from interacting with the pieces properly, pressing escape to go to the menu once fixes it for some reason. once the piece becomes interactable, though, the move plates dont properly collide with existing pieces (e.g., pawns can attack pawns infront of them)
    }
    private void InitializeBoard(List<ChessmanData> dataList)
    {
        Debug.Log("isPaused " + PauseMenu.isPaused);
        foreach (GameObject square in board)
        {
            if (square == null) continue;
            Debug.Log(square.name);
            Destroy(square);
        }
        foreach (ChessmanData data in dataList)
        {
            // Debug.Log(data.player + " " + data.piece + " " + data.x + "," + data.y);
            SetPosition(CreateChessman(data.player, data.piece, data.x, data.y));
        }
    }

    public GameObject CreateChessman(string player, string piece, int x, int y)
    {
        GameObject boardSquare = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        // problem may be with this instantiate. maybe the last instances arent being killed off or something? its spawning everything in the middle of hte board
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

    public bool IsPristine()
    {
        return pristine;
    }

    public void NextTurn()
    {
        pristine = false;
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
        if (data.turnCounter > -1)
        {
            turnCounter = data.turnCounter;
            currentPlayer = data.currentPlayer;
            gameOver = data.gameOver;
            List<ChessmanData> pieces = new List<ChessmanData>();

            foreach (string pieceData in data.pieces)
            {
                pieces.Add(JsonUtility.FromJson<ChessmanData>(pieceData));
            }

            InitializeBoard(pieces);

            if (gameOver)
            {
                Winner(currentPlayer);
            }
        }
        else
        {
            List<ChessmanData> pieceList = new List<ChessmanData> {
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

            InitializeBoard(pieceList);
        }

        GameObject.FindGameObjectWithTag("SaveButton").GetComponent<Button>().interactable = false;
    }

    public void SaveData(GameData data)
    {
        data.turnCounter = turnCounter;
        data.currentPlayer = currentPlayer;
        data.gameOver = gameOver;

        List<string> pieces = new List<string>();
        foreach (GameObject boardSquare in board)
        {
            if (boardSquare == null) continue;
            Chessman chessman = boardSquare.GetComponent<Chessman>();
            pieces.Add(JsonUtility.ToJson(new ChessmanData(chessman.GetPlayer(), chessman.GetPiece(), chessman.GetX(), chessman.GetY())));
        }
        data.pieces = pieces;
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