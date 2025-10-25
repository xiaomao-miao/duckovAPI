# TeamSoda.MiniLocalizor 模块

> 对应程序集：`TeamSoda.MiniLocalizor.dll`

MiniLocalizor 提供 CSV 驱动的文本本地化能力。本节罗列反编译程序集内的所有公共类型、属性与重要方法，方便在编写 Mod 文档时快速对照。

## 核心类型

### `CSVFileLocalizor`
- 实现 `ILocalizationProvider`，负责加载、查询 CSV 文本。
- **构造函数**：
  - `CSVFileLocalizor(string path)` – 直接指定 CSV 路径，自动解析文件名判断 `SystemLanguage`。
  - `CSVFileLocalizor(SystemLanguage language)` – 根据语言拼接 `StreamingAssets/Localization/<Lang>.csv`，若不存在会创建空文件。
- **属性**：
  - `Path` – 当前 CSV 文件路径。
  - `Language` – 解析得到的语言枚举，解析失败时为 `SystemLanguage.Unknown`。
- **方法**：
  - `BuildDictionary()` – 清空并重新读取 CSV，依赖 `MiniExcelLibs` 将每行转换为 `DataEntry`，并将键写入内部 `Dictionary<string, DataEntry>`。读取异常时会输出日志并提示关闭外部编辑器。
  - `Get(string key)` – 返回指定键的本地化字符串，自动对 `\n` 等转义字符执行 `Regex.Unescape`。
  - `GetEntry(string key)` – 返回 `DataEntry`，包含版本号、工作表名等元数据。
  - `HasKey(string key)` – 判断是否存在对应文本。
  - （内部）`ConvertFromEscapes(string origin)` / `ConvertToEscapes(string origin)` – 提供正则转义工具，`Get` 会调用前者以获取原始换行等字符。

### `ILocalizationProvider`
- 简单接口，仅定义 `string Get(string key)`。
- 任何自定义本地化方案只需实现该接口，即可与 `CSVFileLocalizor` 互换。

### `MiniLocalizor.DataEntry`
- 标准化的 CSV 行数据。
- **自动属性**：`key`、`value`、`version`、`sheet`。
- **方法**：`IsNewerThan(string version)` – 比较两条版本号，支持以 `#` 前缀标记开发版本。

### 自动生成的辅助类
- `UnitySourceGeneratedAssemblyMonoScriptTypes_v1`：Unity 生成的脚本注册表，仅用于编辑器反射。
- `Properties.AssemblyInfo`：程序集属性声明（版本、GUID 等），对运行时代码无直接影响。

## 使用建议

- 当 CSV 被外部工具占用时，`BuildDictionary()` 会捕获异常并打印堆栈；确保在编辑完成后关闭 Excel 等软件再重新构建字典。
- 若需要写入 CSV，请手动调用 `ConvertToEscapes`（通过 `CSVFileLocalizor` 的私有方法逻辑参考），以保证换行符等特殊字符在文件中正确转义。
- 可以将 `CSVFileLocalizor` 与游戏现有的 `LocalizationManager`、`ILocalizationProvider` 实现互换，实现 Mod 专属语言包。

## 示例

```csharp
// 加载中文文本
var provider = new CSVFileLocalizor(SystemLanguage.Chinese);
string title = provider.Get("UI_Title");

// 动态监听文件更新
FileSystemWatcher watcher = new(Path.GetDirectoryName(provider.Path));
watcher.Changed += (_, __) => provider.BuildDictionary();
watcher.EnableRaisingEvents = true;
```

借助以上归档，MiniLocalizor 模块的所有公共 API 均已覆盖，可直接用于撰写面向 Mod 的本地化文档。
