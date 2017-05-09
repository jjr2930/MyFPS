using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib;
[RequireComponent( typeof( TweenRectSize ) )]
public class CrossHair : JMonoBehaviour
{

    public const float DEFAULT_SIZE = 50;
    [SerializeField]
    float SIZE_LIMIT = 0;
    TweenRectSize tween = null;
    float accuracy = 0f;
    void Awake()
    {
        tween = GetComponent<TweenRectSize>();
        GlobalEventQueue.RegisterListener( E_Event.GunChange , ListenGunChange );
        GlobalEventQueue.RegisterListener( E_Event.Fire , ListenFire );
        GlobalEventQueue.RegisterListener( VK_State.Up , ListenFireStop );
    }

    void OnDestroy()
    {
        GlobalEventQueue.RemoveListener( E_Event.GunChange , ListenGunChange );
        GlobalEventQueue.RemoveListener( E_Event.Fire , ListenFire );
        GlobalEventQueue.RemoveListener( VK_State.Up , ListenFireStop );
    }
	
    //총바꿈
    public void ListenGunChange(object param)
    {
        FloatParameter p = param as FloatParameter;
        accuracy = p.value;
        float size = DEFAULT_SIZE * accuracy;
        rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical , size );
        rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, size );
    }

    /// <summary>
    /// 발사!!
    /// </summary>
    /// <param name="param"></param>
    public void ListenFire(object param)
    {
        float curWidth = rectTransform.rect.width;
        float curHeight = rectTransform.rect.height;
        float nextWidth = Mathf.Clamp( curWidth * accuracy,DEFAULT_SIZE * accuracy,SIZE_LIMIT);
        float nextHeight =  Mathf.Clamp( curWidth * accuracy,DEFAULT_SIZE * accuracy,SIZE_LIMIT);

        tween.Set( new Vector2( curWidth , curHeight ) , new Vector2( nextWidth , nextHeight ) );
        tween.PlayFromBegin();
        //Debug.Log( "Fire start" );
    }

    /// <summary>
    /// 발사가 끝났다
    /// </summary>
    /// <param name="param"></param>
    public void ListenFireStop(object param)
    {
        VirtualKeyParameter p = param as VirtualKeyParameter;
        if( VK_Enum.VK_Button1 != p.key ) 
        {
            return;
        }

        float curWidth = rectTransform.rect.width;
        float curHeight = rectTransform.rect.height;
        float nextWidth = DEFAULT_SIZE * accuracy;
        float nextHeight = DEFAULT_SIZE * accuracy;

        tween.Set( new Vector2( curWidth , curHeight ) , new Vector2( nextWidth , nextHeight ) );
        tween.PlayFromBegin();
        //Debug.Log( "Fire stop" );
    }
}
