using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ParticleSystemScript : NetworkBehaviour
{
    public GameObject multiParticlePrefab;
    public float playDuration = 1;
    public List<ParticleSystem> particleSystems;
    public bool playOnStart = true;
    public bool loop = false;

	// Use this for initialization
	void Start ()
    {
        Debug.Log(name + "particlesystem start");
        if (multiParticlePrefab != null)
        {
            Debug.Log(name + " multiParticlePrefab != null");
            List<ParticleSystem> childParticleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
            Debug.Log(name + " childParticleSystems length = " + childParticleSystems.Count);
            foreach(ParticleSystem particleSyst in childParticleSystems)
            {
                particleSystems.Add(particleSyst);
                Debug.Log(name + " Particle System added - " + particleSystems.Count);
            }
        }

        if (!playOnStart)
            StopParticles();
        else
            PlayParticles();

        

    }
	
    [Command]
    public void CmdPlayParticles()
    {
        RpcPlayParticlesForTime();
    }

    [ClientRpc]
    public void RpcPlayParticlesForTime()
    {
        Debug.Log("insidePlayParticlesForTime");
        foreach (ParticleSystem system in particleSystems)
        {
            system.Play();
            Debug.Log(system.name);
        }

        Invoke("StopParticles", playDuration);
    }

    [ClientRpc]
    public void RpcPlayParticles()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            system.Play();
        }
    }
        
    [ClientRpc]
    public void RpcStopParticles()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            system.Stop();
        }
    }

    public void PlayParticles()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            system.Play();
        }
    }

    public void StopParticles()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            system.Stop();
        }
    }

    public void StopLooping()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            system.loop = false;
        }
    }

}
