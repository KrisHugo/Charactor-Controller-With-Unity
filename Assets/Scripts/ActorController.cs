using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;//TPS
    public GameObject FPHandle;//FPS
    public ICameraController CamCon;
    [Space(10)]
    [Header("velocity setting")]
    public float walkSpeed = 2.5f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 3;
    //public float rollVelocity = 1;
    //public float dashVelocity = 1;
    public float attackWeightLerp = 0.1f;
    
    [Space(10)]
    [Header("Camera Setting")]
    public bool isThirdPerson = false;


    [Space(10)]
    [Header("friction setting")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    public IUserInput userInput;
    private Animator modelAnim;
    private Rigidbody rigid;
    private WeaponController wc;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool lockPlanarMove = false;
    //private bool trackDirection = false;
    private CapsuleCollider col;
    private Vector3 deltaPos;

    [SerializeField] private bool canScope;
    [SerializeField] private bool canAttack;
    [SerializeField] private float rotateRate = 0.8f;

    [Space(10)]
    [Header("audio setting")]
    [SerializeField]private AudioSource source;
    [SerializeField] private AudioEventReactor footsteps;

    //public delegate void ActionDelegate();
    //public ActionDelegate OnAction;
    [Header("Editor Setting")]
    private bool lastScope = false;
    void Start()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled == true)
            {
                userInput = input;
                break;
            }
        }
        modelAnim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        source = GetComponent<AudioSource>();
        wc = FPHandle.transform.DeepFind("WeaponHandle").GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        //属性判断:
        if (CheekStateTag("ground"))
        {
            //判断能否Attack
            if (NeedResetModel())
            {
                canScope = false;
            }
            else if (userInput.run)
            {
                canScope = false;
                canAttack = false;
            }
            else
            {
                canScope = true;
                canAttack = WeaponOn();
            }
        }

        //属性判断后对状态机的操作:
        SetWeaponLayerWeight(canAttack);
        //按键控制:
        if (CheekStateTag("ground"))
        {
            if (canScope)
            {
                wc.SetBool("Aim", userInput.scope);
                if(userInput.scope == true && lastScope == false)
                {
                    CamCon.ModifyView(CameraViewMode.aim);
                    lastScope = true;
                }
                else if(userInput.scope == false && lastScope == true)
                {
                    CamCon.ModifyView(CameraViewMode.normal);
                    lastScope = false;
                }
                
            }
            else
            {
                CamCon.ModifyView(CameraViewMode.normal);
                wc.SetBool("Aim", false);
            }

            if (canAttack)
            {
                if (userInput.attack)
                {
                    wc.FireOneShot();
                }
            }

            if (userInput.jump)
            {
                modelAnim.SetTrigger("Jump");
            }
        }


        //切换出武器时，将摄像机回归原位（此处是需要将model归回原位）
        if (NeedResetModel())
        {
            model.transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(model.transform.eulerAngles.y, transform.eulerAngles.y, rotateRate), 0));
            //上面这句还是太简单了，还要判断y值的正负情况做分别(但是暂时没有什么好的办法，但是暂时不需要解决)
            //消除差值
            if (Mathf.Abs(model.transform.eulerAngles.y) <= 10f)
            {
                model.transform.rotation = new Quaternion();
            }
        }
        //人物模型控制
        //控制角色移动
        if (CheekStateTag("ground"))
        {
            modelAnim.SetFloat("forward", Mathf.Lerp(modelAnim.GetFloat("forward"), userInput.Dmag * (userInput.run ? 2.0f : 1.0f), 0.2f));
            modelAnim.SetFloat("right", 0);
            if (!WeaponOn())
            {
                if (userInput.Dmag > 0.2f)
                {
                    model.transform.forward = Vector3.Slerp(model.transform.forward, userInput.Dvec, 0.3f);
                }
            }
            if (!lockPlanarMove)
            {
                planarVec = userInput.Dmag * model.transform.forward * walkSpeed * (userInput.run ? runMultiplier : 1.0f);
            }
            planarVec = userInput.Dvec * walkSpeed * (userInput.run ? runMultiplier : 1.0f);
        }
        //控制角色朝向动画
        if (CheekStateTag("ground"))
        {
            modelAnim.SetFloat("BodyUp", userInput.VerticalView / 90f);
            wc.SetFloat("Velocity", Mathf.Lerp(wc.GetFloat("Velocity"), userInput.Dmag * (userInput.run ? 2.0f : 1.0f), 0.2f));
        }
        //第一人称手臂模型控制
        FPHandle.transform.position = CamCon.transform.position;
        FPHandle.transform.rotation = CamCon.transform.rotation;
    }

    void FixedUpdate()//fps : 1 / 50
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        //clearSignal
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    public bool WeaponOn()
    {
        if(modelAnim.GetInteger("weapon") == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// 检测是否需要调整角色模型
    /// </summary>
    /// <returns></returns>
    public bool NeedResetModel()
    {
        if(WeaponOn() && Mathf.Abs(model.transform.localEulerAngles.y) >= 10f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CanAttack(bool _canAttack)
    {
        canScope = _canAttack;
    }
    
    public void SetWeaponLayerWeight(bool canAttack)
    {
        modelAnim.SetLayerWeight(modelAnim.GetLayerIndex("Weapon"), canAttack ? 1.0f : 0);
    }

    public void SetBool(string name, bool value)
    {
        modelAnim.SetBool(name, value);
    }

    public bool CheekState(string _statename, string _layername = "Base Layer")
    {
        return modelAnim.GetCurrentAnimatorStateInfo(modelAnim.GetLayerIndex(_layername)).IsName(_statename);
    }

    public bool CheekStateTag(string _tagname, string _layername = "Base Layer")
    {
        return modelAnim.GetCurrentAnimatorStateInfo(modelAnim.GetLayerIndex(_layername)).IsTag(_tagname);
    }

    public void IssueTrigger(string triggerName)
    {
        modelAnim.SetTrigger(triggerName);
    }

    public void IsGround()
    {
        modelAnim.SetBool("IsGround", true);
    }

    public void NotGround()
    {
        modelAnim.SetBool("IsGround", false);
    }

    public void OnGroundEnter()
    {
        userInput.inputEnabled = true;
        lockPlanarMove = false;
        col.material = frictionOne;
        //trackDirection = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    //FootStep Audio
    public void PlayFootStep()
    {
        footsteps.Play(source);
    }

}
