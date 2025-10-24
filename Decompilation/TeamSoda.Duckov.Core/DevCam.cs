using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001B6 RID: 438
public class DevCam : MonoBehaviour
{
	// Token: 0x06000CF0 RID: 3312 RVA: 0x00035CF5 File Offset: 0x00033EF5
	private void Awake()
	{
		this.root.gameObject.SetActive(false);
		Shader.SetGlobalFloat("DevCamOn", 0f);
		DevCam.devCamOn = false;
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00035D20 File Offset: 0x00033F20
	private void Toggle()
	{
		this.active = true;
		DevCam.devCamOn = this.active;
		Shader.SetGlobalFloat("DevCamOn", this.active ? 1f : 0f);
		this.root.gameObject.SetActive(this.active);
		for (int i = 0; i < Display.displays.Length; i++)
		{
			if (i == 1 && this.active)
			{
				Display.displays[i].Activate();
			}
		}
		UniversalRenderPipelineAsset universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
		if (universalRenderPipelineAsset != null)
		{
			universalRenderPipelineAsset.shadowDistance = 500f;
		}
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x00035DBC File Offset: 0x00033FBC
	private void OnDestroy()
	{
		DevCam.devCamOn = false;
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x00035DC4 File Offset: 0x00033FC4
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (Gamepad.all.Count <= 0)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer <= 0f)
		{
			this.timer = 0f;
			this.pressCounter = 0;
		}
		if (Gamepad.current.leftStickButton.isPressed && Gamepad.current.rightStickButton.wasPressedThisFrame)
		{
			this.pressCounter++;
			this.timer = 1.5f;
			Debug.Log("Toggle Dev Cam");
			if (this.pressCounter >= 2)
			{
				this.pressCounter = 0;
				this.Toggle();
			}
		}
		if (CharacterMainControl.Main != null)
		{
			this.postTarget.position = CharacterMainControl.Main.transform.position;
		}
	}

	// Token: 0x04000B26 RID: 2854
	public Camera devCamera;

	// Token: 0x04000B27 RID: 2855
	public Transform postTarget;

	// Token: 0x04000B28 RID: 2856
	private bool active;

	// Token: 0x04000B29 RID: 2857
	public Transform root;

	// Token: 0x04000B2A RID: 2858
	public static bool devCamOn;

	// Token: 0x04000B2B RID: 2859
	private float timer = 1.5f;

	// Token: 0x04000B2C RID: 2860
	private int pressCounter;
}
