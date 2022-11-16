using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Android;


public class Target : MonoBehaviour
{
    /// <summary>
    /// ターゲットの子オブジェクトに衝突判定が生じたときに発火する
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
