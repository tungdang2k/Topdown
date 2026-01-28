public interface IAmmoProvider
{
    int CurrentAmmo { get; }
    int MagazineSize { get; }
    int ReserveAmmo { get; }
    bool IsReloading { get; }
    void AddReserveAmmo(int amount);
}
