public interface Attackable
{
    void Attack(Damageable damageable);
}

public interface Damageable
{
    public void Damaged(int damage);
}
