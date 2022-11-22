using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using UnityEngine.LowLevel;

public class GameManager : MonoBehaviour
{

    // TODO マジックナンバー
    private int                   _startTargetNum = 20;
    
    [SerializeField]
    private IntReactiveProperty _currentScore = new IntReactiveProperty(0);

    [SerializeField] private GameObject              _targetPrefab;
    [SerializeField] private float                   _targetLifetime  = 0.5f;
    private                  int[]                   _targetPositionx = new int[] {10, 15, 20, 25, 30};
    private                  int[]                   _targetPositionz;
    private                  Vector3                 _targetPositionTemp = new Vector3();
    private                  CancellationTokenSource _inGameCts;
    
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

    
    private ReactiveProperty<GameStatusEnum>          _gameStatus = new ReactiveProperty<GameStatusEnum>();
    /// <summary>
    /// 現在のゲームステータス
    /// </summary>
    public  IReadOnlyReactiveProperty<GameStatusEnum> GameStatusRP => _gameStatus;
    
    void Awake()
    {
        // RPのライフタイム管理
        _currentScore.AddTo(this);
        _remainingTarget.AddTo(this);
        _gameStatus.AddTo(this);
        InitScore();
    }

    void Start()
    {
    }
    void InitScore()
    {
        _currentScore.Value = 0;
        _remainingTarget.Value = _startTargetNum;
    }

    public void ChangeGameStatus()
    {
        if (_gameStatus.Value == GameStatusEnum.InProgress)
        {
            CancelGame();
        }
        else if (_gameStatus.Value == GameStatusEnum.NotInProgress)
        {
            StartGame();
        }
    }
    
    async private UniTaskVoid StartGame()
    {
        InitScore();
        // ターゲットを特定の間隔でスポーンさせる
        _gameStatus.Value = GameStatusEnum.InProgress;
        _inGameCts = new CancellationTokenSource();
        TargetLifeTimer(_inGameCts).Forget();
        
        // 的の数が0になったら、ゲームを終了する
        await UniTask.WaitUntil(() => _remainingTarget.Value <= 0, cancellationToken: _inGameCts.Token);
        CancelGame();
    }

    private void CancelGame()
    {
        _inGameCts.Cancel();
        _gameStatus.Value = GameStatusEnum.NotInProgress;
    }

    async UniTaskVoid TargetLifeTimer(CancellationTokenSource cts)
    {
        // ゲーム開始時にはじめのスポーンまでに少し猶予を持たせる
        await UniTask.Delay(TimeSpan.FromSeconds(1.5), cancellationToken: cts.Token);
        
        // キャンセルするまで、定期的に的のスポーンとデスポーンを繰り返す
        
        // TODO なんかもっとやりようがありそう
        // try-catch するのに、defaultを代入しないと下のcatch句のなかで targetを参照できない
        GameObject target = default;
        while (!cts.IsCancellationRequested)
        {
            try
            {
                await UniTask.Delay((int) _targetLifetime * 1000, cancellationToken: cts.Token);
                TargetPositionRandomizer();
                target = Instantiate(_targetPrefab, position:_targetPositionTemp, rotation: default);
            
                // TODO
                // なんかここらへん気持ち悪いなー
                // targetのオブジェクトが消えた瞬間にこのタスクも消えてほしい
                // 2こ同時に出すとかに対応できない
                await UniTask.Delay((int) _targetLifetime * 1000,  cancellationToken: cts.Token);

                if (target)
                {
                    target.GetComponent<Target>().Despawn();
                }
            }
            catch (OperationCanceledException e)
            {
                // 的がスポーンしているタイミングでゲームがキャンセルされると、的がそのまま残り続けてしまうため
                // try- catchで的の削除処理を行う
                if (target)
                {
                    target.GetComponent<Target>().Despawn();
                }
                throw e;
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
    public void AddScore(int num)
    {
        _currentScore.Value += num;
    }

    /// <summary>
    /// 残り的数を増加させる
    /// </summary>
    /// <param name="num"></param>
    public void AddRemaining(int num)
    {
        _remainingTarget.Value += num;
    }
}
