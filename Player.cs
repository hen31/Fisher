using Godot;
using System;
using System.Diagnostics;

namespace Fisher2
{
    public class Player : Sprite
    {
        private SettingsNode _settings;

        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        [Export]
        public NodePath ScoreLabelPath { get; set; }
        private Label ScoreLabel { get; set; }
        private FishingLine FishingLine { get; set; }
        public Area2D HookArea { get; set; }
        public AudioStreamPlayer SplashSoundEffectPlayer { get; set; }
        public AudioStreamPlayer LoseSoundEffectPlayer { get; set; }

        public int Score { get; set; } = 0;
        private bool _lost = false;
        private float _timeTillLost = 0f;
        public AnimationPlayer AnimationPlayer { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            ScoreLabel = GetNode<Label>(ScoreLabelPath);
            SplashSoundEffectPlayer = GetNode<AudioStreamPlayer>(nameof(SplashSoundEffectPlayer));
            LoseSoundEffectPlayer = GetNode<AudioStreamPlayer>(nameof(LoseSoundEffectPlayer));
            AnimationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
            FishingLine = FindNode("FishingLine") as FishingLine;
            HookArea = FindNode("HookArea") as Area2D;
            HookArea.Connect("body_entered", this, nameof(HookMakingContact));
            HookArea.Connect("area_entered", this, nameof(HasEnteredArea));
            SetScoreLabel();
            _settings = GetNode<SettingsNode>("/root/SettingsNode");
            
        }

        public void HasEnteredArea(Area2D area)
        {
            FishingLine.StopCatch();
        }

        private void HookMakingContact(Node bodyConnecting)
        {
            if (bodyConnecting is Fish fish && !FishingLine.HasFishHooked())
            {
                FishingLine.HookFish(fish);
            }
            else if (bodyConnecting is Obstacle)
            {
                LoseSoundEffectPlayer.Play();
                _lost = true;
                FishingLine.QueueFree();
                AnimationPlayer.Play("LeaveAnimation");
                //GameOver
            }
        }

        public void AddScore(int amount)
        {
            SplashSoundEffectPlayer.Play();
            Score += amount;
            SetScoreLabel();
            SaveHighScoreIfNeeded();
        }

        private void SaveHighScoreIfNeeded()
        {
            if (Score > int.Parse(_settings.Settings["HighestScore"]))
            {
                _settings.Settings["HighestScore"] = Score.ToString();
                var gameState = GetNode<GameState>("/root/GameState");
                gameState.NewHighScore = true;
            }
        }
        public void Ending()
        {
            LoseSoundEffectPlayer.Play();
        }
        public override void _ExitTree()
        {
            _settings.SaveSettings();
        }
        private void SetScoreLabel()
        {
            ScoreLabel.Text = $"{Tr("ScoreLbl")} {Score}";
        }
        public override void _Process(float delta)
        {
            if (_lost)
            {
                _timeTillLost += delta;
                if (_timeTillLost >= 3f)
                {
                    var gameState = GetNode<GameState>("/root/GameState");
                    gameState.LastScore = Score;
                    gameState.AppUrl = "";
                    gameState.DisplayAd = true;
                    GetTree().ChangeScene("res://GameOver.tscn");
                }
            }
        }
        public override void _UnhandledInput(InputEvent inputEvent)
        {
            if (!_lost)
            {
                if (Input.IsActionJustPressed("ThrowRod"))
                {
                    FishingLine.Throw();
                }
                if (Input.IsActionJustPressed("TestFishCatch"))
                {
                    var fish = GD.Load<PackedScene>("res://AI/Fishes/Fish5.tscn").Instance();
                    this.AddChild(fish);
                    FishingLine.CallDeferred("HookFish", fish as Fish);
                }
            }
        }
    }
}