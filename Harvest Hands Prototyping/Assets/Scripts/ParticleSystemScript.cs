using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ParticleSystemScript : NetworkBehaviour
{

    public float playDuration = 1;
    public List<ParticleSystem> particleSystems;
    public bool playOnStart = true;

	// Use this for initialization
	void Start ()
    {
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

}
