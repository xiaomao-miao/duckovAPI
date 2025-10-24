using System;
using CameraSystems;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001B7 RID: 439
public class FreeCameraController : MonoBehaviour
{
	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00035EB0 File Offset: 0x000340B0
	private Gamepad Gamepad
	{
		get
		{
			return Gamepad.current;
		}
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x00035EB7 File Offset: 0x000340B7
	private void Awake()
	{
		if (!this.propertiesControl)
		{
			this.propertiesControl = base.GetComponent<CameraPropertiesControl>();
		}
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00035ED2 File Offset: 0x000340D2
	private void OnEnable()
	{
		this.SetRotation(base.transform.rotation);
		this.SnapToMainCamera();
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00035EEC File Offset: 0x000340EC
	public void SetRotation(Quaternion rotation)
	{
		Vector3 eulerAngles = rotation.eulerAngles;
		this.yaw = eulerAngles.y;
		this.pitch = eulerAngles.x;
		this.yawTarget = this.yaw;
		this.pitchTarget = this.pitch;
		if (this.pitch > 180f)
		{
			this.pitch -= 360f;
		}
		if (this.pitch < -180f)
		{
			this.pitch += 360f;
		}
		this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x00035FAC File Offset: 0x000341AC
	private unsafe void Update()
	{
		if (this.Gamepad == null)
		{
			return;
		}
		bool isPressed = this.Gamepad.rightShoulder.isPressed;
		float num = this.moveSpeed * (float)(isPressed ? 2 : 1);
		CharacterMainControl main = CharacterMainControl.Main;
		Vector2 vector = *this.Gamepad.leftStick.value;
		float d = *this.Gamepad.rightTrigger.value - *this.Gamepad.leftTrigger.value;
		Vector3 vector2 = new Vector3(vector.x * num, 0f, vector.y * num) * Time.unscaledDeltaTime;
		Vector3 a = this.projectMovementOnXZPlane ? Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized : base.transform.forward;
		Vector3 a2 = this.projectMovementOnXZPlane ? Vector3.ProjectOnPlane(base.transform.right, Vector3.up).normalized : base.transform.right;
		Vector3 b = d * Vector3.up * num * 0.5f * Time.unscaledDeltaTime;
		Vector3 b2 = a * vector2.z + a2 * vector2.x + b;
		if (!this.followCharacter || main == null)
		{
			this.worldPosTarget += b2;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, this.worldPosTarget, ref this.velocityWorldSpace, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
			if (main == null)
			{
				this.followCharacter = false;
			}
		}
		else
		{
			this.offsetFromCharacter += b2;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, main.transform.position + this.offsetFromCharacter, ref this.velocityLocalSpace, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		}
		Vector3 vector3 = *this.Gamepad.rightStick.value * this.rotateSpeed * this.vCamera.m_Lens.FieldOfView / 60f;
		this.yawTarget += vector3.x * Time.unscaledDeltaTime;
		this.yaw = Mathf.SmoothDamp(this.yaw, this.yawTarget, ref this.yawVelocity, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		this.pitchTarget += -vector3.y * Time.unscaledDeltaTime;
		this.pitch = Mathf.SmoothDamp(this.pitch, this.pitchTarget, ref this.pitchVelocity, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
		if (this.Gamepad.buttonNorth.wasPressedThisFrame)
		{
			this.SnapToMainCamera();
		}
		if (this.Gamepad.buttonEast.wasPressedThisFrame)
		{
			this.ToggleFollowTarget();
		}
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x00036322 File Offset: 0x00034522
	private void OnDestroy()
	{
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x00036324 File Offset: 0x00034524
	private void ToggleFollowTarget()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		this.followCharacter = !this.followCharacter;
		if (this.followCharacter)
		{
			this.offsetFromCharacter = base.transform.position - main.transform.position;
		}
		this.worldPosTarget = base.transform.position;
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x0003638C File Offset: 0x0003458C
	private void SnapToMainCamera()
	{
		if (GameCamera.Instance == null)
		{
			return;
		}
		Camera renderCamera = GameCamera.Instance.renderCamera;
		if (renderCamera == null)
		{
			return;
		}
		base.transform.position = renderCamera.transform.position;
		this.worldPosTarget = renderCamera.transform.position;
		this.vCamera.m_Lens.FieldOfView = renderCamera.fieldOfView;
		this.SetRotation(renderCamera.transform.rotation);
		CharacterMainControl main = CharacterMainControl.Main;
		if (main != null && this.followCharacter)
		{
			this.offsetFromCharacter = base.transform.position - main.transform.position;
		}
	}

	// Token: 0x04000B2D RID: 2861
	[SerializeField]
	private CameraPropertiesControl propertiesControl;

	// Token: 0x04000B2E RID: 2862
	[SerializeField]
	private float moveSpeed = 10f;

	// Token: 0x04000B2F RID: 2863
	[SerializeField]
	private float rotateSpeed = 180f;

	// Token: 0x04000B30 RID: 2864
	[SerializeField]
	private float smoothTime = 2f;

	// Token: 0x04000B31 RID: 2865
	[SerializeField]
	private Vector2 minMaxXRotation = new Vector2(-89f, 89f);

	// Token: 0x04000B32 RID: 2866
	[SerializeField]
	private bool projectMovementOnXZPlane;

	// Token: 0x04000B33 RID: 2867
	[Range(-180f, 180f)]
	private float yaw;

	// Token: 0x04000B34 RID: 2868
	[Range(-89f, 89f)]
	private float pitch;

	// Token: 0x04000B35 RID: 2869
	[SerializeField]
	private CinemachineVirtualCamera vCamera;

	// Token: 0x04000B36 RID: 2870
	private bool followCharacter;

	// Token: 0x04000B37 RID: 2871
	private Vector3 offsetFromCharacter;

	// Token: 0x04000B38 RID: 2872
	private Vector3 worldPosTarget;

	// Token: 0x04000B39 RID: 2873
	private Vector3 velocityWorldSpace;

	// Token: 0x04000B3A RID: 2874
	private Vector3 velocityLocalSpace;

	// Token: 0x04000B3B RID: 2875
	private float yawVelocity;

	// Token: 0x04000B3C RID: 2876
	private float pitchVelocity;

	// Token: 0x04000B3D RID: 2877
	private float yawTarget;

	// Token: 0x04000B3E RID: 2878
	private float pitchTarget;
}
