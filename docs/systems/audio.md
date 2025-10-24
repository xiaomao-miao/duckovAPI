# 音频系统

## 概述
音频系统是《逃离鸭科夫》的重要系统之一，负责管理游戏中的声音播放、3D音频、声音类型设置等功能。

## 核心组件

### AudioManager
音频管理器，管理游戏中的音频播放。

#### 主要功能
- **声音播放** - 播放、停止声音
- **3D音频** - 在指定位置播放声音
- **声音类型** - 设置角色声音类型
- **脚步声** - 设置脚步声材质类型
- **状态管理** - 管理音频状态

#### 重要属性
- `Instance` - 获取AudioManager实例
- `IsStingerPlaying` - 是否正在播放Stinger音效

#### 重要方法
- `PlaySound(string eventName)` - 播放声音
- `PlaySound(string eventName, Vector3 position)` - 在指定位置播放声音
- `StopSound(string eventName)` - 停止声音
- `SetVoiceType(GameObject obj, VoiceType voiceType)` - 设置声音类型
- `SetFootStepMaterialType(GameObject obj, FootStepMaterialType materialType)` - 设置脚步声材质类型

#### 重要枚举
- `VoiceType` - 声音类型枚举
- `FootStepMaterialType` - 脚步声材质类型枚举

## 使用示例

### 基本声音播放
```csharp
// 播放声音
AudioManager.PlaySound("GunShot");
Debug.Log("播放枪声");

// 停止声音
AudioManager.StopSound("GunShot");
Debug.Log("停止枪声");
```

### 3D音频播放
```csharp
// 在指定位置播放声音
Vector3 explosionPosition = new Vector3(10, 0, 10);
AudioManager.PlaySound("Explosion", explosionPosition);
Debug.Log("在位置播放爆炸声");

// 在角色位置播放声音
CharacterMainControl character = CharacterMainControl.Main;
if (character != null)
{
    AudioManager.PlaySound("FootStep", character.transform.position);
    Debug.Log("在角色位置播放脚步声");
}
```

### 设置声音类型
```csharp
CharacterMainControl character = CharacterMainControl.Main;
if (character != null)
{
    // 设置角色声音类型
    AudioManager.SetVoiceType(character.gameObject, AudioManager.VoiceType.Male);
    Debug.Log("设置角色声音类型为男性");
    
    // 设置脚步声材质类型
    AudioManager.SetFootStepMaterialType(character.gameObject, AudioManager.FootStepMaterialType.Concrete);
    Debug.Log("设置脚步声材质类型为混凝土");
}
```

### 音频系统集成
```csharp
public class AudioSystem
{
    private AudioManager audioManager;
    private CharacterMainControl character;
    
    public AudioSystem()
    {
        audioManager = AudioManager.Instance;
        character = CharacterMainControl.Main;
    }
    
    public void PlayWeaponSound(string weaponType, Vector3 position)
    {
        string soundEvent = GetWeaponSoundEvent(weaponType);
        if (!string.IsNullOrEmpty(soundEvent))
        {
            audioManager.PlaySound(soundEvent, position);
            Debug.Log($"播放武器声音: {soundEvent}");
        }
    }
    
    public void PlayFootStepSound(Vector3 position, FootStepMaterialType materialType)
    {
        string soundEvent = GetFootStepSoundEvent(materialType);
        if (!string.IsNullOrEmpty(soundEvent))
        {
            audioManager.PlaySound(soundEvent, position);
            Debug.Log($"播放脚步声: {soundEvent}");
        }
    }
    
    public void PlayAmbientSound(string ambientType, Vector3 position)
    {
        string soundEvent = GetAmbientSoundEvent(ambientType);
        if (!string.IsNullOrEmpty(soundEvent))
        {
            audioManager.PlaySound(soundEvent, position);
            Debug.Log($"播放环境声音: {soundEvent}");
        }
    }
    
    private string GetWeaponSoundEvent(string weaponType)
    {
        switch (weaponType)
        {
            case "AK47":
                return "GunShot_AK47";
            case "M4A1":
                return "GunShot_M4A1";
            case "Shotgun":
                return "GunShot_Shotgun";
            default:
                return "GunShot_Default";
        }
    }
    
    private string GetFootStepSoundEvent(FootStepMaterialType materialType)
    {
        switch (materialType)
        {
            case FootStepMaterialType.Concrete:
                return "FootStep_Concrete";
            case FootStepMaterialType.Metal:
                return "FootStep_Metal";
            case FootStepMaterialType.Wood:
                return "FootStep_Wood";
            case FootStepMaterialType.Grass:
                return "FootStep_Grass";
            default:
                return "FootStep_Default";
        }
    }
    
    private string GetAmbientSoundEvent(string ambientType)
    {
        switch (ambientType)
        {
            case "Wind":
                return "Ambient_Wind";
            case "Rain":
                return "Ambient_Rain";
            case "Thunder":
                return "Ambient_Thunder";
            default:
                return "Ambient_Default";
        }
    }
}
```

### 音频事件系统
```csharp
public class AudioEventSystem
{
    private AudioManager audioManager;
    private Dictionary<string, float> soundCooldowns;
    
    public AudioEventSystem()
    {
        audioManager = AudioManager.Instance;
        soundCooldowns = new Dictionary<string, float>();
    }
    
    public void OnWeaponFired(Item weapon, Vector3 position)
    {
        string soundEvent = GetWeaponSoundEvent(weapon);
        PlaySoundWithCooldown(soundEvent, position, 0.1f);
    }
    
    public void OnCharacterMoved(Vector3 position, FootStepMaterialType materialType)
    {
        string soundEvent = GetFootStepSoundEvent(materialType);
        PlaySoundWithCooldown(soundEvent, position, 0.5f);
    }
    
    public void OnExplosion(Vector3 position, float intensity)
    {
        string soundEvent = intensity > 0.5f ? "Explosion_Large" : "Explosion_Small";
        PlaySoundWithCooldown(soundEvent, position, 1.0f);
    }
    
    private void PlaySoundWithCooldown(string soundEvent, Vector3 position, float cooldown)
    {
        if (CanPlaySound(soundEvent, cooldown))
        {
            audioManager.PlaySound(soundEvent, position);
            soundCooldowns[soundEvent] = Time.time + cooldown;
        }
    }
    
    private bool CanPlaySound(string soundEvent, float cooldown)
    {
        if (soundCooldowns.ContainsKey(soundEvent))
        {
            return Time.time >= soundCooldowns[soundEvent];
        }
        return true;
    }
    
    private string GetWeaponSoundEvent(Item weapon)
    {
        // 根据武器类型获取声音事件
        return "GunShot_Default";
    }
    
    private string GetFootStepSoundEvent(FootStepMaterialType materialType)
    {
        // 根据材质类型获取声音事件
        return "FootStep_Default";
    }
}
```

### 音频设置系统
```csharp
public class AudioSettingsSystem
{
    private AudioManager audioManager;
    private AudioSettings audioSettings;
    
    public AudioSettingsSystem()
    {
        audioManager = AudioManager.Instance;
        audioSettings = new AudioSettings();
    }
    
    public void ApplyAudioSettings()
    {
        // 应用主音量
        SetMasterVolume(audioSettings.MasterVolume);
        
        // 应用音效音量
        SetSFXVolume(audioSettings.SFXVolume);
        
        // 应用音乐音量
        SetMusicVolume(audioSettings.MusicVolume);
        
        // 应用语音音量
        SetVoiceVolume(audioSettings.VoiceVolume);
        
        Debug.Log("音频设置已应用");
    }
    
    public void SetMasterVolume(float volume)
    {
        audioSettings.MasterVolume = Mathf.Clamp01(volume);
        // 应用主音量设置
        Debug.Log($"设置主音量: {audioSettings.MasterVolume}");
    }
    
    public void SetSFXVolume(float volume)
    {
        audioSettings.SFXVolume = Mathf.Clamp01(volume);
        // 应用音效音量设置
        Debug.Log($"设置音效音量: {audioSettings.SFXVolume}");
    }
    
    public void SetMusicVolume(float volume)
    {
        audioSettings.MusicVolume = Mathf.Clamp01(volume);
        // 应用音乐音量设置
        Debug.Log($"设置音乐音量: {audioSettings.MusicVolume}");
    }
    
    public void SetVoiceVolume(float volume)
    {
        audioSettings.VoiceVolume = Mathf.Clamp01(volume);
        // 应用语音音量设置
        Debug.Log($"设置语音音量: {audioSettings.VoiceVolume}");
    }
    
    public void SaveAudioSettings()
    {
        SavesSystem.Save("AudioSettings", audioSettings);
        Debug.Log("音频设置已保存");
    }
    
    public void LoadAudioSettings()
    {
        audioSettings = SavesSystem.Load("AudioSettings", new AudioSettings());
        ApplyAudioSettings();
        Debug.Log("音频设置已加载");
    }
}

[System.Serializable]
public class AudioSettings
{
    public float MasterVolume = 1.0f;
    public float SFXVolume = 1.0f;
    public float MusicVolume = 1.0f;
    public float VoiceVolume = 1.0f;
}
```

### 音频池系统
```csharp
public class AudioPoolSystem
{
    private AudioManager audioManager;
    private Dictionary<string, Queue<GameObject>> audioPools;
    private Dictionary<string, GameObject> audioPrefabs;
    
    public AudioPoolSystem()
    {
        audioManager = AudioManager.Instance;
        audioPools = new Dictionary<string, Queue<GameObject>>();
        audioPrefabs = new Dictionary<string, GameObject>();
    }
    
    public void PlayPooledSound(string soundEvent, Vector3 position)
    {
        GameObject audioObject = GetPooledAudioObject(soundEvent);
        if (audioObject != null)
        {
            audioObject.transform.position = position;
            audioObject.SetActive(true);
            
            // 播放声音
            audioManager.PlaySound(soundEvent, position);
            
            // 设置自动回收
            StartCoroutine(ReturnToPool(audioObject, soundEvent, 2.0f));
        }
    }
    
    private GameObject GetPooledAudioObject(string soundEvent)
    {
        if (!audioPools.ContainsKey(soundEvent))
        {
            audioPools[soundEvent] = new Queue<GameObject>();
        }
        
        Queue<GameObject> pool = audioPools[soundEvent];
        
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            return CreateNewAudioObject(soundEvent);
        }
    }
    
    private GameObject CreateNewAudioObject(string soundEvent)
    {
        if (audioPrefabs.ContainsKey(soundEvent))
        {
            GameObject prefab = audioPrefabs[soundEvent];
            GameObject audioObject = Instantiate(prefab);
            audioObject.SetActive(false);
            return audioObject;
        }
        
        return null;
    }
    
    private IEnumerator ReturnToPool(GameObject audioObject, string soundEvent, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        audioObject.SetActive(false);
        audioPools[soundEvent].Enqueue(audioObject);
    }
}
```

### 音频可视化系统
```csharp
public class AudioVisualizationSystem
{
    private AudioManager audioManager;
    private List<AudioVisualizer> visualizers;
    
    public AudioVisualizationSystem()
    {
        audioManager = AudioManager.Instance;
        visualizers = new List<AudioVisualizer>();
    }
    
    public void AddVisualizer(AudioVisualizer visualizer)
    {
        visualizers.Add(visualizer);
    }
    
    public void RemoveVisualizer(AudioVisualizer visualizer)
    {
        visualizers.Remove(visualizer);
    }
    
    public void UpdateVisualization()
    {
        foreach (AudioVisualizer visualizer in visualizers)
        {
            if (visualizer.IsActive)
            {
                visualizer.UpdateVisualization();
            }
        }
    }
}

public class AudioVisualizer : MonoBehaviour
{
    public bool IsActive = true;
    public string SoundEvent;
    public float Sensitivity = 1.0f;
    
    private AudioManager audioManager;
    
    void Start()
    {
        audioManager = AudioManager.Instance;
    }
    
    public void UpdateVisualization()
    {
        // 更新音频可视化
        float audioLevel = GetAudioLevel();
        UpdateVisualEffects(audioLevel);
    }
    
    private float GetAudioLevel()
    {
        // 获取音频级别
        return 0.5f; // 示例
    }
    
    private void UpdateVisualEffects(float audioLevel)
    {
        // 更新视觉效果
        float intensity = audioLevel * Sensitivity;
        // 根据强度更新视觉效果
    }
}
```

## 注意事项

1. 音频系统使用FMOD，需要正确的事件名称
2. 3D音频需要正确的位置信息
3. 声音类型设置影响角色声音表现
4. 脚步声材质类型影响脚步声效果
5. 音频设置需要保存到存档中
6. 音频池系统提高性能，避免频繁创建销毁
7. 音频事件系统需要正确管理声音播放
8. 音频可视化系统提供视觉反馈
9. 音频系统与角色系统、战斗系统紧密相关
10. 音频设置应该支持实时调整
