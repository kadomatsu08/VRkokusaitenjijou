using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{

    // TODO マジックナンバー
    private int                   _startTargetNum = 20;
    
    [SerializeField]
    private IntReactiveProperty _currentScore = new IntReactiveProperty(0);

    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private float      _targetLifetime = 2f;
    
    /// <summary>
    /// 現在のスコアのReactiveProperty
    /// </summary>
    public IReadOnlyReactiveProperty<int> CurrentScoreRP => _currentScore;
    
    [SerializeField]
    private IntReactiveProperty _remainingTarget = new IntReactiveProperty(0);

    /// <summary>
    /// 現在の残り的数のReactiveProperty
    /// </summary>
    public IReadOnlyReactiveProperty<int> RemainingTargetRP => _remainingTarget;

    void Awake()
    {
        // RPのライフタイム管理
        _currentScore.AddTo(this);
        _remainingTarget.AddTo(this);
        InitScore();
    }

    void Start()
    {
        StartGame();
    }
    private void InitScore()
    {
        _currentScore.Value = 0;
        _remainingTarget.Value = _startTargetNum;
    }

    void StartGame()
    {
        InitScore();
        // ターゲットを特定の間隔でスポーンさせる
        
        var targetCts = new CancellationTokenSource();
        TargetLifeTimer(targetCts).Forget();

    }

    async UniTaskVoid TargetLifeTimer(CancellationTokenSource token)
    {
        // キャンセルするまで、定期的に的のスポーンとデスポーンを繰り返す
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay((int) _targetLifetime * 1000);
            
            var target = Instantiate(_targetPrefab);
            
            await UniTask.Delay((int) _targetLifetime * 1000);

            if (target)
            {
                target.GetComponent<Target>().Despawn();
            }
        }
    }
}
