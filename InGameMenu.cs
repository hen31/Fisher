using Godot;
using System;

public class InGameMenu : PanelContainer
{
    private SettingsNode _settings;

    public Button ExitButton { get; private set; }
    public Button ResumeButton { get; private set; }
    public Button SoundButton { get; private set; }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _settings = GetNode<SettingsNode>("/root/SettingsNode");
        ExitButton = FindNode(nameof(ExitButton)) as Button;
        ExitButton.Text = Tr("ToMenuBtn");
        ExitButton.Connect("pressed", this, nameof(ExitClicked));
        ResumeButton = FindNode(nameof(ResumeButton)) as Button;
        ResumeButton.Text = Tr("ResumeBtn");
        ResumeButton.Connect("pressed", this, nameof(ResumeClicked));
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

    private void ResumeClicked()
    {
        this.Visible = false;
        GetTree().Paused = false;
    }

    private void ExitClicked()
    {
        var gameState = GetNode<GameState>("/root/GameState");
        gameState.LastScore = 0;
        gameState.AppUrl = "";
        GetTree().ChangeScene("res://StartScreen.tscn");
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
