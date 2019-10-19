using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public GameObject weapon;
    private Animator weaponAnim;
    private AudioSource weaponSource;
    [SerializeField]private AudioEventReactor fireAudio;

    // Start is called before the first frame update
    void Start()
    {
        weaponAnim = weapon.GetComponent<Animator>();
        weaponSource = weapon.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void FireOneShot()
    {
        //Audio
        fireAudio.Play(weaponSource);
        //Anim
        SetTrigger("Fire");
        //PostProcess

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
