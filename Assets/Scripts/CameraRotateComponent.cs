using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JLib;
public class CameraRotateComponent : JMonoBehaviour
{
    [SerializeField]
        List<CharacterControllerEventData> mappingList
            = new List<CharacterControllerEventData>();

        [SerializeField]
        float moveSpeed  = 0f;

        [SerializeField]
        float rotateSpeed = 0f;

        [SerializeField]
        float jumpForce = 10f;


        Vector3 jumpAccel = Vector3.zero;

        float pitchDirection = 0f;
        float yawDirecction = 0f;
        float rollDirection = 0f;

        float pitchFactor = 0f;
        float yawFactor = 0f;
        float rollFactor = 0;

        public bool IsGrounded = false;
        Vector3 moveAccel = Vector3.zero;


        Dictionary<VK_Enum, Action<float>> PressListeners
            = new Dictionary<VK_Enum,Action<float>>();

        Dictionary<VK_Enum, Action<float>> UpListeners
            = new Dictionary<VK_Enum,Action<float>>();

        Dictionary<VK_Enum, Action<float>> DownListeners
            = new Dictionary<VK_Enum,Action<float>>();

        void Awake()
        {
            MappingVirtualKey();
            RegisterListener();
        }

        void Update()
        {
            moveAccel *= moveSpeed * JTime.DeltaTime;

            transform.Translate( moveAccel );

            Rotate( );
        }
        
        void LateUpdate()
        {
            //V3Extension.SetZero( ref moveAccel );            
        }

        void MappingVirtualKey()
        {
            for( int i = 0 ; i < mappingList.Count ; i++ )
            {
                Action<float> action = null;
                VK_Enum key = mappingList[i].key;
                Dictionary<VK_Enum,Action<float>> foundedDic = null;
                switch( mappingList[ i ].state )
                {
                    case VK_State.Down:
                        foundedDic = DownListeners;
                        break;

                    case VK_State.Press:
                        foundedDic = PressListeners;
                        break;

                    case VK_State.Up:
                        foundedDic = UpListeners;
                        break;
                }

                switch( mappingList[ i ].method )
                {
                    case CharacterMethod.Back:
                        action = Back;
                        break;

                    case CharacterMethod.Forward:
                        action = Forward;
                        break;

                    case CharacterMethod.Jump:
                        action = Jump;
                        break;

                    case CharacterMethod.Left:
                        action = Left;
                        break;

                    case CharacterMethod.Right:
                        action = Right;
                        break;

                    case CharacterMethod.PitchCounterClock:
                        action = PitchCounterClock;
                        break;

                    case CharacterMethod.PitchClock:
                        action = PitchClock;
                        break;

                    case CharacterMethod.YawClock:
                        action = YawClock;
                        break;

                    case CharacterMethod.YawCounterClock:
                        action = YawCounterClock;
                        break;

                    case CharacterMethod.RollClock:
                        action = RollClock;
                        break;

                    case CharacterMethod.RollCounterClock:
                        action = RollCounterClock;
                        break;

                }

                foundedDic.Add( key , action );
            }
        }

        void RegisterListener()
        {
            GlobalEventQueue.RegisterListener( VK_State.Press , ListenPress );
            GlobalEventQueue.RegisterListener( VK_State.Down , ListenDown );
            GlobalEventQueue.RegisterListener( VK_State.Up , ListenUp );
        }

        void ListenPress( object param )
        {
            VirtualKeyParameter p = param as VirtualKeyParameter;

            Action<float> action = null;
            if( PressListeners.TryGetValue( p.key , out action  ) )
            {
                action(p.additionalData);
            }
        }

        void ListenDown( object param)
        {
            VirtualKeyParameter p = param as VirtualKeyParameter;
            
            Action<float> action = null;
            if( DownListeners.TryGetValue( p.key, out action ) )
            {
                action(p.additionalData);
            }
        }

        void ListenUp( object param )
        {
            VirtualKeyParameter p = param as VirtualKeyParameter;

            Action<float> action = null;
            if( UpListeners.TryGetValue( p.key, out action ) )
            {
                action(p.additionalData);
            }
        }

        void Rotate()
        {
            //pitch
            transform.Rotate( Vector3.right, rotateSpeed * pitchDirection * pitchFactor * JTime.DeltaTime );

            //yaw
            transform.Rotate( Vector3.up , rotateSpeed * yawDirecction * yawFactor * JTime.DeltaTime );

            //roll
            transform.Rotate( Vector3.forward , rotateSpeed * rollDirection * rollFactor * JTime.DeltaTime );

            pitchDirection = 0f;
            yawDirecction = 0f;
            rollDirection = 0f;
        }
        
        void Forward(float value)
        {
            V3Extension.Add( ref moveAccel , transform.forward );
        }

        void Back(float value)
        {
            V3Extension.Subtract( ref moveAccel , transform.forward );
        }

        void Left(float value)
        {
            V3Extension.Subtract( ref moveAccel , transform.right );
        }

        void Right(float value)
        {
            V3Extension.Add( ref moveAccel , transform.right );
        }

        void Jump(float value)
        {
            jumpAccel = - Physics.gravity.normalized * jumpForce;
        }

        
        void PitchClock(float value)
        {
            pitchDirection = 1f ;
            pitchFactor = Mathf.Abs( value );
        }

        void PitchCounterClock(float value)
        {
            pitchDirection = -1f;
            pitchFactor = Mathf.Abs( value );
        }


        void YawClock(float value)
        {
            yawDirecction = 1f;
            yawFactor = Mathf.Abs( value );
        }
        
        void YawCounterClock(float value)
        {
            yawDirecction = -1f;
            yawFactor = Mathf.Abs( value );
        }

        void RollClock(float value)
        {
            rollDirection = 1f;
            rollFactor = Mathf.Abs( value );
        }

        void RollCounterClock(float value)
        {
            rollDirection = -1f;
            rollFactor = Mathf.Abs( value );
        }
    
}

