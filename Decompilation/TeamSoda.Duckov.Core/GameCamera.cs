using System;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x0200004B RID: 75
public class GameCamera : MonoBehaviour
{
	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060001CF RID: 463 RVA: 0x00008ACC File Offset: 0x00006CCC
	public static GameCamera Instance
	{
		get
		{
			LevelManager instance = LevelManager.Instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GameCamera;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x00008ADE File Offset: 0x00006CDE
	private bool sickProtectModeOn
	{
		get
		{
			return DisableCameraOffset.disableCameraOffset;
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00008AE8 File Offset: 0x00006CE8
	private void Start()
	{
		this.currentFov = this.mainVCam.m_Lens.FieldOfView;
		if (!this.dofComponent && this.mainVCam)
		{
			VolumeProfile profile = this.mainVCam.GetComponent<CinemachineVolumeSettings>().m_Profile;
			if (!profile)
			{
				return;
			}
			profile.TryGet<DepthOfField>(out this.dofComponent);
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00008B4C File Offset: 0x00006D4C
	private void Update()
	{
		if (!this.target)
		{
			return;
		}
		this.volumeProxy.position = this.target.transform.position;
		this.mainCamDepthPoint.position = this.target.GetCurrentAimPoint();
		this.gun = this.target.GetGun();
		if (!this.inputManager)
		{
			this.inputManager = LevelManager.Instance.InputManager;
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00008BC8 File Offset: 0x00006DC8
	private void LateUpdate()
	{
		if (!this.target)
		{
			return;
		}
		float num = this.adsValue;
		if (this.gun)
		{
			this.adsValue = this.target.AdsValue;
			this.maxAimOffset = Mathf.Lerp(this.sickProtectModeOn ? 0f : this.defaultAimOffset, this.defaultAimOffset * this.gun.ADSAimDistanceFactor, this.adsValue);
			this.aimOffsetDistanceFactor = Mathf.Lerp(this.defaultAimOffsetDistanceFactor, this.defaultAimOffsetDistanceFactor * this.gun.ADSAimDistanceFactor, this.adsValue);
		}
		else
		{
			this.adsValue = Mathf.MoveTowards(this.adsValue, this.target.IsInAdsInput ? 1f : 0f, Time.deltaTime * 10f);
			this.maxAimOffset = Mathf.Lerp(this.sickProtectModeOn ? 0f : this.defaultAimOffset, this.defaultAimOffset * 1.25f, this.adsValue);
			this.aimOffsetDistanceFactor = Mathf.Lerp(this.defaultAimOffsetDistanceFactor, this.defaultAimOffsetDistanceFactor, this.adsValue);
		}
		this.adsChanging = (num != this.adsValue);
		this.UpdateFov(Time.deltaTime);
		this.UpdatePosition(Time.deltaTime);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00008D1C File Offset: 0x00006F1C
	public void UpdateFov(float deltaTime)
	{
		float num = Mathf.Lerp(this.defaultFOV, this.adsFOV, this.adsValue);
		if (this.currentFov != num)
		{
			this.currentFov = num;
			this.mainVCam.m_Lens.FieldOfView = this.currentFov;
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00008D67 File Offset: 0x00006F67
	public void ForceSyncPos()
	{
		this.UpdatePosition(1f);
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00008D74 File Offset: 0x00006F74
	public void SetTarget(CharacterMainControl _target)
	{
		this.target = _target;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00008D80 File Offset: 0x00006F80
	private void UpdateCameraVectors()
	{
		this.cameraForwardVector = this.renderCamera.transform.forward;
		this.cameraForwardVector.y = 0f;
		this.cameraForwardVector.Normalize();
		this.cameraRightVector = this.renderCamera.transform.right;
		this.cameraRightVector.y = 0f;
		this.cameraRightVector.Normalize();
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00008DF0 File Offset: 0x00006FF0
	public void UpdatePosition(float deltaTime)
	{
		this.UpdateCameraVectors();
		if (this.target)
		{
			if (this.inputManager)
			{
				GameCamera.CameraAimingTypes cameraAimingTypes = this.cameraAimingType;
				if (cameraAimingTypes != GameCamera.CameraAimingTypes.normal)
				{
					if (cameraAimingTypes == GameCamera.CameraAimingTypes.bounds)
					{
						this.UpdateAimOffsetUsingBound(deltaTime);
					}
				}
				else
				{
					this.UpdateAimOffsetNormal(deltaTime);
				}
			}
			Vector3 b = this.cameraForwardVector * this.offsetFromTargetZ + this.cameraRightVector * this.offsetFromTargetX;
			this.virtualTarget = this.target.transform.position + b + Vector3.up * 0.5f;
			Vector3.Distance(base.transform.position, this.virtualTarget);
			base.transform.position = this.virtualTarget;
			Action<GameCamera, CharacterMainControl> onCameraPosUpdate = GameCamera.OnCameraPosUpdate;
			if (onCameraPosUpdate != null)
			{
				onCameraPosUpdate(this, this.target);
			}
			if (this.dofComponent)
			{
				this.dofComponent.focusDistance.value = Vector3.Distance(this.renderCamera.transform.position, this.target.transform.position) - 1.5f;
			}
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00008F28 File Offset: 0x00007128
	private void UpdateAimOffsetNormal(float deltaTime)
	{
		float num = Mathf.InverseLerp(20f, 50f, this.mianCameraArm.pitch);
		this.lerpSpeed = Mathf.MoveTowards(this.lerpSpeed, this.inputManager.TriggerInput ? 1.5f : 12f, Time.deltaTime * 1f);
		this.lerpSpeed = Mathf.Lerp(1.5f, this.lerpSpeed, num);
		if (!InputManager.InputActived)
		{
			this.offsetFromTargetX = Mathf.Lerp(this.offsetFromTargetX, 0f, Time.unscaledDeltaTime * this.lerpSpeed);
			this.offsetFromTargetZ = Mathf.Lerp(this.offsetFromTargetZ, 0f, Time.unscaledDeltaTime * this.lerpSpeed);
			return;
		}
		Vector2 mousePos = this.inputManager.MousePos;
		Vector2 vector = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
		Vector3 vector2 = this.ScreenPointToCharacterPlane(vector);
		Vector3 a = this.ScreenPointToCharacterPlane(new Vector2(vector.x, (float)Screen.height));
		Vector3 b = this.ScreenPointToCharacterPlane(new Vector2(vector.x, 0f));
		Vector3.Distance(a, b);
		Vector3 a2 = this.ScreenPointToCharacterPlane(mousePos);
		Vector3 vector3 = (a + b) * 0.5f;
		vector3 = Vector3.MoveTowards(vector2, vector3, 5f * num);
		float num2 = Vector3.Distance(vector3, vector2);
		Vector3 lhs = Vector3.ClampMagnitude(a2 - vector3, this.maxAimOffset) * num;
		float num3 = Vector3.Dot(lhs, this.cameraRightVector);
		float num4 = Vector3.Dot(lhs, this.cameraForwardVector);
		this.offsetFromTargetX = Mathf.Lerp(this.offsetFromTargetX, Mathf.Clamp(num3 * this.aimOffsetDistanceFactor, -this.maxAimOffset, this.maxAimOffset), deltaTime * this.lerpSpeed);
		this.offsetFromTargetZ = Mathf.Lerp(this.offsetFromTargetZ, Mathf.Clamp(num4 * this.aimOffsetDistanceFactor, -this.maxAimOffset, this.maxAimOffset) - num2, deltaTime * this.lerpSpeed);
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00009140 File Offset: 0x00007340
	private void UpdateAimOffsetUsingBound(float deltaTime)
	{
		if (this.inputManager.TriggerInput)
		{
			return;
		}
		float num = 20f;
		Vector2 mousePos = this.inputManager.MousePos;
		Vector2 vector = Vector2.zero;
		int num2 = (int)((float)Screen.height * 0.05f);
		if (mousePos.x < (float)num2)
		{
			vector += Vector2.left;
		}
		else if (mousePos.x > (float)(Screen.width - num2))
		{
			vector += Vector2.right;
		}
		if (mousePos.y < (float)num2)
		{
			vector += Vector2.down;
		}
		else if (mousePos.y > (float)(Screen.height - num2))
		{
			vector += Vector2.up;
		}
		if (!InputManager.InputActived)
		{
			vector = Vector2.zero;
		}
		if (!Application.isFocused)
		{
			vector = Vector2.zero;
		}
		if (vector.x != 0f)
		{
			this.offsetFromTargetX += vector.x * deltaTime * num;
			this.offsetFromTargetX = Mathf.Clamp(this.offsetFromTargetX, -this.maxAimOffset, this.maxAimOffset);
		}
		if (vector.y != 0f)
		{
			this.offsetFromTargetZ += vector.y * deltaTime * num;
			this.offsetFromTargetZ = Mathf.Clamp(this.offsetFromTargetZ, -this.maxAimOffset, this.maxAimOffset);
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00009288 File Offset: 0x00007488
	private Vector3 ScreenPointToCharacterPlane(Vector3 screenPoint)
	{
		Plane plane = new Plane(Vector3.up, this.target.transform.position + Vector3.up * 0.5f);
		Ray ray = this.renderCamera.ScreenPointToRay(screenPoint);
		float distance;
		if (plane.Raycast(ray, out distance))
		{
			return ray.GetPoint(distance);
		}
		return Vector3.zero;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x000092EC File Offset: 0x000074EC
	public bool IsOffScreen(Vector3 woorldPos)
	{
		Vector3 vector = Camera.main.WorldToScreenPoint(woorldPos);
		return vector.x <= 0f || vector.x >= (float)Screen.width || vector.y <= 0f || vector.y >= (float)Screen.height;
	}

	// Token: 0x04000180 RID: 384
	public Camera renderCamera;

	// Token: 0x04000181 RID: 385
	public CinemachineVirtualCamera mainVCam;

	// Token: 0x04000182 RID: 386
	public CameraArm mianCameraArm;

	// Token: 0x04000183 RID: 387
	public CharacterMainControl target;

	// Token: 0x04000184 RID: 388
	public CinemachineBrain brain;

	// Token: 0x04000185 RID: 389
	public float defaultFOV = 20f;

	// Token: 0x04000186 RID: 390
	public float adsFOV = 15f;

	// Token: 0x04000187 RID: 391
	public Transform mainCamDepthPoint;

	// Token: 0x04000188 RID: 392
	private float currentFov;

	// Token: 0x04000189 RID: 393
	public static Action<GameCamera, CharacterMainControl> OnCameraPosUpdate;

	// Token: 0x0400018A RID: 394
	private Vector3 virtualTarget;

	// Token: 0x0400018B RID: 395
	private float offsetFromTargetX;

	// Token: 0x0400018C RID: 396
	private float offsetFromTargetZ;

	// Token: 0x0400018D RID: 397
	public Transform volumeProxy;

	// Token: 0x0400018E RID: 398
	private DepthOfField dofComponent;

	// Token: 0x0400018F RID: 399
	private float adsValue;

	// Token: 0x04000190 RID: 400
	private bool adsChanging;

	// Token: 0x04000191 RID: 401
	private float defaultAimOffset = 5f;

	// Token: 0x04000192 RID: 402
	private float maxAimOffset = 999f;

	// Token: 0x04000193 RID: 403
	private ItemAgent_Gun gun;

	// Token: 0x04000194 RID: 404
	private InputManager inputManager;

	// Token: 0x04000195 RID: 405
	private Vector3 cameraForwardVector;

	// Token: 0x04000196 RID: 406
	private Vector3 cameraRightVector;

	// Token: 0x04000197 RID: 407
	private float defaultAimOffsetDistanceFactor = 0.5f;

	// Token: 0x04000198 RID: 408
	private float aimOffsetDistanceFactor;

	// Token: 0x04000199 RID: 409
	private GameCamera.CameraAimingTypes cameraAimingType;

	// Token: 0x0400019A RID: 410
	private float lerpSpeed = 12f;

	// Token: 0x0200042C RID: 1068
	public enum CameraAimingTypes
	{
		// Token: 0x04001A07 RID: 6663
		normal,
		// Token: 0x04001A08 RID: 6664
		bounds
	}
}
