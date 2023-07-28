[System.Serializable]
public class CharacterData
{
    public CharacterInfo[] characterList;
}

[System.Serializable]
public class CharacterInfo
{
    public int id;
    public string name;
    public string prefab;
    public string missilePrefab;
    public string skillImage;
    public float skillTime;
    public int hp;
    public string speed;
    public float attackSpeed;
    public int attackDamage;
    public string info;
}
