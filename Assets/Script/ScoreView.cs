using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UniRx;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private GameManager     _gameManager;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _remainingText;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager.CurrentScoreRP
            .Subscribe(x => UpdateScore(x))
            .AddTo(this);
        
        _gameManager.RemainingTargetRP
            .Subscribe(x => UpdateRemaining(x))
            .AddTo(this);
    }

    private void UpdateScore(int x)
    {
        _scoreText.text = StringFormatter(x);
    }
    
    private void UpdateRemaining(int x)
    {
        _remainingText.text = StringFormatter(x);
    }
    
    private string StringFormatter(int num)
    {
        // 2桁で0うめの数値を返す
        return ZString.Format("{0:D2}", num);
    }
}
