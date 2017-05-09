using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireComponent : MonoBehaviour
{
    public Material material = null;
    public float size = 128f;
    public int count = 8;
    public int currentIndex = 0;
    public float frameDelay = 0.1f;
    IEnumerator itor = null;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Start()
    {
        JLib.GlobalEventQueue.RegisterListener( E_Event.Fire , ListenFire );
    }

    void OnDistroy()
    {
        JLib.GlobalEventQueue.RemoveListener( E_Event.Fire , ListenFire );
    }
    void ListenFire(object p)
    {
        gameObject.SetActive( true );
    }
    void OnEnable()
    {
        material.SetTextureOffset( "_MainTex" , new Vector2( 0f , 0f ) );
        itor = Anim();
        StartCoroutine( itor );
    }

    void OnDisable()
    {
        StopCoroutine( itor );
    }

    IEnumerator Anim()
    {
        for(int i = 0 ; i<8 ; i++)
        {
            yield return new WaitForSeconds( frameDelay );
            currentIndex = ( currentIndex + 1 ) % count;
            float y = 128f * currentIndex / 1028f;
            material.SetTextureOffset( "_MainTex" , new Vector2( 0f , y ) );
        }
        gameObject.SetActive( false );
    }
}
