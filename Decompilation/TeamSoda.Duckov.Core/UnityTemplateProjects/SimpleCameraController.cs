using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityTemplateProjects
{
	// Token: 0x02000223 RID: 547
	public class SimpleCameraController : MonoBehaviour
	{
		// Token: 0x06001068 RID: 4200 RVA: 0x0003F6DC File Offset: 0x0003D8DC
		private void Start()
		{
			InputActionMap map = new InputActionMap("Simple Camera Controller");
			this.lookAction = map.AddAction("look", InputActionType.Value, "<Mouse>/delta", null, null, null, null);
			this.movementAction = map.AddAction("move", InputActionType.Value, "<Gamepad>/leftStick", null, null, null, null);
			this.verticalMovementAction = map.AddAction("Vertical Movement", InputActionType.Value, null, null, null, null, null);
			this.boostFactorAction = map.AddAction("Boost Factor", InputActionType.Value, "<Mouse>/scroll", null, null, null, null);
			this.lookAction.AddBinding("<Gamepad>/rightStick", null, null, null).WithProcessor("scaleVector2(x=15, y=15)");
			this.movementAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/w", null, null).With("Up", "<Keyboard>/upArrow", null, null).With("Down", "<Keyboard>/s", null, null).With("Down", "<Keyboard>/downArrow", null, null).With("Left", "<Keyboard>/a", null, null).With("Left", "<Keyboard>/leftArrow", null, null).With("Right", "<Keyboard>/d", null, null).With("Right", "<Keyboard>/rightArrow", null, null);
			this.verticalMovementAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/pageUp", null, null).With("Down", "<Keyboard>/pageDown", null, null).With("Up", "<Keyboard>/e", null, null).With("Down", "<Keyboard>/q", null, null).With("Up", "<Gamepad>/rightshoulder", null, null).With("Down", "<Gamepad>/leftshoulder", null, null);
			this.boostFactorAction.AddBinding("<Gamepad>/Dpad", null, null, null).WithProcessor("scaleVector2(x=1, y=4)");
			this.movementAction.Enable();
			this.lookAction.Enable();
			this.verticalMovementAction.Enable();
			this.boostFactorAction.Enable();
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0003F908 File Offset: 0x0003DB08
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0003F92C File Offset: 0x0003DB2C
		private Vector3 GetInputTranslationDirection()
		{
			Vector3 zero = Vector3.zero;
			Vector2 vector = this.movementAction.ReadValue<Vector2>();
			zero.x = vector.x;
			zero.z = vector.y;
			zero.y = this.verticalMovementAction.ReadValue<Vector2>().y;
			return zero;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0003F980 File Offset: 0x0003DB80
		private void Update()
		{
			if (this.IsEscapePressed())
			{
				Application.Quit();
			}
			if (this.IsRightMouseButtonDown())
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (this.IsRightMouseButtonUp())
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			if (this.IsCameraRotationAllowed())
			{
				Vector2 vector = this.GetInputLookRotation() * Time.deltaTime * 5f;
				if (this.invertY)
				{
					vector.y = -vector.y;
				}
				float num = this.mouseSensitivityCurve.Evaluate(vector.magnitude);
				this.m_TargetCameraState.yaw += vector.x * num;
				this.m_TargetCameraState.pitch += vector.y * num;
			}
			Vector3 vector2 = this.GetInputTranslationDirection() * Time.deltaTime;
			if (this.IsBoostPressed())
			{
				vector2 *= 10f;
			}
			this.boost += this.GetBoostFactor();
			vector2 *= Mathf.Pow(2f, this.boost);
			this.m_TargetCameraState.Translate(vector2);
			float positionLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.positionLerpTime * Time.deltaTime);
			float rotationLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.rotationLerpTime * Time.deltaTime);
			this.m_InterpolatingCameraState.LerpTowards(this.m_TargetCameraState, positionLerpPct, rotationLerpPct);
			this.m_InterpolatingCameraState.UpdateTransform(base.transform);
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0003FB04 File Offset: 0x0003DD04
		private float GetBoostFactor()
		{
			return this.boostFactorAction.ReadValue<Vector2>().y * 0.01f;
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0003FB1C File Offset: 0x0003DD1C
		private Vector2 GetInputLookRotation()
		{
			return this.lookAction.ReadValue<Vector2>();
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0003FB29 File Offset: 0x0003DD29
		private bool IsBoostPressed()
		{
			return (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed) | (Gamepad.current != null && Gamepad.current.xButton.isPressed);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0003FB5E File Offset: 0x0003DD5E
		private bool IsEscapePressed()
		{
			return Keyboard.current != null && Keyboard.current.escapeKey.isPressed;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0003FB78 File Offset: 0x0003DD78
		private bool IsCameraRotationAllowed()
		{
			return (Mouse.current != null && Mouse.current.rightButton.isPressed) | (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0f);
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0003FBC7 File Offset: 0x0003DDC7
		private bool IsRightMouseButtonDown()
		{
			return Mouse.current != null && Mouse.current.rightButton.isPressed;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0003FBE1 File Offset: 0x0003DDE1
		private bool IsRightMouseButtonUp()
		{
			return Mouse.current != null && !Mouse.current.rightButton.isPressed;
		}

		// Token: 0x04000D0C RID: 3340
		private SimpleCameraController.CameraState m_TargetCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000D0D RID: 3341
		private SimpleCameraController.CameraState m_InterpolatingCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000D0E RID: 3342
		[Header("Movement Settings")]
		[Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
		public float boost = 3.5f;

		// Token: 0x04000D0F RID: 3343
		[Tooltip("Time it takes to interpolate camera position 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float positionLerpTime = 0.2f;

		// Token: 0x04000D10 RID: 3344
		[Header("Rotation Settings")]
		[Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x04000D11 RID: 3345
		[Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float rotationLerpTime = 0.01f;

		// Token: 0x04000D12 RID: 3346
		[Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
		public bool invertY;

		// Token: 0x04000D13 RID: 3347
		private InputAction movementAction;

		// Token: 0x04000D14 RID: 3348
		private InputAction verticalMovementAction;

		// Token: 0x04000D15 RID: 3349
		private InputAction lookAction;

		// Token: 0x04000D16 RID: 3350
		private InputAction boostFactorAction;

		// Token: 0x04000D17 RID: 3351
		private bool mouseRightButtonPressed;

		// Token: 0x02000506 RID: 1286
		private class CameraState
		{
			// Token: 0x0600276D RID: 10093 RVA: 0x0008FF50 File Offset: 0x0008E150
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x0600276E RID: 10094 RVA: 0x0008FFC4 File Offset: 0x0008E1C4
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x0600276F RID: 10095 RVA: 0x00090028 File Offset: 0x0008E228
			public void LerpTowards(SimpleCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x06002770 RID: 10096 RVA: 0x000900C5 File Offset: 0x0008E2C5
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x04001DBC RID: 7612
			public float yaw;

			// Token: 0x04001DBD RID: 7613
			public float pitch;

			// Token: 0x04001DBE RID: 7614
			public float roll;

			// Token: 0x04001DBF RID: 7615
			public float x;

			// Token: 0x04001DC0 RID: 7616
			public float y;

			// Token: 0x04001DC1 RID: 7617
			public float z;
		}
	}
}
