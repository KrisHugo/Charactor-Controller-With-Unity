using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnGroundSensor : MonoBehaviour
{

    public CapsuleCollider capcol;
    public float offset = 0.1f;

    private float radius;
    private Vector3 point1;
    private Vector3 point2;

    void Awake()
    {
        capcol = GetComponentInParent<CapsuleCollider>();
        radius = capcol.radius - 0.05f;

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius - offset);
        point2 = transform.position + transform.up * (capcol.height - offset) - transform.up * (radius - offset);

        Collider[] colliders = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("ground"));

        if (colliders.Length != 0)
        {
            //获取audio,并尝试用sendmessageUpward去传递给ActorController
            //if(colliders[0].GetComponent<AudioManager>() == null)
            //{
            //    colliders[0].gameObject.AddComponent<AudioManager>();
            //}
            //GroundType tempClip = colliders[0].GetComponent<AudioManager>().groundType;
            //GetComponentInParent<ActorController>().SetAudioClip(tempClip);
            
            //
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("NotGround");
        }
    }
}
