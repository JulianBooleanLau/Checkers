using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayExplosion : MonoBehaviour
{
    public static ParticleSystem grayExplosion;
    // Start is called before the first frame update
    void Start()
    {
        grayExplosion = GetComponent<ParticleSystem>();
        grayExplosion.Stop();
    }

    public static void explode(Vector3 position) {
        grayExplosion.transform.position = position;
        grayExplosion.Play();
    }
}
