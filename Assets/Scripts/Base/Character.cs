[System.Serializable]
public class CharacterData
{
    public CharacterInfo[] characterList;
}

[System.Serializable]
public class CharacterInfo
{
    public string name;
    public string prefab;
    public string missilePrefab;
    public int hp;
    public string speed;
    public float attackSpeed;
}
