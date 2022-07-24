using Godot;
using System;

public class GameOverScreen : Control
{
    private GameState _gameState;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var retryBtn = FindNode("RetryButton") as Button;
        retryBtn.Connect("pressed", this, nameof(RetryGameClicked));

        var audioPlayer = GetNode<AudioStreamPlayer>("WinAudioPlayer");
        _gameState = GetNode<GameState>("/root/GameState");
        var scoreLbl = FindNode("ScoreLabel") as Label;
        scoreLbl.Text = $"{Tr("ScoreWasLbl")} {_gameState.LastScore}";
        var HighScoreLbl = FindNode("HighScoreLbl") as Label;
        HighScoreLbl.Visible = _gameState.NewHighScore;
        if(_gameState.NewHighScore)
        {
            audioPlayer.Play();
        }
        _gameState.NewHighScore = false;
        if (_gameState.FromLink)
        {
            var shareScoreBtn = FindNode("ShareScoreBtn") as Button;
            shareScoreBtn.Text = Tr("ShareScoreBtn");
            shareScoreBtn.Visible = true;
            
        }
    }

    private void RetryGameClicked()
    {
        GetTree().ChangeScene("res://Game.tscn");
        _gameState.NumberOfTimesPlayed++;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
