using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 弾を意味するベースクラス
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public abstract class BulletBase : MonoBehaviour
{
    // 弾の生存時間(秒単位)
    [SerializeField] private int       _destroyTimeLimit = 5;
    private                  Rigidbody _rigidbody;
    private                float     _muzzleVelocity;
    protected                bool      _doSetMazzleVelocity = false;

    public float MuzzleVelocity
    {
        get => _muzzleVelocity;
        set => _muzzleVelocity = value;
    }


    void Awake()
    {
        DestroyTimer(this.gameObject.GetCancellationTokenOnDestroy()).Forget();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (!_doSetMazzleVelocity)
        {
            _rigidbody.AddForce(transform.forward * _muzzleVelocity,ForceMode.Impulse);
            _doSetMazzleVelocity = true;
        }
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

    /// <summary>
    /// 弾のPrefabを生成するクラス
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="speed">弾の初速</param>
    /// <returns></returns>
    public static BulletBase Instantiate(BulletBase prefab, Vector3 position, Quaternion rotation, float speed)
    {
        BulletBase obj = Instantiate(prefab, position, rotation) as BulletBase;
        obj._muzzleVelocity = speed;
        return obj;
    }
}
