
public class WarriorTeleporter : PlayerTeleporter
{

    #region Start

    protected override void Start()
    {
        playerToTeleport = "Warrior";
        base.Start();
    }

    #endregion

}
