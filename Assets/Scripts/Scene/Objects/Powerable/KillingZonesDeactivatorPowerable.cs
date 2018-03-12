using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingZonesDeactivatorPowerable : PowerableObject {

    public KillingObject[] killingObjects;

    protected override void DoYourPowerableThing()
    { 
        foreach (KillingObject kObject in killingObjects)
        {
            kObject.SetActive(false);
        }
    }

    protected override void UndoYourPowerableThing()
    {
        foreach (KillingObject kObject in killingObjects)
        {
            kObject.SetActive(true);
        }
    }


}
