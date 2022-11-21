using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class StartCancelButton : MonoBehaviour
{
    private                  GameManager  _gameManager;
    [SerializeField] private Material     _cancel;
    [SerializeField] private Material     _start;
    private                  MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _meshRenderer.material = _start;
    }

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameManager.GameStatusRP
            .Subscribe(x => UpdateStatusColor(x))
            .AddTo(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _gameManager.ChangeGameStatus();
    }

    private void UpdateStatusColor(GameStatusEnum gameStatus)
    {
        if (gameStatus == GameStatusEnum.InProgress)
        {
            _meshRenderer.material = _cancel;
        }
        else if (gameStatus == GameStatusEnum.NotInProgress)
        {
            _meshRenderer.material = _start;
        }
    }
}
