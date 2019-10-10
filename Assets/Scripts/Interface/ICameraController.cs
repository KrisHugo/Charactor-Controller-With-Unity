using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraViewMode
{
    normal,
    aim,
}
public class ICameraController : MonoBehaviour
{

    [Header("Camera Setting")]
    public bool isReflectY = false;
    public float hSpeed = 100.0f;
    public float vSpeed = 80.0f;
    public float smoothFollowRate = 0.3f;
    public bool isAI = false;
    public float smoothViewRate = 5f;
    //public Image lockDot;
    //public bool lockState = false;

    [Header("Editor Setting")]
    protected IUserInput userInput;
    protected GameObject cameraHandle;
    protected GameObject playerHandle;
    protected GameObject model;
    protected GameObject mainCamera;
    protected float rotateEularX;
    [SerializeField]protected float minEularX = 85f;
    [SerializeField]protected float maxEularX = -90f;
    [SerializeField]protected int targetView = 60;


    protected void Initiate()
    {
        rotateEularX = 20.0f;
        cameraHandle = transform.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        model = playerHandle.GetComponent<ActorController>().model;
        userInput = playerHandle.GetComponent<ActorController>().userInput;
        if (!isAI)
        {
            mainCamera = Camera.main.gameObject;
            mainCamera.GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("renderInOther"));
            //lockDot.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    protected void UpdateCamera()
    {
        rotateEularX -= userInput.RSUp * (isReflectY ? 1f : -1f) * vSpeed * Time.fixedDeltaTime;
        rotateEularX = Mathf.Clamp(rotateEularX, maxEularX, minEularX);
        userInput.VerticalView = -rotateEularX;
    }

    protected virtual void UpdateView()
    {
        mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, targetView, smoothViewRate * Time.fixedDeltaTime);
    }

    public void ModifyView(CameraViewMode viewMode)
    {
        switch(viewMode)
        {
            case CameraViewMode.aim:
                targetView = 40;
                break;
            case CameraViewMode.normal:
                targetView = 60;
                break;
        }
    }

    //public void LockUnlock()
    //{
    //    Vector3 modelOrigin1 = model.transform.position;
    //    Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
    //    Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
    //    Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, !isAI? LayerMask.GetMask("enemy") : LayerMask.GetMask("player"));
    //    if(cols.Length == 0)
    //    {
    //        OnOffLock(null, false, false, isAI);
    //    }
    //    else
    //    {
    //        foreach (var col in cols)
    //        {
    //            if(lockTarget != null && lockTarget.obj == col.gameObject)
    //            {
    //                OnOffLock(null, false, false, isAI);
    //                break;
    //            }
    //            OnOffLock(new LockTarget(col.gameObject, col.bounds.extents.y), true, true, isAI);
    //            break;
    //        }
    //    }
    //}

    //private void OnOffLock(LockTarget _lockTarget, bool _lockDot, bool _lockState, bool _isAI)
    //{
    //    lockTarget = _lockTarget;
    //    if (!isAI)
    //    {
    //        lockDot.enabled = _lockDot;
    //    }
    //    lockState = _lockState;
    //}

    //private void Update()
    //{
    //     if(lockTarget != null)
    //     {
    //if (!isAI) 
    //{
    //	lockDot.rectTransform.position = Camera.main.WorldToScreenPoint (lockTarget.obj.transform.position + new Vector3 (0, lockTarget.halfHeight, 0));
    //}
    //         if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f)
    //         {
    //             OnOffLock(null, false, false, isAI);
    //         }
    //         if (lockTarget.am != null && lockTarget.am.sm.isDie)
    //         {
    //             OnOffLock(null, false, false, isAI);
    //         }
    //     }
    //}

    //private class LockTarget
    //{
    //    public GameObject obj;
    //    public float halfHeight;
    //    public ActorManager am;


    //    public LockTarget(GameObject _obj, float _halfHeight)
    //    {
    //        obj = _obj;
    //        halfHeight = _halfHeight;
    //        am = obj.GetComponent<ActorManager>();
    //    }
    //}
}
