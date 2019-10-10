using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public GameObject weapon;
    private Animator weaponAnim;
    [SerializeField]private AudioClip fire;

    // Start is called before the first frame update
    void Start()
    {
        weaponAnim = weapon.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip FireClip()
    {
        if(fire != null)
        {
            return fire;
        }
        else
        {
            return null; //应该返回一个写死的默认音效，但是这个音效应该交给一个系统来返回
        }
    }


    public void SetFloat(string name, float _value)
    {
        weaponAnim.SetFloat(name, Mathf.Lerp(weaponAnim.GetFloat(name), _value, 0.2f));
    }

    public void SetBool(string name, bool _value)
    {
        weaponAnim.SetBool(name, _value);
    }

    public void SetTrigger(string name)
    {
        weaponAnim.SetTrigger(name);
    }

    public float GetFloat(string name)
    {
        return weaponAnim.GetFloat(name);
    }

    

}
