 public interface IWeapon
{
    public void Attack();
    public WeaponData GetWeaponData();
    float FireRate { get; }
}
