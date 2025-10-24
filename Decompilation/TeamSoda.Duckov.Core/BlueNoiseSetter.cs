using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class BlueNoiseSetter : MonoBehaviour
{
	// Token: 0x06000A15 RID: 2581 RVA: 0x0002B210 File Offset: 0x00029410
	private void Update()
	{
		Shader.SetGlobalTexture("GlobalBlueNoise", this.blueNoises[this.index]);
		this.index++;
		if (this.index >= this.blueNoises.Count)
		{
			this.index = 0;
		}
	}

	// Token: 0x040008CE RID: 2254
	public List<Texture2D> blueNoises;

	// Token: 0x040008CF RID: 2255
	private int index;
}
