using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConfigGenerator
{
    public class GenerateTaiwuConfigTask : Task
    {
        public override bool Execute()
        {
            if (PluginDir == null)
            {
            }
            else
            {
                ConfigLuaPath = PluginDir.Of("Config.lua");
                TargetDllDir = PluginDir.Of("Plugins");
            }

            if (TargetDlls == null)
            {
                if (TargetDllDir != null)
                {
                    TargetDlls = Directory.GetFiles(TargetDllDir, "*.dll");
                }
                else if (TargetDll != null)
                {
                    TargetDlls = new string[] { TargetDll };
                }
            }

            string generatorPath = Assembly.GetAssembly(typeof(GenerateTaiwuConfigTask)).Location.Parent().Parent().Parent() + "\\tools\\MynahModConfigGenerator.exe";
            if (generatorPath == null)
            {
                return false;
            }

            IEnumerable<string> args = new string[] { ConfigLuaPath }.Concat(TargetDlls);

            Process p = Process.Start(new ProcessStartInfo
            {
                FileName=generatorPath,
                Arguments=args.Select(WrapParam).Join(" "),
                UseShellExecute=false,
            });
            p.WaitForExit();

            return true;
        }

        /// <summary>
        /// 插件输入路径，假设遵循如下文件结构
        /// 
        /// [PluginDir]
        /// │  Config.lua
        /// │
        /// └─Plugins
        ///         YourMod.dll
        ///         
        /// </summary>
        public string PluginDir { get; set; }

        /// <summary>
        /// 游戏Mod Config.lua 文件路径
        /// </summary>
        public string ConfigLuaPath { get; set; }

        /// <summary>
        /// Mod插件dll文件路径
        /// </summary>
        public string TargetDll { get; set; }

        /// <summary>
        /// Mod插件dll文件夹路径，会自动搜索路径下的全部dll提取配置
        /// </summary>
        public string TargetDllDir { get; set; }

        public string[] TargetDlls { get; set; }

        public static string WrapParam(string p)
        {
            return $"\"{p}\"";
        }
    }

    public static class StringJoinExtension
    {
        public static string Join(this IEnumerable<string> strs, string sep)
        {
            return string.Join(sep, strs);
        }
    }

    public static class PathExtension
    {
        public static string Parent(this string path)
        {
            return Directory.GetParent(path).FullName;
        }

        public static string Of(this string path, string sub)
        {
            return Path.Combine(path, sub);
        }
    }
}