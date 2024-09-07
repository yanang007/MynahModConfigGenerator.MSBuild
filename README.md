TaiwuConfigGenerator
--------------------

该仓库将[MynahModConfigGenerator](https://github.com/12548/MynahTaiwuMods/tree/develop/MynahModConfigGenerator)打包为NuGet包，实现快速引入到太吾绘卷插件项目中。

在项目中，通过添加生成任务`GenerateTaiwuConfigTask`，可以实现基于代码的太吾绘卷mod配置信息生成，根据annotation自动向Config.lua注入配置信息，具体配置信息参考[MynahBaseModBase.cs](https://github.com/12548/MynahTaiwuMods/blob/develop/MynahBaseModBase/MynahBaseModBase.cs)。

## 使用方式

1. 在项目中引入依赖。
```
  <ItemGroup>
    <PackageReference Include="TaiwuConfigGenerator" Version="1.0.0">
      <PrivateAssets>all</PrivateAssets>
   </PackageReference>
  </ItemGroup>
```

2. 配置项目发布路径，将Config.lua和生成的plugin dll拷贝到发布路径，例如（其中GameDir需替换为你的游戏安装路径）：
```
  <PropertyGroup>
    <OutputDir>.\Mod\</OutputDir>
  </PropertyGroup>

  <Target Name="CopyPlugin" AfterTargets="Build">
	  <Copy SourceFiles="$(TargetDir)$(ProjectName).dll" DestinationFolder="$(OutputDir)Plugins" />
	  <Copy SourceFiles="$(TargetDir)$(ProjectName).pdb" DestinationFolder="$(OutputDir)Plugins" />
    <Copy SourceFiles="Config.lua" DestinationFolder="$(OutputDir)" />
  </Target>
```

3. 通过`GenerateTaiwuConfigTask`任务注入配置描述信息，例如：
```
  <Target Name="GenerateConfig" AfterTargets="Build">
    <GenerateTaiwuConfigTask PluginDir="$(OutputDir)" />
  </Target>
```

## 参数定义

`GenerateTaiwuConfigTask`包含如下参数：

|参数|描述|
|----|----|
|PluginDir|插件发布目录，假设路径下包含Config.lua文件和Plugins文件夹，自动读取Plugins中的dll生成配置|
|ConfigLuaPath|`未指定PluginDir时` 指定待注入配置信息的基础Config.lua路径|
|TargetDll|`未指定PluginDir时` 指定插件dll所在路径|

## 编译方式

1. 参考MynahTaiwuMods项目开发流程，设置环境变量`TAIWU_PATH`为你的游戏安装路径。

以cmd和powershell为例，其中路径为游戏路径示例，需要替换为你的游戏安装路径。
```cmd
set TAIWU_PATH=E:\SteamLibrary\steamapps\common\The Scroll Of Taiwu\
dotnet build --configuration Release
```

```powershell
$env:TAIWU_PATH=E:\SteamLibrary\steamapps\common\The Scroll Of Taiwu\
dotnet build --configuration Release
```

2. 输出插件应位于[./TaiwuConfigGenerator/bin/Release](./TaiwuConfigGenerator/bin/Release)下。

## 致谢

[MynahModConfigGenerator](https://github.com/12548/MynahTaiwuMods/tree/develop/MynahModConfigGenerator)：实现了根据代码注解自动生成配置文件。

[BepInEx.AssemblyPublicizer.MSBuild](https://github.com/BepInEx/BepInEx.AssemblyPublicizer/tree/master/BepInEx.AssemblyPublicizer.MSBuild)：将AssemblyPublicizer打包为NuGet包实现快速引入。
