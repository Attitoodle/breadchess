using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject movePlate;

    // Positions
    private int x = -1;
    private int y = -1;

    // Identifiers
    private string piece = "";
    private string player = "";

    private bool moved = false;
    private int jumpedTurn = -1;

    //Sprite References
    public Sprite black_king, black_queen, black_bishop, black_rook, black_knight, black_pawn;
    public Sprite white_king, white_queen, white_bishop, white_rook, white_knight, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetWorldPosition();

        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (GetPlayer() == "black")
        {
            switch (GetPiece())
            {
                case "king":
                    spriteRenderer.sprite = black_king;
                    break;
                case "queen":
                    spriteRenderer.sprite = black_queen;
                    break;
                case "bishop":
                    spriteRenderer.sprite = black_bishop;
                    break;
                case "knight":
                    spriteRenderer.sprite = black_knight;
                    break;
                case "rook":
                    spriteRenderer.sprite = black_rook;
                    break;
                case "pawn":
                    spriteRenderer.sprite = black_pawn;
                    break;
            }

        }
        else
        {
            switch (GetPiece())
            {
                case "king":
                    spriteRenderer.sprite = white_king;
                    break;
                case "queen":
                    spriteRenderer.sprite = white_queen;
                    break;
                case "bishop":
                    spriteRenderer.sprite = white_bishop;
                    break;
                case "knight":
                    spriteRenderer.sprite = white_knight;
                    break;
                case "rook":
                    spriteRenderer.sprite = white_rook;
                    break;
                case "pawn":
                    spriteRenderer.sprite = white_pawn;
                    break;
            }
        }
    }
    public void SetPlayer(string player)
    {
        this.player = player;
    }
    public string GetPlayer()
    {
        return player;
    }
    public void SetPiece(string piece)
    {
        this.piece = piece;
    }
    public string GetPiece()
    {
        return piece;
    }

    private float OffsetForBoardPosition(float pos)
    {
        return pos * 0.66f - 2.3f;
    }


    public void SetWorldPosition()
    {
        // z = -1 to keep in front of board
        this.transform.position = new Vector3(OffsetForBoardPosition(x), OffsetForBoardPosition(y), -1.0f);
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public void SetX(int x)
    {
        this.x = x;
    }

    public void SetY(int y)
    {
        this.y = y;
    }

    private void OnMouseUp()
    {
        if (!PauseMenu.isPaused && !controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == GetPlayer())
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    private void InitiateMovePlates()
    {
        void SetOrthogonal()
        {
            LineMovePlate(1, 0);
            LineMovePlate(0, 1);
            LineMovePlate(-1, 0);
            LineMovePlate(0, -1);
        }

        void SetDiagonal()
        {
            LineMovePlate(1, 1);
            LineMovePlate(1, -1);
            LineMovePlate(-1, 1);
            LineMovePlate(-1, -1);
        }

        switch (GetPiece())
        {
            case "king":
                if (!moved)
                {
                    //do the castling stuff
                    // check the rooks on the same row and see if they've moved, then give the option to castle if neither have moved.
                    //todo after that add a turn timer to the UI 
                }
                SingleOmniDirectionalMovePlate();
                break;
            case "queen":
                SetOrthogonal();
                SetDiagonal();
                break;
            case "bishop":
                SetDiagonal();
                break;
            case "knight":
                LMovePlate();
                break;
            case "rook":
                SetOrthogonal();
                break;
            case "pawn":
                PawnMovePlate();
                break;
        }
    }
    private void LineMovePlate(int xOffset, int yOffset)
    {
        Game game = controller.GetComponent<Game>();

        int worldX = x + xOffset;
        int worldY = y + yOffset;

        while (game.IsValidBoardPosition(worldX, worldY) && game.GetPosition(worldX, worldY) == null)
        {
            MovePlateSpawn(worldX, worldY);
            worldX += xOffset;
            worldY += yOffset;
        }

        if (game.IsValidBoardPosition(worldX, worldY) && game.GetPosition(worldX, worldY).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(worldX, worldY);
        }

    }

    private void LMovePlate()
    {
        PointMovePlate(x + 1, y + 2);
        PointMovePlate(x + 1, y - 2);
        PointMovePlate(x - 1, y + 2);
        PointMovePlate(x - 1, y - 2);
        PointMovePlate(x + 2, y + 1);
        PointMovePlate(x + 2, y - 1);
        PointMovePlate(x - 2, y + 1);
        PointMovePlate(x - 2, y - 1);
    }

    private void SingleOmniDirectionalMovePlate()
    {
        PointMovePlate(x, y + 1);
        PointMovePlate(x, y - 1);
        PointMovePlate(x - 1, y + 1);
        PointMovePlate(x - 1, y);
        PointMovePlate(x - 1, y - 1);
        PointMovePlate(x + 1, y + 1);
        PointMovePlate(x + 1, y);
        PointMovePlate(x + 1, y - 1);
    }
    private void PointMovePlate(int x, int y)
    {
        Game game = controller.GetComponent<Game>();
        if (game.IsValidBoardPosition(x, y))
        {
            GameObject boardSquare = game.GetPosition(x, y);
            if (boardSquare == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (boardSquare.GetComponent<Chessman>().GetPlayer() != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    private void PawnMovePlate()
    {
        Game game = controller.GetComponent<Game>();

        int leftOffset = x - 1;
        int rightOffset = x + 1;
        int jump = 0;
        int passantRow = 4;
        int targetY = y;

        // Determine direction and if a jump is possible.
        if (GetPlayer() == "white")
        {
            targetY++;
            if (!HasMoved()) jump = 1;
        }
        else
        {
            targetY--;
            if (!HasMoved()) jump = -1;
            passantRow = 3;
        }

        // Create jump option if possible.
        if (game.IsValidBoardPosition(x, targetY))
        {
            if (game.GetPosition(x, targetY) == null)
            {
                MovePlateSpawn(x, targetY);
                if (jump != 0)
                {
                    MovePlateSpawn(x, targetY + jump);
                }
            }
        }

        // Create diagonal attacks if possible.
        if (game.IsValidBoardPosition(leftOffset, targetY) && game.GetPosition(leftOffset, targetY) != null)
        {
            if (game.GetPosition(leftOffset, targetY).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(leftOffset, targetY);
            }
        }
        if (game.IsValidBoardPosition(rightOffset, targetY) && game.GetPosition(rightOffset, targetY) != null && game.GetPosition(rightOffset, targetY).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(rightOffset, targetY);
        }

        // Create En Passant attacks if possible.
        if (y == passantRow)
        {
            if (game.IsValidBoardPosition(leftOffset, y))
            {
                GameObject neighboringSquare = game.GetPosition(leftOffset, y);
                if (neighboringSquare != null)
                {
                    Chessman neighbor = neighboringSquare.GetComponent<Chessman>();
                    if (neighbor.player != player && neighbor.Passantable())
                    {
                        MovePlateAttackSpawn(leftOffset, targetY, true);
                    }
                }
            }

            if (game.IsValidBoardPosition(rightOffset, y))
            {

                GameObject neighboringSquare = game.GetPosition(rightOffset, y);
                if (neighboringSquare != null)
                {
                    Chessman neighbor = neighboringSquare.GetComponent<Chessman>(); if (neighbor.player != player && neighbor.Passantable())
                    {
                        MovePlateAttackSpawn(rightOffset, targetY, true);
                    }
                }
            }
        }
    }

    public void MovePlateSpawn(int x, int y)
    {
        MovePlate movePlateScript = Instantiate(movePlate, new Vector3(OffsetForBoardPosition(x), OffsetForBoardPosition(y), -3.0f), Quaternion.identity).GetComponent<MovePlate>();
        movePlateScript.SetParent(gameObject);
        movePlateScript.SetCoords(x, y);
    }

    public void MovePlateAttackSpawn(int x, int y, bool passant = false)
    {
        MovePlate movePlateScript = Instantiate(movePlate, new Vector3(OffsetForBoardPosition(x), OffsetForBoardPosition(y), -3.0f), Quaternion.identity).GetComponent<MovePlate>();
        movePlateScript.SetAttackMove();
        movePlateScript.SetParent(gameObject);
        movePlateScript.SetCoords(x, y);
        if (passant)
        {
            movePlateScript.SetPassant();
        }
    }

    public void RecordMoved()
    {
        moved = true;
    }
    private bool HasMoved()
    {
        return moved;
    }
    public void SetJumpedTurn(int turnNumber)
    {
        this.jumpedTurn = turnNumber;
    }
    public bool Passantable()
    {
        int currentTurn = controller.GetComponent<Game>().GetTurnNumber();
        if ((currentTurn - 1) > 0)
        {
            if (player == "white")
            {
                if (jumpedTurn == currentTurn)
                {
                    return true;
                }

            }
            else if (jumpedTurn == (currentTurn - 1))
            {
                return true;
            }
        }
        return false;
    }
}
