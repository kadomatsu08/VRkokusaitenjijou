using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEditor.U2D;

/// <summary>
/// 弾を意味するベースクラス
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public abstract class BulletBase : MonoBehaviour
{
    // 弾の生存時間(秒単位)
    [SerializeField] private int                     _destroyTimeLimit = 5;
    private                  Rigidbody               _rigidbody;
    private                  float                   _muzzleVelocity;
    protected                bool                    _doSetMazzleVelocity = false;
    private                  CancellationToken _ct;

    private void OnConnectedToServer()
    {
        throw new NotImplementedException();
    }

    public float MuzzleVelocity
    {
        get => _muzzleVelocity;
        set => _muzzleVelocity = value;
    }


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ct = this.GetCancellationTokenOnDestroy();
        
        // 一定時間後に弾をdestroyする
        DestroyTimer(_ct).Forget();
        // 弾が何かにあたったときdestroyする
        OnHit(_ct).Forget();

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

    /// <summary>
    /// ものにあたったときの挙動を定義する
    /// </summary>
    async protected UniTaskVoid OnHit(CancellationToken ct)
    {
        // 物にあたった場合、弾を削除する
        // 貫通とか全く考慮していない
        await this.GetAsyncCollisionEnterTrigger().OnCollisionEnterAsync(ct);
        Destroy(this.gameObject);
    }


}
