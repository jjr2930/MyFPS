using UnityEngine;
using System.Collections.Generic;
using JLib;

[System.Serializable]
public class MappingData
{
    public KeyCode key;
    public VK_Enum virtualKey;
}
public class InputMapper : MonoBehaviour
{

    [SerializeField]
    List<MappingData> mappingList = new List<MappingData>();

    Dictionary<KeyCode,VK_Enum> dicMap = new Dictionary<KeyCode, VK_Enum>();
    // Use this for initialization
    void Start()
    {
        for( int i = 0 ; i < mappingList.Count ; i++ )
        {
            dicMap.Add( mappingList[ i ].key , mappingList[ i ].virtualKey );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.anyKeyDown )
        {
            for( int i = 0 ; i < mappingList.Count ; i++ )
            {
                if(Input.GetKeyDown(mappingList[i].key))
                {
                    
                }
            }
        }
    }
}
