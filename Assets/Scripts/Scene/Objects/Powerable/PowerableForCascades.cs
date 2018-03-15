using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableForCascades : PowerableObject {

    public int numberOfCascadeTag;

    protected override void DoYourPowerableThing()
    {
        levelManager.TogglePowerableAnimatorsWithTag("WaterFalling", true, "LavaCascade" + numberOfCascadeTag);
    }

    protected override void UndoYourPowerableThing()
    {
        levelManager.TogglePowerableAnimatorsWithTag("WaterFalling", false, "LavaCascade" + numberOfCascadeTag);
    }

}
