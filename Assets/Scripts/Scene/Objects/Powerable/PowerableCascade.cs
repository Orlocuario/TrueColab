
public class PowerableCascade : PowerableObject
{
    public int numberOfCascadeTag;

    #region Common

    protected override void DoYourPowerableThing()
    {
        levelManager.TogglePowerableAnimatorsWithTag("WaterFalling", true, "LavaCascade" + numberOfCascadeTag);
    }

    protected override void UndoYourPowerableThing()
    {
        levelManager.TogglePowerableAnimatorsWithTag("WaterFalling", false, "LavaCascade" + numberOfCascadeTag);
    }

    #endregion

}
