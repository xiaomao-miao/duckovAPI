using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules.UI;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class GamePrepareProcess : MonoBehaviour
{
	// Token: 0x06000E5B RID: 3675 RVA: 0x00039A34 File Offset: 0x00037C34
	private UniTask Execute()
	{
		GamePrepareProcess.<Execute>d__6 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<GamePrepareProcess.<Execute>d__6>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00039A77 File Offset: 0x00037C77
	private void Start()
	{
		this.Execute().Forget();
	}

	// Token: 0x04000BD1 RID: 3025
	[SerializeField]
	private DifficultySelection difficultySelection;

	// Token: 0x04000BD2 RID: 3026
	[SerializeField]
	[SceneID]
	private string introScene;

	// Token: 0x04000BD3 RID: 3027
	[SerializeField]
	[SceneID]
	private string guideScene;

	// Token: 0x04000BD4 RID: 3028
	public bool goToBaseSceneIfVisted;

	// Token: 0x04000BD5 RID: 3029
	[SerializeField]
	[SceneID]
	private string baseScene;

	// Token: 0x04000BD6 RID: 3030
	public SceneReference overrideCurtainScene;
}
