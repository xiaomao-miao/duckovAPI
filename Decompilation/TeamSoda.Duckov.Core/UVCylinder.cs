using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class UVCylinder : MonoBehaviour
{
	// Token: 0x06000A55 RID: 2645 RVA: 0x0002D89C File Offset: 0x0002BA9C
	private void Generate()
	{
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
		}
		this.mesh.Clear();
		new List<Vector3>();
		new List<Vector2>();
		new List<Vector3>();
		new List<int>();
		for (int i = 0; i < this.subdivision; i++)
		{
		}
	}

	// Token: 0x04000906 RID: 2310
	public float radius = 1f;

	// Token: 0x04000907 RID: 2311
	public float height = 2f;

	// Token: 0x04000908 RID: 2312
	public int subdivision = 16;

	// Token: 0x04000909 RID: 2313
	private Mesh mesh;
}
