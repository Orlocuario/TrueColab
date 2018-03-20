using UnityEngine;
using System.Collections;

public class GearSystemActions : ActivableSystemActions
{
    public float blockerSpeed;

    #region Common

    public void DoSomething(GearSystem gearSystem, bool notifyOthers)
    {

        switch (gearSystem.name)
        {
            case "MaquinaEngranajeA":
                HandleGearSystemA(gearSystem, notifyOthers);
                break;
            case "MaquinaEngranajeB":
                HandleGearSystemB(gearSystem, notifyOthers);
                break;
			case "MaquinaEngranajeC":
				HandleGearSystemC(gearSystem, notifyOthers);
				break;
			case "MaquinaEngranajeD":
				HandleGearSystemD(gearSystem, notifyOthers);
				break;
        }

    }

    #endregion

    #region Handlers

    private void HandleGearSystemA(GearSystem gearSystem, bool notifyOthers)
    {

        // Dispose every used gear in case of reconnection
        for (int i = 0; i < gearSystem.components.Length; i++)
        {
            string usedGearName = gearSystem.components[i].sprite.name;
            GameObject usedGear = GameObject.Find(usedGearName);

            if (usedGear)
            {
                DestroyObject(usedGearName, .1f);
            }

        }

        // Hide every placed gear
        SpriteRenderer[] componentSlots = gearSystem.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < componentSlots.Length; i++)
        {
            componentSlots[i].sprite = null;
        }

        // Change the gearsystem sprite

        SpriteRenderer systemSpriteRenderer = gearSystem.GetComponent<SpriteRenderer>();
        systemSpriteRenderer.sprite = gearSystem.activatedSprite;

        // If is Engineer: Start Coroutine

        gearSystem.ToggleParticles(true);
        SetAnimatorBool("startMoving", true, gearSystem);


        GameObject secondMachine = GameObject.Find("MaqEngranaje2");
        if (secondMachine)
        {
            ActivableSystem secondGear = secondMachine.GetComponent<ActivableSystem>();
            SetAnimatorBool("startMoving", true, secondGear);
        }

        MoveTowardsAndDie blocksMover = GameObject.Find("GiantBlockers").GetComponent<MoveTowardsAndDie>();
        blocksMover.StartMoving(gearSystem.GetParticles());

        if (notifyOthers)
        {
            SetAnimatorBool("startMovingMachine", false, gearSystem, 2f);
        }

        if (Object.FindObjectOfType<Planner>())
        {
            if (gearSystem.switchObj)
            {
                gearSystem.switchObj.ActivateSwitch();

                Planner planner = Object.FindObjectOfType<Planner>();
                planner.Monitor();
            }
        }

        if (notifyOthers)
        {
            SendMessageToServer("ObstacleDestroyed/GiantBlockers", true);
            SendMessageToServer("ActivateSystem/" + gearSystem.name, true);
        }
    }

    private void HandleGearSystemB(GearSystem gearSystem, bool notifyOthers)
    {

        // Dispose every used gear in case of reconnection
        for (int i = 0; i < gearSystem.components.Length; i++)
        {
            string usedGearName = gearSystem.components[i].sprite.name;
            GameObject usedGear = GameObject.Find(usedGearName);

            if (usedGear)
            {
                DestroyObject(usedGearName, .1f);
            }

        }

        // Hide every placed gear
        SpriteRenderer[] componentSlots = gearSystem.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < componentSlots.Length; i++)
        {
            componentSlots[i].sprite = null;
        }

        // Change the gearsystem sprite

        SpriteRenderer systemSpriteRenderer = gearSystem.GetComponent<SpriteRenderer>();
        systemSpriteRenderer.sprite = gearSystem.activatedSprite;

        // Doing Something

        OneTimeMovingObject altarEngin1 = GameObject.Find("AltarEnginMovable").GetComponent<OneTimeMovingObject>();
        altarEngin1.move = true;

        //  Planner 
        if (gearSystem.switchObj)
        {
            gearSystem.switchObj.ActivateSwitch();

            Planner planner = GameObject.FindObjectOfType<Planner>();
            planner.Monitor();
        }

        if (notifyOthers)
        {
            SendMessageToServer("ActivateSystem/" + gearSystem.name, true);
        }

    }

	private void HandleGearSystemC(GearSystem gearSystem, bool notifyOthers)
	{

		// Dispose every used gear in case of reconnection
		for (int i = 0; i < gearSystem.components.Length; i++)
		{
			string usedGearName = gearSystem.components[i].sprite.name;
			GameObject usedGear = GameObject.Find(usedGearName);

			if (usedGear)
			{
				DestroyObject(usedGearName, .1f);
			}

		}

		// Hide every placed gear
		SpriteRenderer[] componentSlots = gearSystem.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentSlots.Length; i++)
		{
			componentSlots[i].sprite = null;
		}

		// Change the gearsystem sprite

		SpriteRenderer systemSpriteRenderer = gearSystem.GetComponent<SpriteRenderer>();
		systemSpriteRenderer.sprite = gearSystem.activatedSprite;

		// Doing Something

		BubbleRotatingInstantiator bInstantiatior = GameObject.Find ("BubbleCentralInstatiator").GetComponent<BubbleRotatingInstantiator>();
		bInstantiatior.GearActivation ();

		//  Planner 
		if (gearSystem.switchObj)
		{
			gearSystem.switchObj.ActivateSwitch();

			Planner planner = GameObject.FindObjectOfType<Planner>();
			planner.Monitor();
		}

		if (notifyOthers)
		{
			SendMessageToServer("ActivateSystem/" + gearSystem.name, true);
		}
	}

	private void HandleGearSystemD(GearSystem gearSystem, bool notifyOthers)
	{

		// Dispose every used gear in case of reconnection
		for (int i = 0; i < gearSystem.components.Length; i++)
		{
			string usedGearName = gearSystem.components[i].sprite.name;
			GameObject usedGear = GameObject.Find(usedGearName);

			if (usedGear)
			{
				DestroyObject(usedGearName, .1f);
			}

		}

		// Hide every placed gear
		SpriteRenderer[] componentSlots = gearSystem.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentSlots.Length; i++)
		{
			componentSlots[i].sprite = null;
		}

		// Change the gearsystem sprite

		SpriteRenderer systemSpriteRenderer = gearSystem.GetComponent<SpriteRenderer>();
		systemSpriteRenderer.sprite = gearSystem.activatedSprite;

		// Doing Something

		BubbleRotatingInstantiator bInstantiatior = GameObject.Find ("BubbleCentralInstatiator").GetComponent<BubbleRotatingInstantiator>();
		bInstantiatior.GearActivation ();

		//  Planner 
		if (gearSystem.switchObj)
		{
			gearSystem.switchObj.ActivateSwitch();

			Planner planner = GameObject.FindObjectOfType<Planner>();
			planner.Monitor();
		}

		if (notifyOthers)
		{
			SendMessageToServer("ActivateSystem/" + gearSystem.name, true);
		}
	}
}

    #endregion

