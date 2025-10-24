# UI系统

## 概述
UI系统是《逃离鸭科夫》的重要系统之一，负责管理游戏界面、交互、对话、自定义面部等功能。

## 核心组件

### View
视图基类，管理游戏中的各种视图。

#### 主要功能
- **视图管理** - 显示、隐藏视图
- **状态管理** - 管理视图状态
- **事件通知** - 活动视图改变事件

#### 重要属性
- `ActiveView` - 当前活动视图

#### 重要事件
- `OnActiveViewChanged` - 活动视图改变事件

#### 重要方法
- `Show()` - 显示视图
- `Hide()` - 隐藏视图
- `Toggle()` - 切换视图显示状态

### DialogueUI
对话UI系统，管理游戏中的对话显示。

#### 主要功能
- **对话管理** - 显示、隐藏对话
- **角色设置** - 设置对话角色
- **状态管理** - 管理对话状态

#### 重要属性
- `Active` - 是否有活动对话

#### 重要事件
- `OnDialogueStatusChanged` - 对话状态改变事件

#### 重要方法
- `ShowDialogue(DialogueData dialogueData)` - 显示对话
- `HideDialogue()` - 隐藏对话
- `SetDialogueActor(string actorName)` - 设置对话角色

### CustomFaceUI
自定义面部UI系统，管理角色面部自定义。

#### 主要功能
- **面部自定义** - 自定义角色面部
- **UI管理** - 管理自定义UI
- **状态管理** - 管理UI状态

#### 重要属性
- `ActiveView` - 当前活动视图

#### 重要事件
- `OnCustomUIViewChanged` - 自定义UI视图改变事件

#### 重要方法
- `ShowCustomFaceUI()` - 显示自定义面部UI
- `HideCustomFaceUI()` - 隐藏自定义面部UI

### CameraMode
相机模式系统，管理游戏相机的不同模式。

#### 主要功能
- **相机模式** - 进入、退出相机模式
- **状态管理** - 管理相机模式状态
- **事件通知** - 相机模式改变事件

#### 重要属性
- `Active` - 是否处于相机模式

#### 重要事件
- `OnCameraModeChanged` - 相机模式改变事件

#### 重要方法
- `EnterCameraMode()` - 进入相机模式
- `ExitCameraMode()` - 退出相机模式
- `ToggleCameraMode()` - 切换相机模式

### DialogueBubblesManager
对话气泡管理器，管理游戏中的对话气泡显示。

#### 主要功能
- **气泡显示** - 显示对话气泡
- **气泡管理** - 管理气泡生命周期
- **清理管理** - 清除所有气泡

#### 重要方法
- `Show(string text, Transform target, float duration, bool followTarget, bool showBackground, float height, float size)` - 显示对话气泡
- `Hide(Transform target)` - 隐藏对话气泡
- `ClearAll()` - 清除所有对话气泡

### PopText
弹出文本系统，管理游戏中的文本弹出效果。

#### 主要功能
- **文本弹出** - 弹出文本效果
- **样式管理** - 管理文本样式
- **生命周期** - 管理文本生命周期

#### 重要属性
- `instance` - 获取PopText实例

#### 重要方法
- `Pop(string text, Vector3 position, Color color, float size, Sprite icon)` - 弹出文本
- `Pop(string text, Vector3 position)` - 弹出文本（默认样式）

## 使用示例

### 视图管理
```csharp
// 显示视图
View.Show();
Debug.Log("显示视图");

// 隐藏视图
View.Hide();
Debug.Log("隐藏视图");

// 切换视图
View.Toggle();
Debug.Log("切换视图");

// 检查活动视图
if (View.ActiveView != null)
{
    Debug.Log($"活动视图: {View.ActiveView.name}");
}
```

### 监听视图事件
```csharp
void OnEnable()
{
    View.OnActiveViewChanged += OnActiveViewChanged;
}

void OnDisable()
{
    View.OnActiveViewChanged -= OnActiveViewChanged;
}

private void OnActiveViewChanged(View newView)
{
    Debug.Log($"活动视图改变: {newView.name}");
}
```

### 对话系统
```csharp
// 显示对话
DialogueData dialogueData = GetDialogueData();
DialogueUI.ShowDialogue(dialogueData);
Debug.Log("显示对话");

// 隐藏对话
DialogueUI.HideDialogue();
Debug.Log("隐藏对话");

// 设置对话角色
DialogueUI.SetDialogueActor("NPC1");
Debug.Log("设置对话角色为NPC1");

// 检查对话状态
if (DialogueUI.Active)
{
    Debug.Log("有活动对话");
}
```

### 监听对话事件
```csharp
void OnEnable()
{
    DialogueUI.OnDialogueStatusChanged += OnDialogueStatusChanged;
}

void OnDisable()
{
    DialogueUI.OnDialogueStatusChanged -= OnDialogueStatusChanged;
}

private void OnDialogueStatusChanged()
{
    Debug.Log("对话状态改变");
}
```

### 自定义面部UI
```csharp
// 显示自定义面部UI
CustomFaceUI.ShowCustomFaceUI();
Debug.Log("显示自定义面部UI");

// 隐藏自定义面部UI
CustomFaceUI.HideCustomFaceUI();
Debug.Log("隐藏自定义面部UI");

// 检查活动视图
if (CustomFaceUI.ActiveView != null)
{
    Debug.Log($"活动自定义UI视图: {CustomFaceUI.ActiveView.name}");
}
```

### 相机模式
```csharp
// 进入相机模式
CameraMode.EnterCameraMode();
Debug.Log("进入相机模式");

// 退出相机模式
CameraMode.ExitCameraMode();
Debug.Log("退出相机模式");

// 切换相机模式
CameraMode.ToggleCameraMode();
Debug.Log("切换相机模式");

// 检查相机模式状态
if (CameraMode.Active)
{
    Debug.Log("处于相机模式");
}
```

### 监听相机模式事件
```csharp
void OnEnable()
{
    CameraMode.OnCameraModeChanged += OnCameraModeChanged;
}

void OnDisable()
{
    CameraMode.OnCameraModeChanged -= OnCameraModeChanged;
}

private void OnCameraModeChanged()
{
    Debug.Log("相机模式改变");
}
```

### 对话气泡
```csharp
// 显示对话气泡
Transform target = GetTarget();
DialogueBubblesManager.Show("Hello World!", target, 3f, true, true, 2f, 1.5f);
Debug.Log("显示对话气泡");

// 隐藏对话气泡
DialogueBubblesManager.Hide(target);
Debug.Log("隐藏对话气泡");

// 清除所有对话气泡
DialogueBubblesManager.ClearAll();
Debug.Log("清除所有对话气泡");
```

### 弹出文本
```csharp
// 弹出文本
Vector3 position = GetPosition();
PopText.Pop("伤害: 100", position, Color.red, 1.5f);
Debug.Log("弹出伤害文本");

// 使用默认样式
PopText.Pop("经验值 +50", position);
Debug.Log("弹出经验值文本");
```

### UI系统集成
```csharp
public class UISystem
{
    private View currentView;
    private DialogueUI dialogueUI;
    private CustomFaceUI customFaceUI;
    private CameraMode cameraMode;
    
    public UISystem()
    {
        dialogueUI = GetComponent<DialogueUI>();
        customFaceUI = GetComponent<CustomFaceUI>();
        cameraMode = GetComponent<CameraMode>();
    }
    
    public void ShowMainMenu()
    {
        // 显示主菜单
        currentView = GetMainMenuView();
        currentView.Show();
        Debug.Log("显示主菜单");
    }
    
    public void ShowGameUI()
    {
        // 显示游戏UI
        currentView = GetGameUIView();
        currentView.Show();
        Debug.Log("显示游戏UI");
    }
    
    public void ShowDialogue(string dialogueText, string actorName)
    {
        // 显示对话
        DialogueData dialogueData = CreateDialogueData(dialogueText, actorName);
        dialogueUI.ShowDialogue(dialogueData);
        Debug.Log($"显示对话: {dialogueText}");
    }
    
    public void ShowCustomFaceUI()
    {
        // 显示自定义面部UI
        customFaceUI.ShowCustomFaceUI();
        Debug.Log("显示自定义面部UI");
    }
    
    public void EnterCameraMode()
    {
        // 进入相机模式
        cameraMode.EnterCameraMode();
        Debug.Log("进入相机模式");
    }
    
    public void ExitCameraMode()
    {
        // 退出相机模式
        cameraMode.ExitCameraMode();
        Debug.Log("退出相机模式");
    }
    
    private View GetMainMenuView()
    {
        // 获取主菜单视图
        return null; // 示例
    }
    
    private View GetGameUIView()
    {
        // 获取游戏UI视图
        return null; // 示例
    }
    
    private DialogueData CreateDialogueData(string text, string actor)
    {
        // 创建对话数据
        return new DialogueData(); // 示例
    }
}
```

### UI事件系统
```csharp
public class UIEventSystem
{
    private Dictionary<string, List<System.Action>> eventHandlers;
    
    public UIEventSystem()
    {
        eventHandlers = new Dictionary<string, List<System.Action>>();
    }
    
    public void RegisterEventHandler(string eventName, System.Action handler)
    {
        if (!eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName] = new List<System.Action>();
        }
        
        eventHandlers[eventName].Add(handler);
        Debug.Log($"注册事件处理器: {eventName}");
    }
    
    public void UnregisterEventHandler(string eventName, System.Action handler)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName].Remove(handler);
            Debug.Log($"注销事件处理器: {eventName}");
        }
    }
    
    public void TriggerEvent(string eventName)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            foreach (System.Action handler in eventHandlers[eventName])
            {
                handler.Invoke();
            }
            Debug.Log($"触发事件: {eventName}");
        }
    }
}
```

### UI动画系统
```csharp
public class UIAnimationSystem
{
    private Dictionary<GameObject, Coroutine> animations;
    
    public UIAnimationSystem()
    {
        animations = new Dictionary<GameObject, Coroutine>();
    }
    
    public void AnimateUI(GameObject uiObject, UIAnimationType animationType, float duration)
    {
        if (animations.ContainsKey(uiObject))
        {
            StopCoroutine(animations[uiObject]);
        }
        
        Coroutine animation = StartCoroutine(PlayAnimation(uiObject, animationType, duration));
        animations[uiObject] = animation;
    }
    
    private IEnumerator PlayAnimation(GameObject uiObject, UIAnimationType animationType, float duration)
    {
        switch (animationType)
        {
            case UIAnimationType.FadeIn:
                yield return FadeInAnimation(uiObject, duration);
                break;
            case UIAnimationType.FadeOut:
                yield return FadeOutAnimation(uiObject, duration);
                break;
            case UIAnimationType.SlideIn:
                yield return SlideInAnimation(uiObject, duration);
                break;
            case UIAnimationType.SlideOut:
                yield return SlideOutAnimation(uiObject, duration);
                break;
        }
        
        animations.Remove(uiObject);
    }
    
    private IEnumerator FadeInAnimation(GameObject uiObject, float duration)
    {
        // 淡入动画
        yield return null; // 示例
    }
    
    private IEnumerator FadeOutAnimation(GameObject uiObject, float duration)
    {
        // 淡出动画
        yield return null; // 示例
    }
    
    private IEnumerator SlideInAnimation(GameObject uiObject, float duration)
    {
        // 滑入动画
        yield return null; // 示例
    }
    
    private IEnumerator SlideOutAnimation(GameObject uiObject, float duration)
    {
        // 滑出动画
        yield return null; // 示例
    }
}

public enum UIAnimationType
{
    FadeIn,
    FadeOut,
    SlideIn,
    SlideOut
}
```

## 注意事项

1. UI系统支持多种视图类型
2. 对话系统支持多语言
3. 相机模式会禁用其他输入
4. 弹出文本会自动销毁，无需手动管理
5. 所有系统都支持事件监听，便于Mod开发
6. UI事件系统需要正确管理事件处理器
7. UI动画系统提供丰富的视觉效果
8. UI系统与角色系统、音频系统紧密相关
9. 事件监听后记得在OnDisable中取消订阅
10. UI系统需要考虑性能优化和内存管理
