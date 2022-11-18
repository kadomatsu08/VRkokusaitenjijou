using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Android;

[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] private AudioClip   _spawnSound;
    [SerializeField] private AudioClip   _despawnSound;

    private AudioSource _audioSource;

    
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_spawnSound);
    }
    
    /// <summary>
    /// ターゲットの子オブジェクトに衝突判定が生じたときに発火する
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(AudioRandomizer(_hitSounds), this.transform.position);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 自然消滅するときに呼び出すメソッド
    /// </summary>
    public void Despawn()
    {
        AudioSource.PlayClipAtPoint(_despawnSound , this.transform.position);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// audioClipのランダム再生を行う
    /// </summary>
    /// <param name="audios"></param>
    /// <returns></returns>
    private AudioClip AudioRandomizer(AudioClip[] audios)
    {
        return audios[UnityEngine.Random.Range(0, audios.Length)];
    }
}
