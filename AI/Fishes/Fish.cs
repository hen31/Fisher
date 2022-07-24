using Godot;
using System;
using System.Diagnostics;

namespace Fisher2
{
    public class Fish : MoveableObject
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        [Export]
        public int Score { get; set; }
        [Export]
        public float LineOffset { get; internal set; }
    }
}
