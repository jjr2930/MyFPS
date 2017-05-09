using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum E_Event
{
    None,
    /// <summary>
    /// 발사에 성공하였다.
    /// </summary>
    Fire,   
    /// <summary>
    /// 무언가를 맞추었다.
    /// </summary>
    Shot,   
    /// <summary>
    /// 총을 변경했다.
    /// </summary>
    GunChange,
}
