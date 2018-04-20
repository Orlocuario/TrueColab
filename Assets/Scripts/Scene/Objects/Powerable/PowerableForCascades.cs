using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableForCascades : PowerableObject {

    public int lavaId;

    protected override void DoYourPowerableThing()
    {
        levelManager.PowerableToggleLavaIntoWater("WaterFalling", true, lavaId);
    }

    protected override void UndoYourPowerableThing()
    {
        levelManager.PowerableToggleLavaIntoWater("WaterFalling", false, lavaId);
    }

}
