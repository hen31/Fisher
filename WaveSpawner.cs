using Godot;
using System;
using Troschuetz.Random;

namespace Fisher2
{
    public class WaveSpawner : Node2D
    {
        [Export]
        public float WaveLayerYOffset = -24f;
        [Export]
        public float WaveLayerXOffset = 24f;

        // Declare member variables here. Examples:
        [Export]
        public int AmountOfWaves { get; set; } = 25;
        [Export]
        public int WaveDepth { get; set; } = 2;
        [Export]
        public PackedScene WaveScene { get; set; }

        [Export]
        public float WaveWidth { get; set; }
        [Export]
        public float PlaySpeed { get; set; } = 0.5f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            TRandom rand = new TRandom();
            float yOffset = 0f;
            float xOffset = 0f;

            for (int currentWaveDepth = 0; currentWaveDepth < WaveDepth; currentWaveDepth++)
            {
                Vector2 locationOfWave = GlobalPosition + new Vector2(xOffset, yOffset);
                Vector2 waveOffset = new Vector2(WaveWidth, 0);
                for (int i = 0; i < AmountOfWaves; i++)
                {
                    Sprite waveSprite = WaveScene.Instance() as Sprite;
                    waveSprite.PauseMode = PauseMode;
                    this.AddChild(waveSprite);
                    waveSprite.GlobalPosition = locationOfWave;
                    var animationPlayer = waveSprite.GetNode<AnimationPlayer>("AnimationPlayer");
                    // animationPlayer.Advance((i + yOffset % waveSprite.Hframes) / 2f);
                    animationPlayer.Advance(rand.Next(0, (int)(animationPlayer.CurrentAnimationLength * 1000f)) / 1000f);
                    animationPlayer.PlaybackSpeed = PlaySpeed;
                    if (PauseMode == PauseModeEnum.Stop)
                    {
                        animationPlayer.Stop();
                    }
                    waveSprite.ZIndex = -currentWaveDepth;
                    locationOfWave += waveOffset;
                }
                yOffset += WaveLayerYOffset;
                xOffset += WaveLayerXOffset;
            }
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}