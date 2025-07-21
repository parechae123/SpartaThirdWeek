namespace Test.Data;

[Serializable]
public class PlayerData
{
    public string Name {get; set;} = "Player";
    
    public int Level { get; set; } = 1;
    
    public string Class { get; set; } = "Musician";
    
    public int HealthPoint { get; set; } = 200;
    
    public int AttackPoint { get; set; } = 20;
    
    public int DefensePoint { get; set; } = 20;

    public int[] Inventory { get; set; } = [];
    
    public int[] Equipments { get; set; } = [];
}