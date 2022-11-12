using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;


namespace VrKokusaitenjijo.Tools
{
    /// <summary>
    /// XRIT用のControllerState監視スクリプト
    /// </summary>
    [RequireComponent(typeof(XRBaseController))]
    
    public class ControllerStateWatcher : MonoBehaviour
    {
        [SerializeField] private XRBaseController _leftController;
        [SerializeField] private XRBaseController _rightController;

        private XRControllerState _leftControllerState;
        private XRControllerState _rightControllerState;
        
        // Start is called before the first frame update
        void Start()
        {
            _leftControllerState = _leftController.currentControllerState;
            _rightControllerState = _rightController.currentControllerState;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(_leftControllerState.ToString());
            Debug.Log(_rightControllerState.ToString());
        }
    }
}
