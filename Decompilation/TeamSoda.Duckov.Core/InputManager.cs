using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.Options;
using Duckov.UI;
using Duckov.UI.DialogueBubbles;
using Duckov.Utilities;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000104 RID: 260
public class InputManager : MonoBehaviour
{
	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000883 RID: 2179 RVA: 0x00026018 File Offset: 0x00024218
	public static InputManager.InputDevices InputDevice
	{
		get
		{
			return InputManager.inputDevice;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x0002601F File Offset: 0x0002421F
	public Vector3 WorldMoveInput
	{
		get
		{
			return this.worldMoveInput;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x00026027 File Offset: 0x00024227
	public Transform AimTarget
	{
		get
		{
			return this.aimTargetCol;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002602F File Offset: 0x0002422F
	public Vector2 MoveAxisInput
	{
		get
		{
			return this.moveAxisInput;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000887 RID: 2183 RVA: 0x00026037 File Offset: 0x00024237
	public Vector2 AimScreenPoint
	{
		get
		{
			return this.aimScreenPoint;
		}
	}

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06000888 RID: 2184 RVA: 0x00026040 File Offset: 0x00024240
	// (remove) Token: 0x06000889 RID: 2185 RVA: 0x00026074 File Offset: 0x00024274
	public static event Action OnInputDeviceChanged;

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x0600088A RID: 2186 RVA: 0x000260A7 File Offset: 0x000242A7
	public Vector3 InputAimPoint
	{
		get
		{
			return this.inputAimPoint;
		}
	}

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x0600088B RID: 2187 RVA: 0x000260B0 File Offset: 0x000242B0
	// (remove) Token: 0x0600088C RID: 2188 RVA: 0x000260E4 File Offset: 0x000242E4
	public static event Action<int> OnSwitchBulletTypeInput;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x0600088D RID: 2189 RVA: 0x00026118 File Offset: 0x00024318
	// (remove) Token: 0x0600088E RID: 2190 RVA: 0x0002614C File Offset: 0x0002434C
	public static event Action<int> OnSwitchWeaponInput;

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x0002617F File Offset: 0x0002437F
	private static InputManager instance
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance.InputManager;
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0002619A File Offset: 0x0002439A
	private void OnDestroy()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000891 RID: 2193 RVA: 0x000261A8 File Offset: 0x000243A8
	public static bool InputActived
	{
		get
		{
			return InputManager.instance && !GameManager.Paused && !CameraMode.Active && LevelManager.LevelInited && CharacterMainControl.Main && !CharacterMainControl.Main.Health.IsDead && InputManager.instance.inputActiveCoolCounter <= 0;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000892 RID: 2194 RVA: 0x0002620D File Offset: 0x0002440D
	public Vector2 MousePos
	{
		get
		{
			return this.inputMousePosition;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000893 RID: 2195 RVA: 0x00026215 File Offset: 0x00024415
	public bool TriggerInput
	{
		get
		{
			return this.triggerInput;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000894 RID: 2196 RVA: 0x0002621D File Offset: 0x0002441D
	// (set) Token: 0x06000895 RID: 2197 RVA: 0x00026250 File Offset: 0x00024450
	private Vector2 AimMousePosition
	{
		get
		{
			if (!this.aimMousePosFirstSynced)
			{
				this.aimMousePosFirstSynced = true;
				if (Mouse.current != null)
				{
					this._aimMousePosCache = Mouse.current.position.ReadValue();
				}
			}
			return this._aimMousePosCache;
		}
		set
		{
			if (!this.aimMousePosFirstSynced)
			{
				this.aimMousePosFirstSynced = true;
				if (Mouse.current != null)
				{
					this._aimMousePosCache = Mouse.current.position.ReadValue();
				}
			}
			this._aimMousePosCache = value;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000896 RID: 2198 RVA: 0x00026284 File Offset: 0x00024484
	public bool AimingEnemyHead
	{
		get
		{
			return this.aimingEnemyHead;
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0002628C File Offset: 0x0002448C
	private void Start()
	{
		this.obsticleHits = new RaycastHit[3];
		this.obsticleLayers = (GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask);
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000262C4 File Offset: 0x000244C4
	private void OnApplicationFocus(bool hasFocus)
	{
		this.currentFocus = hasFocus;
		if (!this.currentFocus)
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x000262DB File Offset: 0x000244DB
	private void Awake()
	{
		if (this.blockInputSources == null)
		{
			this.blockInputSources = new HashSet<GameObject>();
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x000262F0 File Offset: 0x000244F0
	public static void DisableInput(GameObject source)
	{
		if (source == null)
		{
			return;
		}
		if (InputManager.instance == null)
		{
			return;
		}
		InputManager.instance.inputActiveCoolCounter = 2;
		InputManager.instance.blockInputSources.Add(source);
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00026326 File Offset: 0x00024526
	public static void ActiveInput(GameObject source)
	{
		if (source == null)
		{
			return;
		}
		InputManager.instance.blockInputSources.Remove(source);
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x00026343 File Offset: 0x00024543
	public static void SetInputDevice(InputManager.InputDevices _inputDevice)
	{
		Action onInputDeviceChanged = InputManager.OnInputDeviceChanged;
		if (onInputDeviceChanged == null)
		{
			return;
		}
		onInputDeviceChanged();
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00026354 File Offset: 0x00024554
	private void UpdateCursor()
	{
		if (LevelManager.Instance == null || this.characterMainControl == null || !this.characterMainControl.gameObject.activeInHierarchy)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			return;
		}
		bool flag = !this.characterMainControl || this.characterMainControl.Health.IsDead;
		bool flag2 = true;
		if (InputManager.InputActived && !flag)
		{
			flag2 = false;
		}
		if (CameraMode.Active)
		{
			flag2 = false;
		}
		if (View.ActiveView != null)
		{
			flag2 = true;
		}
		if (!Application.isFocused)
		{
			flag2 = true;
		}
		if (this.cursorVisable != flag2)
		{
			this.cursorVisable = !this.cursorVisable;
		}
		if (this.cursorVisable)
		{
			this.recoilNeedToRecover = Vector2.zero;
			if (Mouse.current != null)
			{
				this.AimMousePosition = Mouse.current.position.ReadValue();
			}
		}
		if (Application.isFocused)
		{
			Cursor.visible = this.cursorVisable;
		}
		else
		{
			Cursor.visible = true;
		}
		bool flag3 = false;
		if (CameraMode.Active)
		{
			flag3 = true;
		}
		if (this.currentFocus)
		{
			Cursor.lockState = (flag3 ? CursorLockMode.Locked : CursorLockMode.Confined);
			return;
		}
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00026474 File Offset: 0x00024674
	private void Update()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!this.mainCam)
		{
			this.mainCam = LevelManager.Instance.GameCamera.renderCamera;
			return;
		}
		this.UpdateInputActived();
		this.UpdateCursor();
		if (this.runInput)
		{
			if (this.runInptutThisFrame)
			{
				this.runInputBuffer = !this.runInputBuffer;
			}
		}
		else if (this.moveAxisInput.magnitude < 0.1f)
		{
			this.runInputBuffer = false;
		}
		else if (this.adsInput)
		{
			this.runInputBuffer = false;
		}
		this.characterMainControl.SetRunInput(InputManager.useRunInputBuffer ? this.runInputBuffer : this.runInput);
		this.SetMoveInput(this.moveAxisInput);
		if (InputManager.InputDevice == InputManager.InputDevices.touch)
		{
			this.UpdateJoystickAim();
			this.UpdateAimWhileUsingTouch();
		}
		if (this.checkGunDurabilityCoolTimer <= this.checkGunDurabilityCoolTime)
		{
			this.checkGunDurabilityCoolTimer += Time.deltaTime;
		}
		this.runInptutThisFrame = false;
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00026570 File Offset: 0x00024770
	private void UpdateInputActived()
	{
		this.blockInputSources.RemoveWhere((GameObject x) => x == null || !x.activeInHierarchy);
		if (this.blockInputSources.Count > 0)
		{
			InputManager.instance.inputActiveCoolCounter = 2;
			return;
		}
		if (InputManager.instance.inputActiveCoolCounter > 0)
		{
			InputManager.instance.inputActiveCoolCounter--;
		}
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x000265E1 File Offset: 0x000247E1
	private void UpdateAimWhileUsingTouch()
	{
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x000265E4 File Offset: 0x000247E4
	public void SetTrigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		this.triggerInput = false;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.Trigger(false, false, false);
			return;
		}
		this.triggerInput = trigger;
		this.characterMainControl.Trigger(trigger, triggerThisFrame, releaseThisFrame);
		if (trigger)
		{
			this.CheckGunDurability();
		}
		if (triggerThisFrame)
		{
			this.runInputBuffer = false;
			this.characterMainControl.Attack();
		}
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00026650 File Offset: 0x00024850
	private void CheckAttack()
	{
		if (InputManager.InputDevice != InputManager.InputDevices.touch)
		{
			return;
		}
		if (this.characterMainControl.CurrentAction && this.characterMainControl.CurrentAction.Running)
		{
			return;
		}
		ItemAgent_MeleeWeapon meleeWeapon = this.characterMainControl.GetMeleeWeapon();
		if (meleeWeapon == null)
		{
			return;
		}
		if (meleeWeapon.AttackableTargetInRange())
		{
			this.characterMainControl.Attack();
		}
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x000266B8 File Offset: 0x000248B8
	private void CheckGunDurability()
	{
		if (this.checkGunDurabilityCoolTimer <= this.checkGunDurabilityCoolTime)
		{
			return;
		}
		ItemAgent_Gun gun = this.characterMainControl.GetGun();
		if (gun != null && gun.Item.Durability <= 0f)
		{
			DialogueBubblesManager.Show("Pop_GunBroken".ToPlainText(), this.characterMainControl.transform, 2.5f, false, false, -1f, 2f).Forget();
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0002672C File Offset: 0x0002492C
	private Vector3 TrnasAxisInputToWorld(Vector2 axisInput)
	{
		Vector3 result = Vector3.zero;
		if (!this.mainCam)
		{
			return result;
		}
		if (!this.characterMainControl)
		{
			return result;
		}
		if (MoveDirectionOptions.MoveViaCharacterDirection)
		{
			Vector3 vector = this.inputAimPoint - this.characterMainControl.transform.position;
			vector.y = 0f;
			if (vector.magnitude < 1f)
			{
				return this.characterMainControl.transform.forward;
			}
			vector.Normalize();
			Vector3 a = Quaternion.Euler(0f, 90f, 0f) * vector;
			result = axisInput.x * a + axisInput.y * vector;
		}
		else
		{
			Vector3 right = this.mainCam.transform.right;
			right.y = 0f;
			right.Normalize();
			Vector3 forward = this.mainCam.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			result = axisInput.x * right + axisInput.y * forward;
		}
		return result;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00026859 File Offset: 0x00024A59
	public void SetSwitchBulletTypeInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		Action<int> onSwitchBulletTypeInput = InputManager.OnSwitchBulletTypeInput;
		if (onSwitchBulletTypeInput == null)
		{
			return;
		}
		onSwitchBulletTypeInput(dir);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00026881 File Offset: 0x00024A81
	public void SetSwitchWeaponInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		Action<int> onSwitchWeaponInput = InputManager.OnSwitchWeaponInput;
		if (onSwitchWeaponInput != null)
		{
			onSwitchWeaponInput(dir);
		}
		this.characterMainControl.SwitchWeapon(dir);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x000268B6 File Offset: 0x00024AB6
	public void SetSwitchInteractInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.SwitchInteractSelection((dir > 0) ? -1 : 1);
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x000268E4 File Offset: 0x00024AE4
	public void SetMoveInput(Vector2 axisInput)
	{
		this.moveAxisInput = axisInput;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.SetMoveInput(Vector3.zero);
			return;
		}
		this.worldMoveInput = this.TrnasAxisInputToWorld(axisInput);
		Vector3 normalized = this.worldMoveInput;
		if (normalized.magnitude > 0.02f)
		{
			normalized = normalized.normalized;
		}
		this.characterMainControl.SetMoveInput(normalized);
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00026954 File Offset: 0x00024B54
	public void SetRunInput(bool run)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.runInput = false;
			this.runInptutThisFrame = false;
			this.characterMainControl.SetRunInput(false);
			return;
		}
		this.runInptutThisFrame = (!this.runInput && run);
		this.runInput = run;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x000269A9 File Offset: 0x00024BA9
	public void SetAdsInput(bool ads)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.SetAdsInput(false);
			this.adsInput = false;
			return;
		}
		this.adsInput = ads;
		this.characterMainControl.SetAdsInput(ads);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x000269E7 File Offset: 0x00024BE7
	public void ToggleView()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		CameraArm.ToggleView();
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00026A04 File Offset: 0x00024C04
	public void ToggleNightVision()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ToggleNightVision();
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00026A27 File Offset: 0x00024C27
	public void SetAimInputUsingJoystick(Vector2 _joystickAxisInput)
	{
		if (InputManager.InputDevice == InputManager.InputDevices.mouseKeyboard)
		{
			return;
		}
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.joystickAxisInput = Vector3.zero;
			return;
		}
		this.joystickAxisInput = _joystickAxisInput;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00026A5E File Offset: 0x00024C5E
	private void UpdateJoystickAim()
	{
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x00026A60 File Offset: 0x00024C60
	public void SetAimType(AimTypes aimType)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		SkillBase currentRunningSkill = this.characterMainControl.GetCurrentRunningSkill();
		if (aimType != this.characterMainControl.AimType && currentRunningSkill != null)
		{
			Debug.Log("skill is running:" + currentRunningSkill.name);
			return;
		}
		this.characterMainControl.SetAimType(aimType);
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00026AC8 File Offset: 0x00024CC8
	public void SetMousePosition(Vector2 mousePosition)
	{
		this.inputMousePosition = mousePosition;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00026AD4 File Offset: 0x00024CD4
	public void SetAimInputUsingMouse(Vector2 mouseDelta)
	{
		this.aimingEnemyHead = false;
		this.AimMousePosition += mouseDelta * OptionsManager.MouseSensitivity / 10f;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		ItemAgent_Gun gun = this.characterMainControl.GetGun();
		if (gun)
		{
			this.AimMousePosition = this.ProcessMousePosViaRecoil(this.AimMousePosition, mouseDelta, gun);
		}
		Vector2 vector = default(Vector2);
		if (Application.isFocused && InputManager.InputActived && !Application.isEditor)
		{
			Vector2 aimMousePosition = this.AimMousePosition;
			this.ClampMousePosInWindow(ref aimMousePosition, ref vector);
			this.AimMousePosition = aimMousePosition;
		}
		this.aimScreenPoint = this.AimMousePosition;
		this.characterMainControl.GetCurrentRunningSkill();
		Ray ray = LevelManager.Instance.GameCamera.renderCamera.ScreenPointToRay(this.aimScreenPoint);
		Plane plane = new Plane(Vector3.up, Vector3.up * (this.characterMainControl.transform.position.y + 0.5f));
		float d = 0f;
		plane.Raycast(ray, out d);
		Vector3 vector2 = ray.origin + ray.direction * d;
		Debug.DrawLine(vector2, vector2 + Vector3.up * 3f, Color.yellow);
		Vector3 aimPoint = vector2;
		if (gun && this.characterMainControl.CanControlAim())
		{
			if (Physics.Raycast(ray, out this.hittedHead, 100f, 1 << LayerMask.NameToLayer("HeadCollider")))
			{
				this.aimingEnemyHead = true;
			}
			Vector3 position = this.characterMainControl.transform.position;
			if (gun)
			{
				position = gun.muzzle.transform.position;
			}
			Vector3 vector3 = vector2 - position;
			vector3.y = 0f;
			vector3.Normalize();
			Vector3 axis = Vector3.Cross(vector3, ray.direction);
			this.aimCheckLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
			int num = 0;
			while ((float)num < 45f)
			{
				int num2 = num;
				if (num > 23)
				{
					num2 = -(num - 23);
				}
				float d2 = 1.5f;
				Vector3 vector4 = Quaternion.AngleAxis(-2f * (float)num2, axis) * vector3;
				Ray ray2 = new Ray(position + d2 * vector4, vector4);
				if (Physics.SphereCast(ray2, 0.02f, out this.hittedCharacterDmgReceiverInfo, gun.BulletDistance, this.aimCheckLayers, QueryTriggerInteraction.Ignore) && this.hittedCharacterDmgReceiverInfo.distance > 0.1f && !Physics.SphereCast(ray2, 0.1f, out this.hittedObsticleInfo, this.hittedCharacterDmgReceiverInfo.distance, this.obsticleLayers, QueryTriggerInteraction.Ignore))
				{
					aimPoint = this.hittedCharacterDmgReceiverInfo.point;
					break;
				}
				num++;
			}
		}
		if (this.aimingEnemyHead)
		{
			Vector3 direction = ray.direction;
			Vector3 rhs = this.hittedHead.collider.transform.position - this.hittedHead.point;
			float d3 = Vector3.Dot(direction, rhs);
			aimPoint = this.hittedHead.point + direction * d3 * 0.5f;
		}
		this.inputAimPoint = vector2;
		this.characterMainControl.SetAimPoint(aimPoint);
		if (Application.isFocused && this.currentFocus && InputManager.InputActived)
		{
			Mouse.current.WarpCursorPosition(this.AimMousePosition);
		}
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00026E6C File Offset: 0x0002506C
	private Vector2 ProcessMousePosViaCameraChange(Vector2 inputMousePos)
	{
		Camera renderCamera = LevelManager.Instance.GameCamera.renderCamera;
		if (this.fovCache < 0f)
		{
			this.fovCache = renderCamera.fieldOfView;
			return inputMousePos;
		}
		float fieldOfView = renderCamera.fieldOfView;
		Vector2 a = new Vector2(inputMousePos.x / (float)Screen.width * 2f - 1f, inputMousePos.y / (float)Screen.height * 2f - 1f);
		float d = Mathf.Tan(this.fovCache * 0.017453292f / 2f) / Mathf.Tan(fieldOfView * 0.017453292f / 2f);
		Vector2 vector = a * d;
		Vector2 result = new Vector2((vector.x + 1f) * 0.5f * (float)Screen.width, (vector.y + 1f) * 0.5f * (float)Screen.height);
		this.fovCache = fieldOfView;
		return result;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00026F54 File Offset: 0x00025154
	private void ClampMousePosInWindow(ref Vector2 mousePosition, ref Vector2 deltaValue)
	{
		Vector2 zero = Vector2.zero;
		zero.x = Mathf.Clamp(mousePosition.x, 0f, (float)Screen.width);
		zero.y = Mathf.Clamp(mousePosition.y, 0f, (float)Screen.height);
		deltaValue = zero - mousePosition;
		mousePosition = zero;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00026FBA File Offset: 0x000251BA
	public void Interact()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.Interact();
		Action onInteractButtonDown = InputManager.OnInteractButtonDown;
		if (onInteractButtonDown == null)
		{
			return;
		}
		onInteractButtonDown();
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00026FEC File Offset: 0x000251EC
	public void PutAway()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ChangeHoldItem(null);
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00027014 File Offset: 0x00025214
	public void SwitchItemAgent(int index)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		switch (index)
		{
		case 1:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.PrimaryWeaponSlotHash);
			return;
		case 2:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.SecondaryWeaponSlotHash);
			return;
		case 3:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.MeleeWeaponSlotHash);
			return;
		default:
			return;
		}
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x0002707E File Offset: 0x0002527E
	public void StopAction()
	{
		if (InputManager.InputActived && this.characterMainControl.CurrentAction && this.characterMainControl.CurrentAction.IsStopable())
		{
			this.characterMainControl.CurrentAction.StopAction();
		}
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x000270BC File Offset: 0x000252BC
	private bool CheckInAimAngleAndNoObsticle()
	{
		if (!this.characterMainControl)
		{
			return false;
		}
		if (this.aimTarget == null || this.characterMainControl.CurrentUsingAimSocket == null)
		{
			return false;
		}
		Vector3 position = this.characterMainControl.CurrentUsingAimSocket.position;
		position.y = 0f;
		Vector3 position2 = this.aimTarget.position;
		position2.y = 0f;
		Vector3 vector = position2 - position;
		float magnitude = vector.magnitude;
		vector.Normalize();
		float num = Mathf.Atan(0.25f / magnitude) * 57.29578f;
		if (Vector3.Angle(this.characterMainControl.CurrentAimDirection, vector) >= num)
		{
			return false;
		}
		Vector3 vector2 = position + Vector3.up * this.characterMainControl.CurrentUsingAimSocket.position.y;
		Vector3 vector3 = vector;
		Debug.DrawLine(vector2, vector2 + vector3 * magnitude);
		return Physics.SphereCastNonAlloc(vector2, 0.1f, vector3, this.obsticleHits, magnitude, this.obsticleLayers, QueryTriggerInteraction.Ignore) <= 0;
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x000271D5 File Offset: 0x000253D5
	public void ReleaseItemSkill()
	{
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ReleaseSkill(SkillTypes.itemSkill);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x000271EC File Offset: 0x000253EC
	public void ReleaseCharacterSkill()
	{
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ReleaseSkill(SkillTypes.characterSkill);
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00027203 File Offset: 0x00025403
	public bool CancleSkill()
	{
		return this.characterMainControl && this.characterMainControl.CancleSkill();
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0002721F File Offset: 0x0002541F
	public void Dash()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.TryCatchFishInput();
		this.characterMainControl.Dash();
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00027250 File Offset: 0x00025450
	public void StartCharacterSkillAim()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		if (this.characterMainControl.skillAction.characterSkillKeeper.Skill == null)
		{
			return;
		}
		if (this.characterMainControl.StartSkillAim(SkillTypes.characterSkill) && this.characterMainControl.skillAction.CurrentRunningSkill && this.characterMainControl.skillAction.CurrentRunningSkill.SkillContext.releaseOnStartAim)
		{
			this.characterMainControl.ReleaseSkill(SkillTypes.characterSkill);
		}
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x000272E0 File Offset: 0x000254E0
	public void StartItemSkillAim()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		if (!this.characterMainControl.agentHolder.Skill)
		{
			return;
		}
		if (this.characterMainControl.StartSkillAim(SkillTypes.itemSkill) && this.characterMainControl.skillAction.CurrentRunningSkill && this.characterMainControl.skillAction.CurrentRunningSkill.SkillContext.releaseOnStartAim)
		{
			this.characterMainControl.ReleaseSkill(SkillTypes.itemSkill);
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0002736C File Offset: 0x0002556C
	public void AddRecoil(ItemAgent_Gun gun)
	{
		if (!gun)
		{
			return;
		}
		this.recoilGun = gun;
		float recoilMultiplier = LevelManager.Rule.RecoilMultiplier;
		this.recoilV = UnityEngine.Random.Range(gun.RecoilVMin, gun.RecoilVMax) * gun.RecoilScaleV * (1f / gun.CharacterRecoilControl) * recoilMultiplier;
		this.recoilH = UnityEngine.Random.Range(gun.RecoilHMin, gun.RecoilHMax) * gun.RecoilScaleH * (1f / gun.CharacterRecoilControl) * recoilMultiplier;
		this.recoilRecover = gun.RecoilRecover;
		this.recoilTime = Mathf.Min(gun.RecoilTime, 1f / gun.ShootSpeed);
		this.recoilRecoverTime = gun.RecoilRecoverTime;
		this.recoilTimer = 0f;
		this.newRecoil = true;
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00027438 File Offset: 0x00025638
	private Vector2 ProcessMousePosViaRecoil(Vector2 mousePos, Vector2 mouseDelta, ItemAgent_Gun gun)
	{
		if (!gun || this.recoilGun != gun)
		{
			this.newRecoil = false;
			this.recoilNeedToRecover = Vector2.zero;
			return mousePos;
		}
		Vector3 position = this.characterMainControl.transform.position;
		if (this.newRecoil)
		{
			Vector2 b = LevelManager.Instance.GameCamera.renderCamera.WorldToScreenPoint(position);
			Vector2 normalized = (mousePos - b).normalized;
			this.recoilThisShot = normalized * this.recoilV + this.recoilH * -Vector2.Perpendicular(normalized);
		}
		Vector3.Distance(this.InputAimPoint, position);
		float num = Time.deltaTime;
		if (this.recoilTimer + num >= this.recoilTime)
		{
			num = this.recoilTime - this.recoilTimer;
		}
		if (num > 0f)
		{
			Vector2 b2 = this.recoilThisShot * num / this.recoilTime * (float)Screen.height / 1440f;
			mousePos += b2;
			this.recoilNeedToRecover += b2;
			Vector2 zero = Vector2.zero;
			this.ClampMousePosInWindow(ref mousePos, ref zero);
			this.recoilNeedToRecover += zero;
		}
		if (num <= 0f && this.recoilTimer > this.recoilRecoverTime && this.recoilNeedToRecover.magnitude > 0f)
		{
			float num2 = Time.deltaTime;
			if (this.recoilTimer - num2 < this.recoilRecoverTime)
			{
				num2 = this.recoilTimer - this.recoilRecoverTime;
			}
			Vector2 a = Vector2.MoveTowards(this.recoilNeedToRecover, Vector2.zero, num2 * this.recoilRecover * (float)Screen.height / 1440f);
			mousePos += a - this.recoilNeedToRecover;
			this.recoilNeedToRecover = a;
		}
		float num3 = Vector2.Dot(-this.recoilNeedToRecover.normalized, mouseDelta);
		if (num3 > 0f)
		{
			this._oppositeDelta = 0f;
			this.recoilNeedToRecover = Vector2.MoveTowards(this.recoilNeedToRecover, Vector2.zero, num3);
		}
		else
		{
			this._oppositeDelta += mouseDelta.magnitude;
			if (this._oppositeDelta > 15f * (float)Screen.height / 1440f)
			{
				this._oppositeDelta = 0f;
				this.recoilNeedToRecover = Vector3.zero;
			}
		}
		this.recoilTimer += Time.deltaTime;
		this.newRecoil = false;
		return mousePos;
	}

	// Token: 0x040007BE RID: 1982
	private static InputManager.InputDevices inputDevice = InputManager.InputDevices.mouseKeyboard;

	// Token: 0x040007BF RID: 1983
	public CharacterMainControl characterMainControl;

	// Token: 0x040007C0 RID: 1984
	public AimTargetFinder aimTargetFinder;

	// Token: 0x040007C1 RID: 1985
	public float runThreshold = 0.85f;

	// Token: 0x040007C2 RID: 1986
	private Vector3 worldMoveInput;

	// Token: 0x040007C3 RID: 1987
	public static Action OnInteractButtonDown;

	// Token: 0x040007C4 RID: 1988
	private Transform aimTargetCol;

	// Token: 0x040007C5 RID: 1989
	private LayerMask obsticleLayers;

	// Token: 0x040007C6 RID: 1990
	private RaycastHit[] obsticleHits;

	// Token: 0x040007C7 RID: 1991
	private RaycastHit hittedCharacterDmgReceiverInfo;

	// Token: 0x040007C8 RID: 1992
	private RaycastHit hittedObsticleInfo;

	// Token: 0x040007C9 RID: 1993
	private RaycastHit hittedHead;

	// Token: 0x040007CA RID: 1994
	private LayerMask aimCheckLayers;

	// Token: 0x040007CB RID: 1995
	private CharacterMainControl foundCharacter;

	// Token: 0x040007CC RID: 1996
	public static readonly int PrimaryWeaponSlotHash = "PrimaryWeapon".GetHashCode();

	// Token: 0x040007CD RID: 1997
	public static readonly int SecondaryWeaponSlotHash = "SecondaryWeapon".GetHashCode();

	// Token: 0x040007CE RID: 1998
	public static readonly int MeleeWeaponSlotHash = "MeleeWeapon".GetHashCode();

	// Token: 0x040007CF RID: 1999
	private Camera mainCam;

	// Token: 0x040007D0 RID: 2000
	private float checkGunDurabilityCoolTimer;

	// Token: 0x040007D1 RID: 2001
	private float checkGunDurabilityCoolTime = 2f;

	// Token: 0x040007D2 RID: 2002
	private Transform aimTarget;

	// Token: 0x040007D3 RID: 2003
	private Vector2 joystickAxisInput;

	// Token: 0x040007D4 RID: 2004
	private Vector2 moveAxisInput;

	// Token: 0x040007D5 RID: 2005
	private Vector2 aimScreenPoint;

	// Token: 0x040007D7 RID: 2007
	private Vector3 inputAimPoint;

	// Token: 0x040007D8 RID: 2008
	public static bool useRunInputBuffer = false;

	// Token: 0x040007D9 RID: 2009
	private HashSet<GameObject> blockInputSources = new HashSet<GameObject>();

	// Token: 0x040007DC RID: 2012
	private int inputActiveCoolCounter;

	// Token: 0x040007DD RID: 2013
	private bool adsInput;

	// Token: 0x040007DE RID: 2014
	private bool runInputBuffer;

	// Token: 0x040007DF RID: 2015
	private bool runInput;

	// Token: 0x040007E0 RID: 2016
	private bool runInptutThisFrame;

	// Token: 0x040007E1 RID: 2017
	private bool newRecoil;

	// Token: 0x040007E2 RID: 2018
	private ItemAgent_Gun recoilGun;

	// Token: 0x040007E3 RID: 2019
	private float recoilV;

	// Token: 0x040007E4 RID: 2020
	private float recoilH;

	// Token: 0x040007E5 RID: 2021
	private float recoilRecover;

	// Token: 0x040007E6 RID: 2022
	private bool triggerInput;

	// Token: 0x040007E7 RID: 2023
	private Vector2 recoilNeedToRecover;

	// Token: 0x040007E8 RID: 2024
	private Vector2 inputMousePosition;

	// Token: 0x040007E9 RID: 2025
	private Vector2 _aimMousePosCache;

	// Token: 0x040007EA RID: 2026
	private bool aimMousePosFirstSynced;

	// Token: 0x040007EB RID: 2027
	private bool cursorVisable = true;

	// Token: 0x040007EC RID: 2028
	private bool aimingEnemyHead;

	// Token: 0x040007ED RID: 2029
	private bool currentFocus = true;

	// Token: 0x040007EE RID: 2030
	private float fovCache = -1f;

	// Token: 0x040007EF RID: 2031
	private float _oppositeDelta;

	// Token: 0x040007F0 RID: 2032
	private float recoilTimer;

	// Token: 0x040007F1 RID: 2033
	private float recoilTime = 0.04f;

	// Token: 0x040007F2 RID: 2034
	private float recoilRecoverTime = 0.1f;

	// Token: 0x040007F3 RID: 2035
	private Vector2 recoilThisShot;

	// Token: 0x02000487 RID: 1159
	public enum InputDevices
	{
		// Token: 0x04001BA3 RID: 7075
		mouseKeyboard,
		// Token: 0x04001BA4 RID: 7076
		touch
	}
}
