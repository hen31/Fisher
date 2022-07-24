using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;
namespace Fisher2
{
    public class SpawnDespawnArea : Area2D
    {
        [Export]
        public int MaxLevel { get; set; } = 30;
        // Declare member variables here. Examples:
        [Export]
        public Array<PackedScene> SpawnableFishes { get; set; }

        [Export]
        public Array<PackedScene> SpawnableObstacles { get; set; }

        [Export]
        public Vector2 Direction { get; set; }
        [Export]
        public bool FlipFish { get; set; }
        [Export]
        public NodePath RandomNumberGenerator { get; set; }
        [Export]
        public float LevelTime = 10f;

        Timer _spawnTimer;
        private RandomGenerator _randomNumberGenerator;
        CollisionShape2D _area;
        private Timer _timer;
        private int minX, maxX, minY, maxY;

        private float _time = 0f;
        public int CurrentLevel = 5;
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _spawnTimer = GetNode<Timer>("Timer");
            _randomNumberGenerator = GetNode<RandomGenerator>(RandomNumberGenerator);
            _area = GetNode<CollisionShape2D>("Area");
            var spawnShape = (_area.Shape as RectangleShape2D);
            _timer = GetNode<Timer>("Timer");
            _timer.Connect("timeout", this, nameof(TimerClick));
            minX = (int)(_area.GlobalPosition.x - spawnShape.Extents.x);
            maxX = (int)(_area.GlobalPosition.x + spawnShape.Extents.x);

            minY = (int)(_area.GlobalPosition.y - spawnShape.Extents.y);
            maxY = (int)(_area.GlobalPosition.y + spawnShape.Extents.y);
            Connect("body_entered", this, nameof(ItemOverlapping));

        }

        private void ItemOverlapping(Node itemOverlapping)
        {
            if (itemOverlapping is MoveableObject moveableObject && moveableObject.SpawnedBy != this)
            {
                itemOverlapping.QueueFree();
            }
        }
        bool _first = true;
        private void TimerClick()
        {
            if (_first)
            {
                _first = false;
                SpawnFish();
                SpawnFish();
                SpawnFish();
                SpawnFish();
                SpawnObstacle();
            }
            if (_randomNumberGenerator.Next(0, 100) < 2)
            {
                SpawnFish();
            }
            if (CurrentLevel > 1)
            {
                if (_randomNumberGenerator.Next(0, 200000) < CurrentLevel * CurrentLevel * CurrentLevel)
                {
                    SpawnObstacle();

                }
            }
        }

        private void SpawnFish()
        {
            var fish = SpawnableFishes[_randomNumberGenerator.Next(0, SpawnableFishes.Count)].Instance() as MoveableObject;
            GetParent().AddChild(fish);
            fish.Direction = Direction;
            fish.Sprite.FlipH = FlipFish;
            if (FlipFish && fish.Particles2D != null)
            {
                /*(fish.Particles2D.ProcessMaterial as ParticlesMaterial).Direction *= -1;*/
                fish.Particles2D.Position = new Vector2(fish.Particles2D.Position.x * -1, fish.Particles2D.Position.y);
                fish.Particles2D.RotationDegrees = 180f;
            }
            fish.GlobalPosition = new Vector2(_randomNumberGenerator.Next(minX, maxX), _randomNumberGenerator.Next(minY, maxY));
            fish.SpawnedBy = this;
        }
        private void SpawnObstacle()
        {
            var obstacle = SpawnableObstacles[_randomNumberGenerator.Next(0, SpawnableObstacles.Count)].Instance() as MoveableObject;
            GetParent().AddChild(obstacle);
            obstacle.Direction = Direction;
            obstacle.Sprite.FlipH = FlipFish;
            obstacle.GlobalPosition = new Vector2(_randomNumberGenerator.Next(minX, maxX), _randomNumberGenerator.Next(minY, maxY));
            obstacle.SpawnedBy = this;
            obstacle.RotationDegrees = _randomNumberGenerator.Next(-80, 80);

        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            _time += delta;
            float levelTime = Mathf.Min(LevelTime * 20, LevelTime * CurrentLevel);
            while (_time >= levelTime)
            {
                _time -= levelTime;
                CurrentLevel = Godot.Mathf.Min(CurrentLevel + 1, MaxLevel);
                // SpawnObstacle();
                Debug.WriteLine($"Level now {CurrentLevel}");
            }
        }
    }
}
