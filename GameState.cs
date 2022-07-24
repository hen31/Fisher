using Godot;
using System;

public class GameState : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public int LastScore;
    public string LastSeedHex = "";
    public bool FromLink;
    public int GameSeed;
    public bool NewHighScore = false;
    public bool DisplayAd = false;
    public string AppUrl = "";
    public string LastAppUrl = "";
    public int NumberOfTimesPlayed = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
