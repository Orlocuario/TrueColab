
public class MageTeleporter : PlayerTeleporter
{

    #region Start

    protected override void Start()
    {
        playerToTeleport = "Mage";
        base.Start();
    }

    #endregion

}
