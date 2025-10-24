using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000027 RID: 39
[ExecuteInEditMode]
public class SodaPointLight : MonoBehaviour
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060000DA RID: 218 RVA: 0x00004776 File Offset: 0x00002976
	// (set) Token: 0x060000DB RID: 219 RVA: 0x0000477E File Offset: 0x0000297E
	public float FallOff
	{
		get
		{
			return this.fallOff;
		}
		set
		{
			this.fallOff = value;
			this.SyncToLight();
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060000DC RID: 220 RVA: 0x0000478D File Offset: 0x0000298D
	// (set) Token: 0x060000DD RID: 221 RVA: 0x00004795 File Offset: 0x00002995
	public float Hardness
	{
		get
		{
			return this.hardness;
		}
		set
		{
			this.hardness = value;
			this.SyncToLight();
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060000DE RID: 222 RVA: 0x000047A4 File Offset: 0x000029A4
	// (set) Token: 0x060000DF RID: 223 RVA: 0x000047AC File Offset: 0x000029AC
	public Color LightColor
	{
		get
		{
			return this.lightColor;
		}
		set
		{
			this.lightColor = value;
			this.SyncToLight();
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000047BB File Offset: 0x000029BB
	private void Awake()
	{
		this.SyncToLight();
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000047C4 File Offset: 0x000029C4
	private void SyncToLight()
	{
		if (this.block == null)
		{
			this.block = new MaterialPropertyBlock();
		}
		this.block.SetFloat(this.hardnessID, this.hardness);
		this.block.SetFloat(this.fallOffID, this.fallOff);
		this.block.SetColor(this.lightColorID, this.LightColor);
		this.block.SetFloat(this.enviromentTintID, this.enviromentTint ? 1f : 0f);
		if (this.lightRenderer)
		{
			this.lightRenderer.SetPropertyBlock(this.block);
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000486C File Offset: 0x00002A6C
	private void OnValidate()
	{
		this.SyncToLight();
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00004874 File Offset: 0x00002A74
	private void OnDrawGizmosSelected()
	{
		Vector3 lossyScale = base.transform.lossyScale;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Color color = this.lightColor;
		color.a = 1f;
		Gizmos.color = color;
		Gizmos.DrawWireSphere(Vector3.zero, 1f);
	}

	// Token: 0x04000046 RID: 70
	[SerializeField]
	private Renderer lightRenderer;

	// Token: 0x04000047 RID: 71
	[SerializeField]
	[Range(0f, 1f)]
	private float hardness = 0.5f;

	// Token: 0x04000048 RID: 72
	[SerializeField]
	[Range(1f, 5f)]
	private float fallOff = 1f;

	// Token: 0x04000049 RID: 73
	[FormerlySerializedAs("dayColor")]
	[SerializeField]
	[ColorUsage(false, true)]
	private Color lightColor = Color.white;

	// Token: 0x0400004A RID: 74
	public bool enviromentTint;

	// Token: 0x0400004B RID: 75
	private int lightColorID = Shader.PropertyToID("_LightColor");

	// Token: 0x0400004C RID: 76
	private int hardnessID = Shader.PropertyToID("_Hardness");

	// Token: 0x0400004D RID: 77
	private int fallOffID = Shader.PropertyToID("_FallOff");

	// Token: 0x0400004E RID: 78
	private int enviromentTintID = Shader.PropertyToID("_EnviromentTintOn");

	// Token: 0x0400004F RID: 79
	private MaterialPropertyBlock block;
}
