using System;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class CharacterPosInShader : MonoBehaviour
{
	// Token: 0x06000870 RID: 2160 RVA: 0x000258F7 File Offset: 0x00023AF7
	private void Update()
	{
		if (!CharacterMainControl.Main)
		{
			return;
		}
		Shader.SetGlobalVector(this.characterPosHash, CharacterMainControl.Main.transform.position);
	}

	// Token: 0x040007AA RID: 1962
	private int characterPosHash = Shader.PropertyToID("CharacterPos");
}
