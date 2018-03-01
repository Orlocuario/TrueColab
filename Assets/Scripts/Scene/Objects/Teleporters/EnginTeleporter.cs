
public class EnginTeleporter : PlayerTeleporter
{

    #region Start

    protected override void Start()
    {
        playerToTeleport = "Engineer";
        base.Start();
    }
    
    #endregion

}
