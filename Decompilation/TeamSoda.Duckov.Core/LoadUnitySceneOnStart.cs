using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000126 RID: 294
public class LoadUnitySceneOnStart : MonoBehaviour
{
	// Token: 0x06000999 RID: 2457 RVA: 0x00029977 File Offset: 0x00027B77
	private void Start()
	{
		SceneManager.LoadScene(this.sceneIndex);
	}

	// Token: 0x0400086C RID: 2156
	public int sceneIndex;
}
