using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerablePlayer : PowerableObject
{

    protected override void DoYourPowerableThing()
    {
        if (gameObject.name == "Warrior")
        {
            WarriorController wController = gameObject.GetComponent<WarriorController>();
            wController.ProtectedByMage(true);
        }

        if (gameObject.name == "Engineer")
        {
            EngineerController eController = gameObject.GetComponent<EngineerController>();
            eController.ProtectedByMage(true);
        }
    }

    protected override void UndoYourPowerableThing()
    {
        if (gameObject.GetComponent<WarriorController>())
        {
            WarriorController wController = gameObject.GetComponent<WarriorController>();
            wController.ProtectedByMage(false);
        }

        if (gameObject.GetComponent<EngineerController>())
        {
            EngineerController eController = gameObject.GetComponent<EngineerController>();
            eController.ProtectedByMage(false);
        }
    }
}
