using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fisher2
{
    public class MoveableObject : KinematicBody2D
    {
        [Export]
        public float Speed { get; set; } = 50f;

        public Vector2 Direction { get; set; }
        public SpawnDespawnArea SpawnedBy { get; set; }
        public Sprite Sprite { get; set; }
        public CollisionShape2D CollisionShape { get; private set; }
        public Particles2D Particles2D { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Sprite = GetNode<Sprite>(nameof(Sprite));
            CollisionShape = GetNode<CollisionShape2D>(nameof(CollisionShape));
            if (HasNode(nameof(Particles2D)))
            {
                Particles2D = GetNode<Particles2D>(nameof(Particles2D));
            }
        }

        public void DisableCollisions()
        {
            CollisionShape.CallDeferred("set", "disabled", true);
            Particles2D?.QueueFree();
            Particles2D = null;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            Position += (Direction * Speed * delta);
        }
    }
}
