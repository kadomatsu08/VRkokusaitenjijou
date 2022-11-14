using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 弾を意味するベースクラス
/// </summary>
public abstract class BulletBase : MonoBehaviour
{
    // 秒単位
    [SerializeField] private int _destroyTimeLimit = 5;
    
    void Awake()
    {
        DestroyTimer(new GameObject().GetCancellationTokenOnDestroy()).Forget();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 弾オブジェクトを指定時間後にDestroyする
    /// </summary>
    /// <param name="ct"></param>
    async UniTaskVoid DestroyTimer(CancellationToken ct)
    {
        await UniTask.Delay(_destroyTimeLimit * 1000, cancellationToken:ct);
        Destroy(this.gameObject);
    }
}
