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
    [SerializeField] private float      _targetLifetime  = 0.5f;
    private                  int[]      _targetPositionx = new int[] {10, 15, 20, 25, 30};
    private                  int[]      _targetPositionz;
    private                  Vector3    _targetPositionTemp = new Vector3();
    
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
    void InitScore()
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
            TargetPositionRandomizer();
            var target = Instantiate(_targetPrefab, position:_targetPositionTemp, rotation: default);
            
            // TODO
            // なんかここらへん気持ち悪いなー
            // targetのオブジェクトが消えた瞬間にこのタスクも消えてほしい
            // 2こ同時に出すとかに対応できない
            await UniTask.Delay((int) _targetLifetime * 1000);

            if (target)
            {
                target.GetComponent<Target>().Despawn();
            }
        }
    }

    //TODO クラス変数をいじってるのキモい
    private void TargetPositionRandomizer()
    {
        var vector3 = new Vector3();
        vector3.x = UnityEngine.Random.Range(-5, 5);
        vector3.y = 0;
        vector3.z = _targetPositionx[UnityEngine.Random.Range(0, _targetPositionx.Length)];
           

        _targetPositionTemp = vector3;
    }

    /// <summary>
    /// スコアを増加させる
    /// </summary>
    /// <param name="num"></param>
    public void addScore(int num)
    {
        _currentScore.Value += num;
    }

    /// <summary>
    /// 残り的数を増加させる
    /// </summary>
    /// <param name="num"></param>
    public void addRemaining(int num)
    {
        _remainingTarget.Value += num;
    }
}
