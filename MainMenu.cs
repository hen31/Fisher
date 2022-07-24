using Godot;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class MainMenu : CanvasLayer
{
    private SettingsNode _settings;
    private Random _randomForSeed = new Random();
    public Button ExitButton { get; private set; }
    public Button StartButton { get; private set; }
    public Button SoundButton { get; private set; }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        ExitButton = FindNode(nameof(ExitButton)) as Button;
        ExitButton.Connect("pressed", this, nameof(ExitClicked));
        StartButton = FindNode(nameof(StartButton)) as Button;
        StartButton.Connect("pressed", this, nameof(StartClicked));
        _settings = GetNode<SettingsNode>("/root/SettingsNode");
        (FindNode("HighScoreLbl") as Label).Text = Tr("HighestScoreLbl") + " " + _settings.Settings["HighestScore"];
        SoundButton = FindNode(nameof(SoundButton)) as Button;
        SoundButton.Connect("pressed", this, nameof(ChangeSound));
        SetSoundText();

    }



    private void ChangeSound()
    {
        AudioServer.SetBusMute(0, !AudioServer.IsBusMute(0));
        _settings.Settings["SoundOn"] = (!AudioServer.IsBusMute(0)).ToString();
        _settings.SaveSettings();
        SetSoundText();
    }

    private void SetSoundText()
    {
        if (AudioServer.IsBusMute(0))
        {
            SoundButton.Text = Tr("SoundBtnOn");
        }
        else
        {
            SoundButton.Text = Tr("SoundBtnOf");
        }
    }
    private void StartClicked()
    {
        var gameState = GetNode<GameState>("/root/GameState");
        gameState.GameSeed = _randomForSeed.Next();
        gameState.NumberOfTimesPlayed++;
        gameState.LastSeedHex = gameState.GameSeed.ToString("X");
        gameState.FromLink = false;
        GetTree().ChangeScene("res://Game.tscn");
    }

    private void ExitClicked()
    {
        GetTree().Quit();
    }
}
