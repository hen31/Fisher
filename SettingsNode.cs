using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using System;

public class SettingsNode : Node
{
    private const string PathOfConfig = "user://settings.cfg";

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Dictionary<string, string> Settings = new Dictionary<string, string>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var configFile = new File();
        if (configFile.FileExists(PathOfConfig))
        {
            configFile.Open(PathOfConfig, File.ModeFlags.Read);
            Settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(configFile.GetAsText());
            configFile.Close();
        }
        InitDefaultParameters();

        if (bool.TryParse(Settings["SoundOn"], out bool soundOn) && !soundOn)
        {
            AudioServer.SetBusMute(0, true);
        }
    }

    private void InitDefaultParameters()
    {
        if (!Settings.ContainsKey("SoundOn"))
        {
            Settings["SoundOn"] = "True";
        }
        if (!Settings.ContainsKey("HighestScore"))
        {
            Settings["HighestScore"] = "0";
        }
    }

    public void SaveSettings()
    {
        var configFile = new File();
        configFile.Open(PathOfConfig, File.ModeFlags.Write);
        configFile.StoreString(JsonConvert.SerializeObject(Settings));
        configFile.Close();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
