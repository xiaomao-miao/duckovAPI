using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class LevelInitializingIndicator : MonoBehaviour
{
	// Token: 0x06000AC6 RID: 2758 RVA: 0x0002E874 File Offset: 0x0002CA74
	private void Awake()
	{
		SceneLoader.onBeforeSetSceneActive += this.SceneLoader_onBeforeSetSceneActive;
		SceneLoader.onAfterSceneInitialize += this.SceneLoader_onAfterSceneInitialize;
		LevelManager.OnLevelInitializingCommentChanged += this.OnCommentChanged;
		SceneLoader.OnSetLoadingComment += this.OnSetLoadingComment;
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0002E8D0 File Offset: 0x0002CAD0
	private void OnSetLoadingComment(string comment)
	{
		this.levelInitializationCommentText.text = SceneLoader.LoadingComment;
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x0002E8E2 File Offset: 0x0002CAE2
	private void OnCommentChanged(string comment)
	{
		this.levelInitializationCommentText.text = SceneLoader.LoadingComment;
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0002E8F4 File Offset: 0x0002CAF4
	private void OnDestroy()
	{
		SceneLoader.onBeforeSetSceneActive -= this.SceneLoader_onBeforeSetSceneActive;
		SceneLoader.onAfterSceneInitialize -= this.SceneLoader_onAfterSceneInitialize;
		LevelManager.OnLevelInitializingCommentChanged -= this.OnCommentChanged;
		SceneLoader.OnSetLoadingComment -= this.OnSetLoadingComment;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x0002E945 File Offset: 0x0002CB45
	private void SceneLoader_onBeforeSetSceneActive(SceneLoadingContext obj)
	{
		this.fadeGroup.Show();
		this.levelInitializationCommentText.text = LevelManager.LevelInitializingComment;
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0002E962 File Offset: 0x0002CB62
	private void SceneLoader_onAfterSceneInitialize(SceneLoadingContext obj)
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x04000953 RID: 2387
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000954 RID: 2388
	[SerializeField]
	private TextMeshProUGUI levelInitializationCommentText;
}
