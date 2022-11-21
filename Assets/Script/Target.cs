using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.Android;

[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    [SerializeField] private AudioClip[]    _hitSounds;
    [SerializeField] private AudioClip      _spawnSound;
    [SerializeField] private AudioClip      _despawnSound;
    private                  GameManager    _gameManager;
    private                  AudioSource    _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_spawnSound);
        _audioSource.minDistance = 100;
        //TODO FIndで呼び出しているの気持ち悪いし、ゲームマネージャーに依存しているのもキモチワル
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }
    
    /// <summary>
    /// ターゲットの子オブジェクトに衝突判定が生じたときに発火する
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //TODO 弾以外のものがあたってもこれが呼ばれてしまう
        _gameManager.AddScore(1);
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

    private void OnDestroy()
    {
        _gameManager.AddRemaining(-1);
    }
}
