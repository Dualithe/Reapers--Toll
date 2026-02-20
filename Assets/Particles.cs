using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem part;

    private void Awake()
    {
        var main = part.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    
    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
