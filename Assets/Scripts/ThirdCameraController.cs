using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdCameraController : ICameraController
{
    protected Vector3 cameraDampVelocity;

    void Start()
    {
        Initiate();
        //lockState = false;
        cameraHandle.transform.eulerAngles = new Vector3(20.0f, 0, 0);
    }

    void FixedUpdate ()
    {

        //提前获取角色旋转角 
        Vector3 tempModeleuler = model.transform.eulerAngles;

        //移动角色
        playerHandle.transform.Rotate(Vector3.up, userInput.RSRight * hSpeed * Time.fixedDeltaTime);
        
        //锁定转动(未拿出武器时,不需要clear角色旋转角)
        if (!playerHandle.GetComponent<ActorController>().WeaponOn())
        {
            model.transform.eulerAngles = tempModeleuler;         
        }
        //移动摄像机
        if (!isAI)
        {
            //摄像机垂直运动
            UpdateCamera();
            cameraHandle.transform.localEulerAngles = new Vector3(rotateEularX, 0, 0);
            //摄像机位置运动
            if (playerHandle.GetComponent<ActorController>().WeaponOn())
            {
                mainCamera.transform.position = transform.position;
            }
            else
            {
                mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.position, ref cameraDampVelocity, smoothFollowRate);
            }
            //摄像机旋转
            mainCamera.transform.LookAt(cameraHandle.transform);
        }
    }
}
