using Fisher2;
using Godot;
using System;
using System.Diagnostics;

public class FishingLine : Line2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public NodePath PlayerPath { get; set; }
    [Export]
    float minAngle = -90f;
    [Export]
    float maxAngle = 90f;
    [Export]
    float speedOfSwing = 30f;
    [Export]
    float maxLenghtOfLine = 800f;
    [Export]
    float defaultLenghtOfLine = 200f;
    [Export]
    float throwSpeed = 30f;
    [Export]
    float retractSpeed = 30f;

    private bool _swingForward = true;
    public bool throwing = false;
    public bool pullingBack = false;
    float lenghtOfLine = 200f;
    private Sprite _hook;
    private Player _player;
    private Fish _currentlyOnHook;

    public bool HasFishHooked()
    {
        return _currentlyOnHook != null;
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lenghtOfLine = defaultLenghtOfLine;
        _hook = GetNode<Sprite>("hook");
        _player = GetNode<Player>(PlayerPath);
        DrawLineAndMoveFishIfNeeded();
    }

    internal void HookFish(Fish fish)
    {
        pullingBack = true;
        _currentlyOnHook = fish;
        fish.GetParent().RemoveChild(fish);
        CallDeferred("add_child", fish);
        fish.RotationDegrees = -90f;
        fish.Sprite.FlipH = false;
        fish.DisableCollisions();
    }

    private void DrawLineAndMoveFishIfNeeded()
    {
        ClearPoints();
        AddPoint(Vector2.Zero);
        AddPoint(new Vector2(0, lenghtOfLine));
        _hook.Position = new Vector2(-8f, lenghtOfLine + 8f);
        if (_currentlyOnHook != null)
        {
            _currentlyOnHook.Position = new Vector2(0, lenghtOfLine + _currentlyOnHook.LineOffset);
        }
    }

    internal void StopCatch()
    {
        pullingBack = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (throwing)
        {
            if (!pullingBack)
            {
                lenghtOfLine = Mathf.Min(lenghtOfLine + (throwSpeed * delta), maxLenghtOfLine);
                if (lenghtOfLine >= maxLenghtOfLine)
                {
                    pullingBack = true;
                }
            }
            else
            {
              
                lenghtOfLine = Mathf.Max(lenghtOfLine - (retractSpeed * delta), defaultLenghtOfLine);
                if (lenghtOfLine <= defaultLenghtOfLine)
                {
                    pullingBack = false;
                    throwing = false;
                    if (_currentlyOnHook != null)
                    {
                       
                        _player.AddScore(_currentlyOnHook.Score);
                        _currentlyOnHook.QueueFree();
                        _currentlyOnHook = null;
                    }
                }
            }
            DrawLineAndMoveFishIfNeeded();
        }
        else
        {
            if (_swingForward)
            {
                this.RotationDegrees += (speedOfSwing * delta);
                if (this.RotationDegrees >= maxAngle)
                {
                    _swingForward = false;
                    float toMuch = this.RotationDegrees - maxAngle;
                    this.RotationDegrees -= toMuch;
                }
            }
            else
            {
                this.RotationDegrees -= (speedOfSwing * delta);
                if (this.RotationDegrees <= minAngle)
                {
                    _swingForward = true;
                    float toMuch = minAngle - this.RotationDegrees;
                    this.RotationDegrees += toMuch;
                }
            }
        }

    }

    internal bool Throw()
    {
        if (throwing != true)
        {
            throwing = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
