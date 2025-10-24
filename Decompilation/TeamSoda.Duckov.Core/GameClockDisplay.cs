using System;
using TMPro;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class GameClockDisplay : MonoBehaviour
{
	// Token: 0x06000D20 RID: 3360 RVA: 0x000369F5 File Offset: 0x00034BF5
	private void Awake()
	{
		this.Refresh();
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x000369FD File Offset: 0x00034BFD
	private void OnEnable()
	{
		GameClock.OnGameClockStep += this.Refresh;
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x00036A10 File Offset: 0x00034C10
	private void OnDisable()
	{
		GameClock.OnGameClockStep -= this.Refresh;
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x00036A24 File Offset: 0x00034C24
	private void Refresh()
	{
		string text;
		if (GameClock.Instance == null)
		{
			text = "--:--";
		}
		else
		{
			text = string.Format("{0:00}:{1:00}", GameClock.Hour, GameClock.Minut);
		}
		this.text.text = text;
	}

	// Token: 0x04000B48 RID: 2888
	[SerializeField]
	private TextMeshProUGUI text;
}
