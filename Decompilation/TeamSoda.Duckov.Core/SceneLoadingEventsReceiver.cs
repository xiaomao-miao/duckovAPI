using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200012B RID: 299
public class SceneLoadingEventsReceiver : MonoBehaviour
{
	// Token: 0x060009CD RID: 2509 RVA: 0x0002A1DA File Offset: 0x000283DA
	private void OnEnable()
	{
		SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
		SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0002A1FE File Offset: 0x000283FE
	private void OnDisable()
	{
		SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
		SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0002A222 File Offset: 0x00028422
	private void OnStartedLoadingScene(SceneLoadingContext context)
	{
		UnityEvent unityEvent = this.onStartLoadingScene;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0002A234 File Offset: 0x00028434
	private void OnFinishedLoadingScene(SceneLoadingContext context)
	{
		UnityEvent unityEvent = this.onFinishedLoadingScene;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x0400088B RID: 2187
	public UnityEvent onStartLoadingScene;

	// Token: 0x0400088C RID: 2188
	public UnityEvent onFinishedLoadingScene;
}
