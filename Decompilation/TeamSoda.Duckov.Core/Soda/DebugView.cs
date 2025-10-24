using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Soda
{
	// Token: 0x02000227 RID: 551
	public class DebugView : MonoBehaviour
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00040BE1 File Offset: 0x0003EDE1
		public DebugView Instance
		{
			get
			{
				return this.instance;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x00040BE9 File Offset: 0x0003EDE9
		public bool EdgeLightActive
		{
			get
			{
				return this.edgeLightActive;
			}
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x060010C0 RID: 4288 RVA: 0x00040BF4 File Offset: 0x0003EDF4
		// (remove) Token: 0x060010C1 RID: 4289 RVA: 0x00040C28 File Offset: 0x0003EE28
		public static event Action<DebugView> OnDebugViewConfigChanged;

		// Token: 0x060010C2 RID: 4290 RVA: 0x00040C5B File Offset: 0x0003EE5B
		private void Awake()
		{
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00040C5D File Offset: 0x0003EE5D
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnlevelInited;
			SceneManager.activeSceneChanged -= this.OnSceneLoaded;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00040C84 File Offset: 0x0003EE84
		private void InitFromData()
		{
			if (PlayerPrefs.HasKey("ResMode"))
			{
				this.resMode = (ResModes)PlayerPrefs.GetInt("ResMode");
			}
			else
			{
				this.resMode = ResModes.R720p;
			}
			if (PlayerPrefs.HasKey("TexMode"))
			{
				this.texMode = (TextureModes)PlayerPrefs.GetInt("TexMode");
			}
			else
			{
				this.texMode = TextureModes.High;
			}
			if (PlayerPrefs.HasKey("InputDevice"))
			{
				this.inputDevice = PlayerPrefs.GetInt("InputDevice");
			}
			else
			{
				this.inputDevice = 1;
			}
			if (PlayerPrefs.HasKey("BloomActive"))
			{
				this.bloomActive = (PlayerPrefs.GetInt("BloomActive") != 0);
			}
			else
			{
				this.bloomActive = true;
			}
			if (PlayerPrefs.HasKey("EdgeLightActive"))
			{
				this.edgeLightActive = (PlayerPrefs.GetInt("EdgeLightActive") != 0);
			}
			else
			{
				this.edgeLightActive = true;
			}
			if (PlayerPrefs.HasKey("AOActive"))
			{
				this.aoActive = (PlayerPrefs.GetInt("AOActive") != 0);
			}
			else
			{
				this.aoActive = false;
			}
			if (PlayerPrefs.HasKey("DofActive"))
			{
				this.dofActive = (PlayerPrefs.GetInt("DofActive") != 0);
			}
			else
			{
				this.dofActive = false;
			}
			if (PlayerPrefs.HasKey("ReporterActive"))
			{
				this.reporterActive = (PlayerPrefs.GetInt("ReporterActive") != 0);
				return;
			}
			this.reporterActive = false;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00040DC8 File Offset: 0x0003EFC8
		private void Update()
		{
			this.deltaTimes[this.frameIndex] = Time.deltaTime;
			this.frameIndex++;
			if (this.frameIndex >= this.frameSampleCount)
			{
				this.frameIndex = 0;
				float num = 0f;
				for (int i = 0; i < this.frameSampleCount; i++)
				{
					num += this.deltaTimes[i];
				}
				int num2 = Mathf.RoundToInt((float)this.frameSampleCount / Mathf.Max(0.0001f, num));
				this.fpsText1.text = num2.ToString();
				this.fpsText2.text = num2.ToString();
			}
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00040E6C File Offset: 0x0003F06C
		public void SetInputDevice(int type)
		{
			if (!true)
			{
				InputManager.SetInputDevice(InputManager.InputDevices.touch);
				this.inputDeviceText.text = "触摸";
				PlayerPrefs.SetInt("InputDevice", 0);
				return;
			}
			InputManager.SetInputDevice(InputManager.InputDevices.mouseKeyboard);
			this.inputDeviceText.text = "键鼠";
			PlayerPrefs.SetInt("InputDevice", 1);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00040EC2 File Offset: 0x0003F0C2
		public void SetRes(int resModeIndex)
		{
			this.SetRes((ResModes)resModeIndex);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00040ECC File Offset: 0x0003F0CC
		public void SetRes(ResModes mode)
		{
			this.resMode = mode;
			this.screenRes.x = (float)Display.main.systemWidth;
			this.screenRes.y = (float)Display.main.systemHeight;
			PlayerPrefs.SetInt("ResMode", (int)mode);
			int num = 1;
			int num2 = 1;
			switch (this.resMode)
			{
			case ResModes.Source:
				num = Mathf.RoundToInt(this.screenRes.x);
				num2 = Mathf.RoundToInt(this.screenRes.y);
				break;
			case ResModes.HalfRes:
				num = Mathf.RoundToInt(this.screenRes.x / 2f);
				num2 = Mathf.RoundToInt(this.screenRes.y / 2f);
				break;
			case ResModes.R720p:
				num = Mathf.RoundToInt(this.screenRes.x / this.screenRes.y * 720f);
				num2 = 720;
				break;
			case ResModes.R480p:
				num = Mathf.RoundToInt(this.screenRes.x / this.screenRes.y * 480f);
				num2 = 480;
				break;
			}
			this.resText.text = string.Format("{0}x{1}", num, num2);
			Screen.SetResolution(num, num2, FullScreenMode.FullScreenWindow);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00041021 File Offset: 0x0003F221
		public void SetTexture(int texModeIndex)
		{
			this.SetTexture((TextureModes)texModeIndex);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0004102C File Offset: 0x0003F22C
		public void SetTexture(TextureModes mode)
		{
			this.texMode = mode;
			QualitySettings.globalTextureMipmapLimit = (int)this.texMode;
			switch (this.texMode)
			{
			case TextureModes.High:
				this.texText.text = "高";
				break;
			case TextureModes.Middle:
				this.texText.text = "中";
				break;
			case TextureModes.Low:
				this.texText.text = "低";
				break;
			case TextureModes.VeryLow:
				this.texText.text = "极低";
				break;
			}
			PlayerPrefs.SetInt("TexMode", (int)this.texMode);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x000410D0 File Offset: 0x0003F2D0
		private void OnlevelInited()
		{
			this.SetInvincible(this.invincible);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x000410E0 File Offset: 0x0003F2E0
		private void OnSceneLoaded(Scene s1, Scene s2)
		{
			this.SetShadow().Forget();
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000410FC File Offset: 0x0003F2FC
		private UniTaskVoid SetShadow()
		{
			DebugView.<SetShadow>d__49 <SetShadow>d__;
			<SetShadow>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SetShadow>d__.<>4__this = this;
			<SetShadow>d__.<>1__state = -1;
			<SetShadow>d__.<>t__builder.Start<DebugView.<SetShadow>d__49>(ref <SetShadow>d__);
			return <SetShadow>d__.<>t__builder.Task;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0004113F File Offset: 0x0003F33F
		public void ToggleBloom()
		{
			this.bloomActive = !this.bloomActive;
			this.SetBloom(this.bloomActive);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0004115C File Offset: 0x0003F35C
		private void SetBloom(bool active)
		{
			Bloom bloom;
			bool flag = this.volumeProfile.TryGet<Bloom>(out bloom);
			this.bloomText.text = (active ? "开" : "关");
			if (flag)
			{
				bloom.active = active;
			}
			this.bloomActive = active;
			PlayerPrefs.SetInt("BloomActive", this.bloomActive ? 1 : 0);
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x000411C6 File Offset: 0x0003F3C6
		public void ToggleEdgeLight()
		{
			this.edgeLightActive = !this.edgeLightActive;
			this.SetEdgeLight(this.edgeLightActive);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x000411E4 File Offset: 0x0003F3E4
		private void SetEdgeLight(bool active)
		{
			this.edgeLightText.text = (active ? "开" : "关");
			this.edgeLightActive = active;
			PlayerPrefs.SetInt("EdgeLightActive", this.edgeLightActive ? 1 : 0);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.supportsCameraDepthTexture = active;
			}
			this.SetShadow();
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00041260 File Offset: 0x0003F460
		public void ToggleAO()
		{
			this.aoActive = !this.aoActive;
			this.SetAO(this.aoActive);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0004127D File Offset: 0x0003F47D
		public void ToggleDof()
		{
			this.dofActive = !this.dofActive;
			this.SetDof(this.dofActive);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0004129A File Offset: 0x0003F49A
		public void ToggleInvincible()
		{
			this.invincible = !this.invincible;
			this.SetInvincible(this.invincible);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x000412B7 File Offset: 0x0003F4B7
		private void SetReporter(bool active)
		{
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000412B9 File Offset: 0x0003F4B9
		public void ToggleReporter()
		{
			this.SetReporter(!this.reporterActive);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000412CC File Offset: 0x0003F4CC
		private void SetAO(bool active)
		{
			ScriptableRendererFeature scriptableRendererFeature = this.rendererData.rendererFeatures.Find((ScriptableRendererFeature a) => a.name == "ScreenSpaceAmbientOcclusion");
			if (scriptableRendererFeature != null)
			{
				scriptableRendererFeature.SetActive(active);
				this.aoText.text = (active ? "开" : "关");
				PlayerPrefs.SetInt("AOActive", active ? 1 : 0);
			}
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00041354 File Offset: 0x0003F554
		private void SetDof(bool active)
		{
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00041356 File Offset: 0x0003F556
		private void SetInvincible(bool active)
		{
			this.invincibleText.text = (active ? "开" : "关");
			this.invincible = active;
			Action<DebugView> onDebugViewConfigChanged = DebugView.OnDebugViewConfigChanged;
			if (onDebugViewConfigChanged == null)
			{
				return;
			}
			onDebugViewConfigChanged(this);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0004138C File Offset: 0x0003F58C
		public void CreateItem()
		{
			this.CreateItemTask().Forget();
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x000413A8 File Offset: 0x0003F5A8
		private UniTaskVoid CreateItemTask()
		{
			DebugView.<CreateItemTask>d__63 <CreateItemTask>d__;
			<CreateItemTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<CreateItemTask>d__.<>4__this = this;
			<CreateItemTask>d__.<>1__state = -1;
			<CreateItemTask>d__.<>t__builder.Start<DebugView.<CreateItemTask>d__63>(ref <CreateItemTask>d__);
			return <CreateItemTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000D28 RID: 3368
		private DebugView instance;

		// Token: 0x04000D29 RID: 3369
		private Vector2 screenRes;

		// Token: 0x04000D2A RID: 3370
		private ResModes resMode;

		// Token: 0x04000D2B RID: 3371
		private TextureModes texMode;

		// Token: 0x04000D2C RID: 3372
		public TextMeshProUGUI resText;

		// Token: 0x04000D2D RID: 3373
		public TextMeshProUGUI texText;

		// Token: 0x04000D2E RID: 3374
		public TextMeshProUGUI fpsText1;

		// Token: 0x04000D2F RID: 3375
		public TextMeshProUGUI fpsText2;

		// Token: 0x04000D30 RID: 3376
		public TextMeshProUGUI inputDeviceText;

		// Token: 0x04000D31 RID: 3377
		public TextMeshProUGUI bloomText;

		// Token: 0x04000D32 RID: 3378
		public TextMeshProUGUI edgeLightText;

		// Token: 0x04000D33 RID: 3379
		public TextMeshProUGUI aoText;

		// Token: 0x04000D34 RID: 3380
		public TextMeshProUGUI dofText;

		// Token: 0x04000D35 RID: 3381
		public TextMeshProUGUI invincibleText;

		// Token: 0x04000D36 RID: 3382
		public TextMeshProUGUI reporterText;

		// Token: 0x04000D37 RID: 3383
		public UniversalRendererData rendererData;

		// Token: 0x04000D38 RID: 3384
		private float[] deltaTimes;

		// Token: 0x04000D39 RID: 3385
		private int frameIndex;

		// Token: 0x04000D3A RID: 3386
		public int frameSampleCount = 30;

		// Token: 0x04000D3B RID: 3387
		public GameObject openButton;

		// Token: 0x04000D3C RID: 3388
		public GameObject panel;

		// Token: 0x04000D3D RID: 3389
		public VolumeProfile volumeProfile;

		// Token: 0x04000D3E RID: 3390
		private bool bloomActive;

		// Token: 0x04000D3F RID: 3391
		private bool edgeLightActive;

		// Token: 0x04000D40 RID: 3392
		private bool aoActive;

		// Token: 0x04000D41 RID: 3393
		private int inputDevice;

		// Token: 0x04000D42 RID: 3394
		private bool dofActive;

		// Token: 0x04000D43 RID: 3395
		private bool invincible;

		// Token: 0x04000D44 RID: 3396
		private bool reporterActive;

		// Token: 0x04000D45 RID: 3397
		private Light light;

		// Token: 0x04000D46 RID: 3398
		[ItemTypeID]
		public int createItemID;
	}
}
