using Godot;
using System;

public class MenuButton : Button
{
    private InGameMenu _menuNode;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public NodePath MenuNodePath { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _menuNode = GetNode<InGameMenu>(MenuNodePath);
        Text = Tr("MenuBtn");
        this.Connect("pressed", this, nameof(Clicked));
    }

    private void Clicked()
    {
        GetTree().Paused = true;
        _menuNode.Visible = true;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
