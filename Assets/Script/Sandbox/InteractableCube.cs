using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.XR.Interaction.Toolkit;

namespace VrKokusaitenjijo.SandBox
{
    /// <summary>
    /// Sandboxシーン用スクリプト インタラクトできるものにアタッチする
    /// </summary>
    [RequireComponent(typeof(XRBaseInteractable))]
    public class InteractableCube : MonoBehaviour , IDisposable
    {
        [SerializeField] private XRBaseInteractable _baseInteractable;
        
        // Start is called before the first frame update
        void Start()
        {
            _baseInteractable.selectEntered.AddListener(DebugEvent);
            _baseInteractable.selectExited.AddListener(DebugEvent);
            _baseInteractable.activated.AddListener(DebugEvent);
            _baseInteractable.deactivated.AddListener(DebugEvent);

            // DoAsync().Forget();
        }

        async UniTask DoAsync()
        {
            await Task.Delay(10000, this.GetCancellationTokenOnDestroy());
            Destroy(gameObject);
        }

        void DebugEvent(BaseInteractionEventArgs args)
        {
            Debug.Log(args.ToString());
        }

        void RemoveEventListeners()
        {
            _baseInteractable.selectEntered.RemoveListener(DebugEvent);
            _baseInteractable.selectExited.RemoveListener(DebugEvent);
            _baseInteractable.activated.RemoveListener(DebugEvent);
            _baseInteractable.deactivated.RemoveListener(DebugEvent);
        }
        
        public void Dispose()
        {
            RemoveEventListeners();
            Debug.Log("Dispose Listener");
        }
        void OnDestroy()
        {
            Dispose();
            Debug.Log("OnDestroy");
        }
    }
}
