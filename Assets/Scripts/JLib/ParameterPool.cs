using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib
{
    public class ParameterPool : Singletone<ParameterPool>
    {
        List<object> sleepObject = new List<object>();
        List<object> awakeObject = new List<object>();
        public T GetObject<T>() where T : class
        {
            for( int i = 0 ; i < sleepObject.Count ; i++ )
            {
                T check = sleepObject[i] as T;
                if(null != check)
                {
                    sleepObject.RemoveAt(i);
                    awakeObject.Add(check);
                    return check;
                }
            }

            //이곳에 도달했다면 없는것임
            return null;
        }

        public void ReturnPool(object obj)
        {
            if(awakeObject.Remove(obj))
            {
                sleepObject.Add(obj);
            }
        }
    }
}
