using Godot;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

public class UrlNode : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void HasAppUrl()
    {
        var gameState = GetNode<GameState>("/root/GameState");
        if (gameState.AppUrl != gameState.LastAppUrl)
        {
            string appUrl = gameState.AppUrl;
            gameState.LastAppUrl = gameState.AppUrl;
            var seedRegex = Regex.Match(appUrl, "challenge=([^&]*)");
            if (seedRegex.Success)
            {
                if (int.TryParse(seedRegex.Groups[0].Captures[0].Value.Split("challenge=")[0],
                System.Globalization.NumberStyles.HexNumber,
                CultureInfo.InvariantCulture, out int gameSeed))
                {
                    gameState.GameSeed = gameSeed;
                    gameState.NumberOfTimesPlayed++;
                    gameState.LastSeedHex = gameState.GameSeed.ToString("X");
                    gameState.FromLink = true;
                    GetTree().ChangeScene("res://Game.tscn");
                }
            }
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
