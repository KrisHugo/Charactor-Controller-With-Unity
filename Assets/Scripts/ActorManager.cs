using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;
    public WeaponManager wm;
    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<ActorController>();
        wm = Bind<WeaponManager>(ac.FPHandle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private T Bind<T>(GameObject go) where T : IActorManagerInterface
    {
        T tempInst;
        tempInst = go.GetComponent<T>();
        if (tempInst == null)
        {
            tempInst = go.AddComponent<T>();
        }
        tempInst.am = this;
        return tempInst;
    }

}
