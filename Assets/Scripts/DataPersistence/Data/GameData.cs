using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<ChessmanData> pieces;
    public string currentPlayer;
    public bool gameOver;
    public int turnCounter;


}


