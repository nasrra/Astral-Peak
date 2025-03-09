using System.Linq;
using System.Text.Json.Serialization;

public class GameData{
    [JsonInclude]
    public string 
        scene_to_load = "TutorialRoom1",
        spawn_point = "Enter";
    [JsonInclude]
    public bool[] boss_states = Enumerable.Repeat(false, 3).ToArray();
    // public bool[] boss_states = Enumerable.Repeat(true, 3).ToArray();
    public bool game_clear = false;
}
