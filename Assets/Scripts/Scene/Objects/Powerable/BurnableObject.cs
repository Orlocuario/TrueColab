using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour {

    public GameObject[] particles;
    public bool burned;

    private void Start()
    {
        burned = false;
        InitializeParticles(); 
    } 

    public void Burn()
    {
        ToggleParticles(true);           
        Destroy(gameObject);
    }

    protected virtual void ToggleParticles(bool active)
    {

        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(active);
            }
        }
    }

    protected void InitializeParticles()
    {
        ParticleSystem[] _particles = GetComponentsInChildren<ParticleSystem>();

        if (_particles == null || _particles.Length == 0)
        {
            return;
        }

        particles = new GameObject[_particles.Length];

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i] = _particles[i].gameObject;
        }

        ToggleParticles(false);

    }
}
