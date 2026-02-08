using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    //渐隐时间
    public const float fadeDuration= 0.35f;
    //渐隐透明度
    public const float targetAlpha = 0.45f;

    //时间缩放 0.01f ： 游戏内1秒钟等于100分之一秒
    public const float secondThreshold = 0.05f;

    public const int secondHold = 59;

    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int monthHold = 11;
    public const int seasonHold = 3;

}
