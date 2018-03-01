using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarMageEnginZone1 : PowerableObject
{

    #region Common

    protected override void DoYourPowerableThing()
    {
		
        levelManager.TogglePowerableAnimatorsWithTag("waterFalling", true, "LavaCascade2");
    }

    protected override void UndoYourPowerableThing()
    {
        levelManager.TogglePowerableAnimatorsWithTag("waterFalling", false, "LavaCascade2");
    }

    #endregion

}
