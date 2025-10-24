using System;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class MoveRing : MonoBehaviour
{
	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00030F07 File Offset: 0x0002F107
	private CharacterMainControl character
	{
		get
		{
			return this.inputManager.characterMainControl;
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00030F14 File Offset: 0x0002F114
	public void SetThreshold(float threshold)
	{
		this.runThreshold = threshold;
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00030F20 File Offset: 0x0002F120
	public void LateUpdate()
	{
		if (!this.inputManager)
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.inputManager = LevelManager.Instance.InputManager;
			return;
		}
		else
		{
			if (!this.character)
			{
				this.SetMove(Vector3.zero, 0f);
				return;
			}
			base.transform.position = this.character.transform.position + Vector3.up * 0.02f;
			this.SetThreshold(this.inputManager.runThreshold);
			this.SetMove(this.inputManager.WorldMoveInput.normalized, this.inputManager.WorldMoveInput.magnitude);
			this.SetRunning(this.character.Running);
			if (this.ring.enabled != this.character.gameObject.activeInHierarchy)
			{
				this.ring.enabled = this.character.gameObject.activeInHierarchy;
			}
			return;
		}
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x0003102C File Offset: 0x0002F22C
	public void SetMove(Vector3 direction, float value)
	{
		if (this.ringMat)
		{
			this.ringMat.SetVector("_Direction", direction);
			this.ringMat.SetFloat("_Distance", value);
			this.ringMat.SetFloat("_Threshold", this.runThreshold);
			return;
		}
		if (!this.ring)
		{
			return;
		}
		this.ringMat = this.ring.material;
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x000310A3 File Offset: 0x0002F2A3
	public void SetRunning(bool running)
	{
		this.ringMat.SetFloat("_Running", (float)(running ? 1 : 0));
	}

	// Token: 0x040009DF RID: 2527
	public Renderer ring;

	// Token: 0x040009E0 RID: 2528
	public float runThreshold;

	// Token: 0x040009E1 RID: 2529
	private Material ringMat;

	// Token: 0x040009E2 RID: 2530
	private InputManager inputManager;
}
