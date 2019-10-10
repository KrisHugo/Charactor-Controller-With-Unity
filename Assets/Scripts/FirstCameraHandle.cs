using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCameraHandle : ICameraController
{
    public Camera FPV;
    
    void Start()
    {
        Initiate();
        FPVInitiate();
    }
    void FPVInitiate()
    {
        FPV = Instantiate(Camera.main);
        Destroy(FPV.GetComponent<AudioListener>());
        FPV.clearFlags = CameraClearFlags.Depth;
        FPV.cullingMask = 1 << LayerMask.NameToLayer("renderInOwn");//~(1 << LayerMask.NameToLayer("renderInOther"))
        FPV.depth = 1;
    }
    void FixedUpdate()
    {
        //第一人称中不进行修正角色旋转（暂时不考虑自由视角）

        //水平旋转角色
        playerHandle.transform.Rotate(Vector3.up, userInput.RSRight * hSpeed * Time.fixedDeltaTime);

        //移动摄像机（第一人称）
        if (!isAI)
        {
            //handle进行垂直运动
            UpdateCamera();
            transform.localEulerAngles = new Vector3(rotateEularX, 0, 0);

            //相机跟随
            mainCamera.transform.rotation = transform.rotation;
            mainCamera.transform.position = transform.position;
            FPV.transform.rotation = transform.rotation;
            FPV.transform.position = transform.position;

            //相机视角缩放
            if(targetView != mainCamera.GetComponent<Camera>().fieldOfView)
            {
                UpdateView();
            }
        }
    }
    protected override void UpdateView()
    {
        base.UpdateView();
        FPV.GetComponent<Camera>().fieldOfView = Mathf.Lerp(FPV.GetComponent<Camera>().fieldOfView, targetView, smoothViewRate * Time.fixedDeltaTime);
    }

}
