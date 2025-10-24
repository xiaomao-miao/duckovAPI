using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class DarkRoomFade : MonoBehaviour
{
	// Token: 0x06000B65 RID: 2917 RVA: 0x00030448 File Offset: 0x0002E648
	public void StartFade()
	{
		this.started = true;
		base.enabled = true;
		this.startPos = CharacterMainControl.Main.transform.position;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0003046D File Offset: 0x0002E66D
	private void Awake()
	{
		this.range = 0f;
		this.UpdateMaterial();
		if (!this.started)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00030490 File Offset: 0x0002E690
	private void Update()
	{
		if (!this.started)
		{
			base.enabled = false;
		}
		this.range += this.speed * Time.deltaTime;
		this.UpdateMaterial();
		if (this.range > this.maxRange)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x000304E0 File Offset: 0x0002E6E0
	private void UpdateMaterial()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.SetFloat("_Range", this.range);
		materialPropertyBlock.SetVector("_CenterPos", this.startPos);
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00030538 File Offset: 0x0002E738
	private void Collect()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sharedMaterial.SetFloat("_Range", 0f);
		}
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00030580 File Offset: 0x0002E780
	public void SetRenderers(bool enable)
	{
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = enable;
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x000305AC File Offset: 0x0002E7AC
	public static void SetRenderersEnable(bool enable)
	{
		DarkRoomFade[] array = UnityEngine.Object.FindObjectsByType<DarkRoomFade>(FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetRenderers(enable);
		}
	}

	// Token: 0x040009B5 RID: 2485
	public float maxRange = 100f;

	// Token: 0x040009B6 RID: 2486
	public float speed = 20f;

	// Token: 0x040009B7 RID: 2487
	public Renderer[] renderers;

	// Token: 0x040009B8 RID: 2488
	private Vector3 startPos;

	// Token: 0x040009B9 RID: 2489
	private float range;

	// Token: 0x040009BA RID: 2490
	private bool started;
}
