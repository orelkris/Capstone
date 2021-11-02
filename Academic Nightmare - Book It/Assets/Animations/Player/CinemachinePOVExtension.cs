using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    public StarterAssetsInputs input;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {

    }
}
