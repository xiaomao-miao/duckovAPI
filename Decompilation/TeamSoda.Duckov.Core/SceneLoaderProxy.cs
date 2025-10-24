using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class SceneLoaderProxy : MonoBehaviour
{
	// Token: 0x0600094F RID: 2383 RVA: 0x00029149 File Offset: 0x00027349
	public void LoadScene()
	{
		if (SceneLoader.Instance == null)
		{
			Debug.LogWarning("没找到SceneLoader实例，已取消加载场景");
			return;
		}
		InputManager.DisableInput(base.gameObject);
		this.Task().Forget();
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0002917C File Offset: 0x0002737C
	private UniTask Task()
	{
		SceneLoaderProxy.<Task>d__10 <Task>d__;
		<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Task>d__.<>4__this = this;
		<Task>d__.<>1__state = -1;
		<Task>d__.<>t__builder.Start<SceneLoaderProxy.<Task>d__10>(ref <Task>d__);
		return <Task>d__.<>t__builder.Task;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x000291BF File Offset: 0x000273BF
	public void LoadMainMenu()
	{
		SceneLoader.LoadMainMenu(this.circleFade);
	}

	// Token: 0x04000846 RID: 2118
	[SceneID]
	[SerializeField]
	private string sceneID;

	// Token: 0x04000847 RID: 2119
	[SerializeField]
	private bool useLocation;

	// Token: 0x04000848 RID: 2120
	[SerializeField]
	private MultiSceneLocation location;

	// Token: 0x04000849 RID: 2121
	[SerializeField]
	private bool showClosure = true;

	// Token: 0x0400084A RID: 2122
	[SerializeField]
	private bool notifyEvacuation = true;

	// Token: 0x0400084B RID: 2123
	[SerializeField]
	private SceneReference overrideCurtainScene;

	// Token: 0x0400084C RID: 2124
	[SerializeField]
	private bool hideTips;

	// Token: 0x0400084D RID: 2125
	[SerializeField]
	private bool circleFade = true;

	// Token: 0x0400084E RID: 2126
	private bool saveToFile;
}
