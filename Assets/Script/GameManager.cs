using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{

    // TODO マジックナンバー
    private int                   _startTargetNum = 20;
    
    [SerializeField]
    private IntReactiveProperty _currentScore;
    /// <summary>
    /// 現在のスコアのReactiveProperty
    /// </summary>
    public IReadOnlyReactiveProperty<int> CurrentScoreRP
    {
        get { return _currentScore; }
    }
    
    [SerializeField]
    private IntReactiveProperty _remainingTarget;
    /// <summary>
    /// 現在の残り的数のReactiveProperty
    /// </summary>
    public IReadOnlyReactiveProperty<int> RemainingTargetRP
    {
        get { return _remainingTarget; }
    }

    void Awake()
    {
        // RPの初期化
        _currentScore = new IntReactiveProperty(0);
        _remainingTarget = new IntReactiveProperty(_startTargetNum);
    }

    private void InitScore()
    {
        _currentScore.Value = 0;
        _remainingTarget.Value = _startTargetNum;
    }

    void StartGame()
    {
        
        // ターゲットを特定の間隔で
        
    }
}
