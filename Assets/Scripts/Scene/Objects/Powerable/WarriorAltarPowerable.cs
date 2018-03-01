
public class WarriorAltarPowerable : PowerableObject {

	protected override void DoYourPowerableThing ()
	{
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.DeactivateObject("RockBlocker");
	}
}
