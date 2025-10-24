using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Scenes;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Token: 0x0200012A RID: 298
public class SceneLoader : MonoBehaviour
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00029C09 File Offset: 0x00027E09
	public static SceneLoader Instance
	{
		get
		{
			return GameManager.SceneLoader;
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00029C10 File Offset: 0x00027E10
	// (set) Token: 0x060009B2 RID: 2482 RVA: 0x00029C17 File Offset: 0x00027E17
	public static bool IsSceneLoading { get; private set; }

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x060009B3 RID: 2483 RVA: 0x00029C20 File Offset: 0x00027E20
	// (remove) Token: 0x060009B4 RID: 2484 RVA: 0x00029C54 File Offset: 0x00027E54
	public static event Action<SceneLoadingContext> onStartedLoadingScene;

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x060009B5 RID: 2485 RVA: 0x00029C88 File Offset: 0x00027E88
	// (remove) Token: 0x060009B6 RID: 2486 RVA: 0x00029CBC File Offset: 0x00027EBC
	public static event Action<SceneLoadingContext> onFinishedLoadingScene;

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x060009B7 RID: 2487 RVA: 0x00029CF0 File Offset: 0x00027EF0
	// (remove) Token: 0x060009B8 RID: 2488 RVA: 0x00029D24 File Offset: 0x00027F24
	public static event Action<SceneLoadingContext> onBeforeSetSceneActive;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060009B9 RID: 2489 RVA: 0x00029D58 File Offset: 0x00027F58
	// (remove) Token: 0x060009BA RID: 2490 RVA: 0x00029D8C File Offset: 0x00027F8C
	public static event Action<SceneLoadingContext> onAfterSceneInitialize;

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x00029DBF File Offset: 0x00027FBF
	// (set) Token: 0x060009BC RID: 2492 RVA: 0x00029DE7 File Offset: 0x00027FE7
	public static string LoadingComment
	{
		get
		{
			if (LevelManager.LevelInitializing)
			{
				return LevelManager.LevelInitializingComment;
			}
			if (SceneLoader.Instance != null)
			{
				return SceneLoader.Instance._loadingComment;
			}
			return null;
		}
		set
		{
			if (SceneLoader.Instance == null)
			{
				return;
			}
			SceneLoader.Instance._loadingComment = value;
			Action<string> onSetLoadingComment = SceneLoader.OnSetLoadingComment;
			if (onSetLoadingComment == null)
			{
				return;
			}
			onSetLoadingComment(value);
		}
	}

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060009BD RID: 2493 RVA: 0x00029E14 File Offset: 0x00028014
	// (remove) Token: 0x060009BE RID: 2494 RVA: 0x00029E48 File Offset: 0x00028048
	public static event Action<string> OnSetLoadingComment;

	// Token: 0x060009BF RID: 2495 RVA: 0x00029E7C File Offset: 0x0002807C
	private void Awake()
	{
		if (SceneLoader.Instance != this)
		{
			Debug.LogError(base.gameObject.scene.name + " 场景中出现了应当删除的Scene Loader");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.pointerClickEventRecevier.onPointerClick.AddListener(new UnityAction<PointerEventData>(this.NotifyPointerClick));
		this.pointerClickEventRecevier.gameObject.SetActive(false);
		this.content.Hide();
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00029EFC File Offset: 0x000280FC
	public UniTask LoadScene(string sceneID, MultiSceneLocation location, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__39 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneID = sceneID;
		<LoadScene>d__.location = location;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__39>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00029F84 File Offset: 0x00028184
	public UniTask LoadScene(string sceneID, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool useLocation = false, MultiSceneLocation location = default(MultiSceneLocation), bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__40 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneID = sceneID;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.useLocation = useLocation;
		<LoadScene>d__.location = location;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__40>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0002A015 File Offset: 0x00028215
	// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0002A01C File Offset: 0x0002821C
	public static bool HideTips { get; private set; }

	// Token: 0x060009C4 RID: 2500 RVA: 0x0002A024 File Offset: 0x00028224
	public UniTask LoadScene(SceneReference sceneReference, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool useLocation = false, MultiSceneLocation location = default(MultiSceneLocation), bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__45 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneReference = sceneReference;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.useLocation = useLocation;
		<LoadScene>d__.location = location;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__45>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0002A0B8 File Offset: 0x000282B8
	public void LoadTarget()
	{
		this.LoadScene(this.target, null, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0002A0E8 File Offset: 0x000282E8
	public UniTask LoadBaseScene(SceneReference overrideCurtainScene = null, bool doCircleFade = true)
	{
		SceneLoader.<LoadBaseScene>d__47 <LoadBaseScene>d__;
		<LoadBaseScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadBaseScene>d__.<>4__this = this;
		<LoadBaseScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadBaseScene>d__.doCircleFade = doCircleFade;
		<LoadBaseScene>d__.<>1__state = -1;
		<LoadBaseScene>d__.<>t__builder.Start<SceneLoader.<LoadBaseScene>d__47>(ref <LoadBaseScene>d__);
		return <LoadBaseScene>d__.<>t__builder.Task;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0002A13B File Offset: 0x0002833B
	public void NotifyPointerClick(PointerEventData eventData)
	{
		this.clicked = true;
		AudioManager.Post("UI/sceneloader_click");
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0002A14F File Offset: 0x0002834F
	internal static void StaticLoadSingle(SceneReference sceneReference)
	{
		SceneManager.LoadScene(sceneReference.Name, LoadSceneMode.Single);
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0002A15D File Offset: 0x0002835D
	internal static void StaticLoadSingle(string sceneID)
	{
		SceneManager.LoadScene(SceneInfoCollection.GetBuildIndex(sceneID), LoadSceneMode.Single);
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0002A16C File Offset: 0x0002836C
	public static void LoadMainMenu(bool circleFade = true)
	{
		if (SceneLoader.Instance)
		{
			SceneLoader.Instance.LoadScene(GameplayDataSettings.SceneManagement.MainMenuScene, null, false, false, circleFade, false, default(MultiSceneLocation), true, false).Forget();
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0002A1CC File Offset: 0x000283CC
	[CompilerGenerated]
	internal static float <LoadScene>g__TimeSinceLoadingStarted|45_0(ref SceneLoader.<>c__DisplayClass45_0 A_0)
	{
		return Time.unscaledTime - A_0.timeWhenLoadingStarted;
	}

	// Token: 0x04000876 RID: 2166
	public SceneReference defaultCurtainScene;

	// Token: 0x04000877 RID: 2167
	[SerializeField]
	private OnPointerClick pointerClickEventRecevier;

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private float minimumLoadingTime = 1f;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private float waitAfterSceneLoaded = 1f;

	// Token: 0x0400087A RID: 2170
	[SerializeField]
	private FadeGroup content;

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	private FadeGroup loadingIndicator;

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	private FadeGroup clickIndicator;

	// Token: 0x0400087D RID: 2173
	[SerializeField]
	private AnimationCurve fadeCurve1;

	// Token: 0x0400087E RID: 2174
	[SerializeField]
	private AnimationCurve fadeCurve2;

	// Token: 0x0400087F RID: 2175
	[SerializeField]
	private AnimationCurve fadeCurve3;

	// Token: 0x04000880 RID: 2176
	[SerializeField]
	private AnimationCurve fadeCurve4;

	// Token: 0x04000886 RID: 2182
	private string _loadingComment;

	// Token: 0x04000888 RID: 2184
	[SerializeField]
	private SceneReference target;

	// Token: 0x04000889 RID: 2185
	private bool clicked;
}
