using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class RespawnOnDrop : MonoBehaviour
{
    [SerializeField] private float _yThresholdForRespawn;

    private Vector3    _initialPosition;
    private Quaternion _initialRotation;
    private Vector3    _initialScale;
    private Transform  _transform;
    private void Awake()
    {
        _transform = this.gameObject.transform;
        _initialPosition = _transform.position;
        _initialRotation = _transform.rotation;
        _initialScale = _transform.localScale;
        var            ct = gameObject.GetCancellationTokenOnDestroy();
        DropOnFloor(ct).Forget();
    }

    async UniTaskVoid DropOnFloor(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitUntil(() => _transform.position.y < _yThresholdForRespawn, cancellationToken: ct);
            _transform.position = _initialPosition;
            _transform.rotation = _initialRotation;
            _transform.localScale = _initialScale;
        }
    }
}
