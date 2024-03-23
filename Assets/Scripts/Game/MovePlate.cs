using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    // Chesspiece that created this MovePlate
    private GameObject parent = null;

    //board positions, not world positions
    private int x;
    private int y;

    //false: movement, true: attack
    private bool attack = false;
    private bool enPassant = false;

    public void Start()
    {
        if (IsAttackMove())
        {
            // square turns red for attack moves
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }
    private void OnMouseUp()
    {
        if (PauseMenu.isPaused) return;

        Chessman chessman = parent.GetComponent<Chessman>();
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();

        if (IsAttackMove())
        {
            Debug.Log(x + " " + (IsPassant() ? chessman.GetPlayer() == "white" ? y + 1 : y - 1 : y) + " " + IsPassant());
            GameObject targetPosition = game.GetPosition(x, IsPassant() ? chessman.GetPlayer() == "white" ? y - 1 : y + 1 : y);
            Chessman target = targetPosition.GetComponent<Chessman>();
            // TODO: add stalemate conditions; use a counter on the game object to track how many turns since a piece was taken(?)
            if (target.GetPiece() == "king")
            {
                game.Winner(target.GetPlayer() == "black" ? "white" : "black");
            }

            Destroy(targetPosition);
        }

        game.SetPositionEmpty(chessman.GetX(), chessman.GetY());
        if (chessman.GetPiece() == "pawn" && Math.Abs(chessman.GetY() - y) == 2)
        {
            chessman.SetJumpedTurn(game.GetTurnNumber());
        }

        chessman.SetX(x);
        chessman.SetY(y);
        chessman.SetWorldPosition();

        game.SetPosition(parent);
        game.NextTurn();

        chessman.RecordMoved();
        chessman.DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetParent(GameObject chessmanReference)
    {
        parent = chessmanReference;
    }
    public void SetAttackMove()
    {
        attack = true;
    }
    private bool IsAttackMove()
    {
        return attack;
    }

    private bool IsPassant()
    {
        return enPassant;
    }
    public void SetPassant()
    {
        enPassant = true;
    }
}
