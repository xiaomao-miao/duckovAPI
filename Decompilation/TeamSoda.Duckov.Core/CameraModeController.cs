using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Token: 0x020001A3 RID: 419
public class CameraModeController : MonoBehaviour
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0003406D File Offset: 0x0003226D
	private static string filePath
	{
		get
		{
			if (GameMetaData.Instance.Platform == Platform.WeGame)
			{
				return Application.streamingAssetsPath + "/ScreenShots";
			}
			return Application.persistentDataPath + "/ScreenShots";
		}
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0003409C File Offset: 0x0003229C
	private void UpdateInput()
	{
		this.moveInput = this.inputActionAsset["CameraModeMove"].ReadValue<Vector2>();
		this.focusInput = this.inputActionAsset["CameraModeFocus"].IsPressed();
		this.upDownInput = this.inputActionAsset["CameraModeUpDown"].ReadValue<float>();
		this.fovInput = this.inputActionAsset["CameraModeFOV"].ReadValue<float>();
		this.aimInput = this.inputActionAsset["CameraModeAim"].ReadValue<Vector2>();
		this.captureInput = this.inputActionAsset["CameraModeCapture"].WasPressedThisFrame();
		this.fastInput = this.inputActionAsset["CameraModeFaster"].IsPressed();
		this.openFolderInput = this.inputActionAsset["CameraModeOpenFolder"].WasPressedThisFrame();
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x00034184 File Offset: 0x00032384
	private void Awake()
	{
		CameraMode.OnCameraModeActivated = (Action)Delegate.Combine(CameraMode.OnCameraModeActivated, new Action(this.OnCameraModeActivated));
		CameraMode.OnCameraModeDeactivated = (Action)Delegate.Combine(CameraMode.OnCameraModeDeactivated, new Action(this.OnCameraModeDeactivated));
		this.inputActionAsset.Enable();
		this.vCam.gameObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x000341FC File Offset: 0x000323FC
	private void Update()
	{
		if (!this.actived)
		{
			return;
		}
		this.UpdateInput();
		if (this.shootting)
		{
			return;
		}
		this.UpdateMove();
		this.UpdateLook();
		this.UpdateFov();
		if (this.captureInput)
		{
			this.Shot().Forget();
		}
		if (this.openFolderInput)
		{
			CameraModeController.OpenFolder();
			this.openFolderInput = false;
		}
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x0003425D File Offset: 0x0003245D
	private void LateUpdate()
	{
		this.UpdateFocus();
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00034268 File Offset: 0x00032468
	private void UpdateMove()
	{
		Vector3 forward = this.vCam.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		Vector3 right = this.vCam.transform.right;
		right.y = 0f;
		right.Normalize();
		Vector3 a = right * this.moveInput.x + forward * this.moveInput.y;
		a.Normalize();
		a += this.upDownInput * Vector3.up;
		this.vCam.transform.position += Time.unscaledDeltaTime * a * (this.fastInput ? this.fastMoveSpeed : this.moveSpeed);
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00034344 File Offset: 0x00032544
	private void UpdateLook()
	{
		this.pitch += -this.aimInput.y * this.aimSpeed * Time.unscaledDeltaTime;
		this.pitch = Mathf.Clamp(this.pitch, -89.9f, 89.9f);
		this.yaw += this.aimInput.x * this.aimSpeed * Time.unscaledDeltaTime;
		this.vCam.transform.localRotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x000343E0 File Offset: 0x000325E0
	private void UpdateFocus()
	{
		if (this.focusInput)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.vCam.transform.position, this.vCam.transform.forward, out raycastHit, 100f, this.dofLayerMask))
			{
				this.dofTargetPoint = raycastHit.point + this.vCam.transform.forward * -0.2f;
				this.dofTarget.position = this.dofTargetPoint;
			}
			this.focusMeshTimer = this.focusMeshAppearTime;
			if (!this.focusMesh.gameObject.activeSelf)
			{
				this.focusMesh.gameObject.SetActive(true);
			}
		}
		else if (this.focusMeshTimer > 0f)
		{
			this.focusMeshTimer -= Time.unscaledDeltaTime;
			if (this.focusMeshTimer <= 0f)
			{
				this.focusMeshTimer = 0f;
				this.focusMesh.gameObject.SetActive(false);
			}
		}
		if (this.focusMesh.gameObject.activeSelf)
		{
			this.focusMesh.transform.localScale = Vector3.one * this.focusMeshSize * this.focusMeshTimer / this.focusMeshAppearTime;
		}
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00034530 File Offset: 0x00032730
	private void UpdateFov()
	{
		float num = this.vCam.m_Lens.FieldOfView;
		num += -this.fovChangeSpeed * this.fovInput;
		num = Mathf.Clamp(num, this.fovRange.x, this.fovRange.y);
		this.vCam.m_Lens.FieldOfView = num;
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00034590 File Offset: 0x00032790
	private void OnDestroy()
	{
		CameraMode.OnCameraModeActivated = (Action)Delegate.Remove(CameraMode.OnCameraModeActivated, new Action(this.OnCameraModeActivated));
		CameraMode.OnCameraModeDeactivated = (Action)Delegate.Remove(CameraMode.OnCameraModeDeactivated, new Action(this.OnCameraModeDeactivated));
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x000345E0 File Offset: 0x000327E0
	private void OnCameraModeActivated()
	{
		GameCamera instance = GameCamera.Instance;
		if (instance != null)
		{
			CameraArm mianCameraArm = instance.mianCameraArm;
			this.yaw = mianCameraArm.yaw;
			this.pitch = mianCameraArm.pitch;
			this.vCam.transform.position = instance.renderCamera.transform.position;
			this.dofTargetPoint = instance.target.transform.position;
			this.actived = true;
			this.vCam.m_Lens.FieldOfView = instance.renderCamera.fieldOfView;
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00034682 File Offset: 0x00032882
	public static void OpenFolder()
	{
		GUIUtility.systemCopyBuffer = CameraModeController.filePath;
		NotificationText.Push(CameraModeController.filePath ?? "");
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x000346A1 File Offset: 0x000328A1
	private void OnCameraModeDeactivated()
	{
		this.actived = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x000346B8 File Offset: 0x000328B8
	private UniTaskVoid Shot()
	{
		CameraModeController.<Shot>d__44 <Shot>d__;
		<Shot>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Shot>d__.<>4__this = this;
		<Shot>d__.<>1__state = -1;
		<Shot>d__.<>t__builder.Start<CameraModeController.<Shot>d__44>(ref <Shot>d__);
		return <Shot>d__.<>t__builder.Task;
	}

	// Token: 0x04000AAC RID: 2732
	public CinemachineVirtualCamera vCam;

	// Token: 0x04000AAD RID: 2733
	private bool actived;

	// Token: 0x04000AAE RID: 2734
	public Transform dofTarget;

	// Token: 0x04000AAF RID: 2735
	private Vector3 dofTargetPoint;

	// Token: 0x04000AB0 RID: 2736
	public InputActionAsset inputActionAsset;

	// Token: 0x04000AB1 RID: 2737
	public LayerMask dofLayerMask;

	// Token: 0x04000AB2 RID: 2738
	private Vector2 moveInput;

	// Token: 0x04000AB3 RID: 2739
	private float upDownInput;

	// Token: 0x04000AB4 RID: 2740
	private bool focusInput;

	// Token: 0x04000AB5 RID: 2741
	private bool captureInput;

	// Token: 0x04000AB6 RID: 2742
	private bool fastInput;

	// Token: 0x04000AB7 RID: 2743
	private bool openFolderInput;

	// Token: 0x04000AB8 RID: 2744
	public GameObject focusMesh;

	// Token: 0x04000AB9 RID: 2745
	public float focusMeshSize = 0.3f;

	// Token: 0x04000ABA RID: 2746
	private float focusMeshCurrentSize = 0.3f;

	// Token: 0x04000ABB RID: 2747
	public float focusMeshAppearTime = 1f;

	// Token: 0x04000ABC RID: 2748
	private float focusMeshTimer = 0.3f;

	// Token: 0x04000ABD RID: 2749
	private float fovInput;

	// Token: 0x04000ABE RID: 2750
	private Vector2 aimInput;

	// Token: 0x04000ABF RID: 2751
	public float moveSpeed;

	// Token: 0x04000AC0 RID: 2752
	public float fastMoveSpeed;

	// Token: 0x04000AC1 RID: 2753
	public float aimSpeed;

	// Token: 0x04000AC2 RID: 2754
	private float yaw;

	// Token: 0x04000AC3 RID: 2755
	private float pitch;

	// Token: 0x04000AC4 RID: 2756
	private bool shootting;

	// Token: 0x04000AC5 RID: 2757
	public ColorPunch colorPunch;

	// Token: 0x04000AC6 RID: 2758
	public Vector2 fovRange = new Vector2(5f, 60f);

	// Token: 0x04000AC7 RID: 2759
	[Range(0.01f, 0.5f)]
	public float fovChangeSpeed = 10f;

	// Token: 0x04000AC8 RID: 2760
	public CanvasGroup indicatorGroup;

	// Token: 0x04000AC9 RID: 2761
	public UnityEvent OnCapturedEvent;
}
