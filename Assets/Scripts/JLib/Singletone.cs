using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace JLib
{
    public class Singletone<T> where T : class, new()
    {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                Initialize();
                return _instance;
            }
        }

        public static void Initialize()
        {
            if (null == _instance)
            {
                Debug.LogFormat("Create {0}",typeof(T).ToString());
                _instance = new T();
            }
        }
    }
}
