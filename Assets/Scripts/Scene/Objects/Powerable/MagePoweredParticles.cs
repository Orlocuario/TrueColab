using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagePoweredParticles : PoweredParticles
{

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
