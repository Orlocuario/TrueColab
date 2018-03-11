using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarMageEnginZone1 : PowerableObject
{
    public int numberOfCascadeTag;
    #region Common

    protected override void DoYourPowerableThing()
    {
		
        levelManager.TogglePowerableAnimatorsWithTag("waterFalling", true, "LavaCascade" + numberOfCascadeTag);
    }

    protected override void UndoYourPowerableThing()
    {
        levelManager.TogglePowerableAnimatorsWithTag("waterFalling", false, "LavaCascade" + numberOfCascadeTag);
    }

    #endregion

}
