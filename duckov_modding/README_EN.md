# Duckov Modding Example

_This is an example project for modding Escape From Duckov._

[中文](README.md) | English

[Notable APIs](Documents/NotableAPIs.md)

## Overview

The modding system of Escape From Duckov scans and reads the subfolders in the Duckov_Data/Mods folder, as well as the folders of subscribed items in the Steam Workshop. Mods are displayed and loaded in the game through the `dll` files, `info.ini`, and `preview.png` contained in these folders.

Escape From Duckov reads the name parameter in info.ini and uses it as a namespace to load a class named ModBehaviour. For example, if info.ini contains `name=MyMod`, it will load `MyMod.ModBehaviour` from the `MyMod.dll` file.

ModBehaviour should inherit from `Duckov.Modding.ModBehaviour`. `Duckov.Modding.ModBehaviour` is a class that inherits from MonoBehaviour and includes some additional features needed in the mod system. When loading a mod, Escape From Duckov creates a GameObject for that mod and creates an instance of ModBehaviour by calling GameObject.AddComponent(Type). Mod authors can implement functionality by writing Unity events such as Start\Update in ModBehaviour, or by registering other events in Escape From Duckov.

## Mod File Structure

Place the prepared Mod folder in `Duckov_Data/Mods` within the Escape From Duckov installation directory, and the Mod can be loaded in the Mods interface on the game's main menu.
Assuming the Mod's name is "MyMod", the published file structure should be as follows:

- MyMod (Folder)
  - MyMod.dll
  - info.ini
  - preview.png (Square preview image, recommended resolution `256*256`)

[Mod Folder Example](DisplayItemValue/ReleaseExample/DisplayItemValue/)

### info.ini

info.ini should contain the following parameters:

- name (mod name, primarily used for loading the dll file)
- displayName (display name)
- description (display description)

info.ini may also contain the following parameters:

- publishedFileId (records this Mod's ID in the Steam Workshop)

**Note: When uploading to Steam Workshop, info.ini will be overwritten. Original information in info.ini may be lost as a result. Therefore, it is not recommended to store any information other than the above items in info.ini.**

## Configuring C# Project

1. Have Escape From Duckov installed on your computer.
2. Create a .NET Class Library project.
3. Configure project parameters.
   1. Target Framework
      - **It is recommended to set TargetFramework to netstandard2.1.**
      - Note: Remove features not supported by TargetFramework, such as `<ImplicitUsings>`
   2. Reference Include
      - Add `\Duckov_Data\Managed\*.dll` from Escape From Duckov to the references.
      - Example:

      ```
      <ItemGroup>
        <Reference Include="$(DuckovPath)\Duckov_Data\Managed\TeamSoda.*" />
        <Reference Include="$(DuckovPath)\Duckov_Data\Managed\ItemStatsSystem.dll" />
        <Reference Include="$(DuckovPath)\Duckov_Data\Managed\Unity*" />
      </ItemGroup> 
      ```

4. Done! Now write a ModBehaviour class in your Mod's Namespace. Build the project to get your mod's main dll.

csproj File Example: [DisplayItemValue.csproj](DisplayItemValue/DisplayItemValue.csproj)

## Other

### Unity Package


When developing with Unity, you can refer to the [manifest.json file](UnityFiles/manifest.json) included in this repository to select packages.

### Custom Game Items

- Call `ItemStatsSystem.ItemAssetsCollection.AddDynamicEntry(Item prefab)` to add custom items
- Call `ItemStatsSystem.ItemAssetsCollection.RemoveDynamicEntry(Item prefab)` to remove the mod item
- Custom item prefabs need to have TypeID configured properly. Avoid conflicts with the base game and other MODs.
- If the corresponding MOD is not loaded when entering the game, custom items in the save file will disappear directly.

### Localization

- Call `SodaCraft.Localizations.LocalizationManager.SetOverrideText(string key, string value)` to override displayed localization text.
- Use the `SodaCraft.Localizations.LocalizationManager.OnSetLanguage:System.Action<SystemLanguage>` event to handle logic when switching languages

## Duckov Community Rules

To aid the long-term development of the Duckov community, we ask everyone to contribute to a positive creative environment. Please adhere to the following rules:
1.Content that violates the laws of the regions where the developer team and the Steam platform operate is strictly prohibited, including but not limited to content involving politics, child sexual exploitation, or the promotion of violence or terrorism.
2.Content that severely insults characters, distorts the story, or aims to cause discomfort, conflict, or controversy in the player community is prohibited. This also includes content related to current events or real-life individuals that may trigger real-world disputes.
3.Unauthorized use of copyrighted game assets or other third-party materials is prohibited.
4.Mods must not be used to direct players to advertisements, fundraising, payment requests, or other commercial or unofficial external links.
5.Mods containing AI-generated content must be clearly labeled.
For mods published on Steam Workshop, any violations of the above rules may result in removal without prior notice and may lead to suspension of the creator’s permissions.


