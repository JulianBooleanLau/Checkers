using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedExplosion : MonoBehaviour
{
    public static ParticleSystem redExplosion;
    // Start is called before the first frame update
    void Start()
    {
        redExplosion = GetComponent<ParticleSystem>();
        redExplosion.Stop();
    }

    public static void explode(Vector3 position) {
        redExplosion.transform.position = position;
        redExplosion.Play();
    }

}
