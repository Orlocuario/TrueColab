using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingInstantiator : MonoBehaviour {

    public Vector2 initialPosition;
    public Vector2 targetPosition;
    public float instantiationRate;
    public float moveSpeed;
    public int powerTime;

	public string objectName;

    public bool isWorking;
    public bool playerHasReturned;

    // Use this for initialization
    void Start () {

		initialPosition = gameObject.transform.position;
		CheckParameters ();
		StartCoroutine (InstantiateDamaging());
		
	}

	// Update is called once per frame
	private IEnumerator InstantiateDamaging()
	{

		while (true) 
		{
			GameObject damagingObject = (GameObject)Instantiate (Resources.Load ("Prefabs/Damaging/" + objectName));
			if (damagingObject != null) 
			{
				damagingObject.transform.position = initialPosition;
                TakeCareOfMOvement(damagingObject);
                TakeCareOfPowerable(damagingObject);

				yield return new WaitForSeconds (instantiationRate);
			}
		}
	}

    private void TakeCareOfPowerable(GameObject damagingObject)
    {
        if (damagingObject.GetComponent<PowerableObject>())
        {
            PowerableObject powerable = damagingObject.GetComponent<PowerableObject>();
            PowerableObject.Power[] powers = powerable.powers;
            powerable.shutdownFrames = powerTime;
            for (int i = 0; i<powers.Length; i++)
            {
                if (i == 0)
                {
                    powers[i].attack = new PunchController();
                }
                if (i == 1)
                {
                    powers[i].expectedParticles = new WarriorPoweredParticles();
                }
            }
        }
    }

    private void TakeCareOfMOvement(GameObject damagingObject)
    {
        if (damagingObject.GetComponent<OneTimeMovingObject>())
        {
            OneTimeMovingObject objectMovement = damagingObject.GetComponent<OneTimeMovingObject>();
            objectMovement.target = targetPosition;
            objectMovement.moveSpeed = moveSpeed;
            objectMovement.move = true;
            objectMovement.diesAtTheEnd = true;

            if (targetPosition.y > transform.position.y + 1)
            {
                Quaternion _Q = objectMovement.transform.rotation;
                objectMovement.transform.rotation = _Q * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            }
            else if (targetPosition.y < transform.position.y - 1)
            {
                Quaternion _Q = objectMovement.transform.rotation;
                objectMovement.transform.rotation = _Q * Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
            }

            if (targetPosition.x > transform.position.x)
            {
                objectMovement.transform.localScale *= -1;
            }

        }
    }

    private void CheckParameters()
	{
		if (initialPosition == new Vector2 (0f, 0f))
		{
			Debug.LogError ("DamagingInstantiator: " + gameObject.name + " needs an Initial Position");
		}

		if (targetPosition == new Vector2 (0f, 0f))
		{
			Debug.LogError ("DamagingInstantiator: " + gameObject.name + " needs a Target Position");
		}

		if (instantiationRate == 0f)
		{
			Debug.LogError ("DamagingInstantiator: " + gameObject.name + " needs an Instantiatio Rate");
		}

		if (moveSpeed == 0f)
		{
			Debug.LogError ("DamagingInstantiator: " + gameObject.name + " needs a MoveSpeed");
		}
	}
}
