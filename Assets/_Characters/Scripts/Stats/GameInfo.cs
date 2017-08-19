using UnityEngine;

public class GameInfo : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static string PlayerName { get; set; }
    public static int PlayerLevel { get; set; }
    public static BaseClass PlayerClass { get; set; }
    public static int PlayerModel { get; set; }

    public static int Strength { get; set; }
    public static int Defense { get; set; }
    public static int Health { get; set; }
    public static int Mana { get; set; }
}
