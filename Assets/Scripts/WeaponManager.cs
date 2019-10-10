using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    public WeaponController wc;
    // Start is called before the first frame update
    void Start()
    {
        wc = transform.DeepFind("WeaponHandle").GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    //public void SetVelocity(float _valid)
    //{
    //    wc.SetFloat("Velocity", _valid);
    //}
}
