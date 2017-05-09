using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib;

public class ShotParameter
{
    public GameObject shotObject;
    public Vector3 shotPoint;
    public float gunDmg;
}


/// <summary>
/// 총이 하는 일에 대해 정의 되어져 있음
/// </summary>
public class GunComponent : JMonoBehaviour
{
    public Transform firePosition = null;
    [Tooltip("분당 발사 속도")]
    public float rpm = 600f;
    [Tooltip("남은 총알의 수")]
    public int remainBullet = 50;
    [Tooltip("총의 데미지")]
    public float gunDamage = 10f;
    [Tooltip("총의 정확도")]
    [Range(1f,2f)]
    public float gunAccuracy = 1.2f;
    public LayerMask enemyLayer;

    float beforeTime = 0f;
    float intervalTime = 0f;

    Camera camera = null;
    Transform camTrans = null;
    Ray ray = new Ray();
    RaycastHit hit = new RaycastHit();
    void Awake()
    {
        camera = Camera.main;
        camTrans = Camera.main.transform;
        CalculateRpm();
        GlobalEventQueue.RegisterListener( VK_State.Down , ListenFireEvent );
        GlobalEventQueue.RegisterListener( VK_State.Press , ListenFireEvent );
    }

    void OnEnable()
    {
        FloatParameter fparam = ParameterPool.GetParameter<FloatParameter>();
        fparam.value = gunAccuracy;
        GlobalEventQueue.EnQueueEvent( E_Event.GunChange , fparam );
    }

    void CalculateRpm()
    {
        float rps = rpm / 60f;
        intervalTime = 1 / rps;
    }

    public void ListenFireEvent( object param )
    {
        VirtualKeyParameter p = param as VirtualKeyParameter;
        if( VK_Enum.VK_Button1 != p.key )
        {
            return;
        }

        bool isCoolTime = beforeTime > Time.realtimeSinceStartup;
        bool isHaveBullet = remainBullet > 0;
        if( !isCoolTime && isHaveBullet )
        {
            GlobalEventQueue.EnQueueEvent( E_Event.Fire , null );
            beforeTime = Time.realtimeSinceStartup + intervalTime;
            --remainBullet;
            ShootRaycast();
        }
    }

    void OnDistroy()
    {
        GlobalEventQueue.RemoveListener( VK_State.Down , ListenFireEvent );
        GlobalEventQueue.RemoveListener( VK_State.Press , ListenFireEvent );
    }

    void ShootRaycast()
    {
        ray.origin = camTrans.position;
        ray.direction = camTrans.forward;

        if( Physics.Raycast( ray , out hit , enemyLayer.value ) )
        {
            ShotParameter sp = ParameterPool.GetParameter<ShotParameter>();
            sp.gunDmg = gunDamage;
            sp.shotObject = hit.transform.gameObject;
            sp.shotPoint = hit.point;
            GlobalEventQueue.EnQueueEvent( E_Event.Shot , sp );

            EffectParameter ep = ParameterPool.GetParameter<EffectParameter>();
            ep.effectName = "Prefabs/Decals/Bullet Hole";
            ep.parent = hit.transform;
            ep.position = hit.point + hit.normal * 0.005f;
            ep.rotation = Quaternion.LookRotation( -hit.normal );
            ep.scale = Vector3.one * 0.1f;
            GlobalEventQueue.EnQueueEvent( DefaultEvent.ShowEffect , ep );

            //Debug.LogFormat( "SHOT {0}" , hit.transform.name );
        }
    }
}
