using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// XRRaycastinteracterのLineRendererの表示/非表示制御を行う
/// </summary>
[RequireComponent(typeof(XRRayInteractor))]
[RequireComponent(typeof(LineRenderer))]
public class HandRaycastController : MonoBehaviour, IDisposable
{
    [SerializeField]
    private XRRayInteractor _rayInteractor;

    [SerializeField] private XRInteractorLineVisual _interactorLineVisual;
    private                  float                  _templineLength;
    private                  float                  _templineWidth;

    
    private void Start()
    {
        _rayInteractor.selectEntered.AddListener(DisableLineRenderer);
        _rayInteractor.selectExited.AddListener(EnableLineRenderer);
        _templineLength = _interactorLineVisual.lineLength;
        _templineWidth = _interactorLineVisual.lineWidth;
    }

    private void EnableLineRenderer(SelectExitEventArgs args)
    {
        _interactorLineVisual.lineLength = _templineLength;
        _interactorLineVisual.lineWidth = _templineWidth;
    }
    
    private void DisableLineRenderer(SelectEnterEventArgs args)
    {
        _templineLength = _interactorLineVisual.lineLength;
        _templineWidth = _interactorLineVisual.lineWidth;
        _interactorLineVisual.lineLength = 0;
        _interactorLineVisual.lineWidth = 0;
    }
    
    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        _rayInteractor.selectEntered.RemoveListener(DisableLineRenderer);
        _rayInteractor.selectExited.RemoveListener(EnableLineRenderer);
    }

}
