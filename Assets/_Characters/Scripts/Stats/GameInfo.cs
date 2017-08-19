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

    public static int Damage { get; set; }
    public static int Armor { get; set; }
    public static int Health { get; set; }
    public static int Mana { get; set; }

    public static float HealthRegen { get; set; }
    public static float ManaRegen { get; set; }

    public static float CritChance { get; set; }
    public static float CritDamage { get; set; }
}
