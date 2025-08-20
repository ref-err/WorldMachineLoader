﻿using System;
using System.IO;
using HarmonyLib;
using Microsoft.Xna.Framework;
using OneShotMG;
using OneShotMG.src;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.TWM.Filesystem;
using WorldMachineLoader.Modding;
using WorldMachineLoader.ModLoader;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.Patches
{
    [HarmonyPatch(typeof(Game1))]
    class Game1Patch
    {
        static Logger Logger = new Logger("WML/GamePatch");

        // patching initialize to change versionstring and window title.
        [HarmonyPrefix]
        [HarmonyPatch("Initialize")]
        static void Initialize_PrefixPatch(Game1 __instance)
        {
            Game1.logMan.Log(LogManager.LogLevel.Info, "Hello from World Machine Loader!");
            Game1.VersionString = $"{Game1.VersionString} + WorldMachineLoader";

            __instance.Window.Title = $"OneShot [{Game1.VersionString}]";
        }

        [HarmonyPrefix]
        [HarmonyPatch("ShutDown")]
        static void ShutDown_Prefix(Game1 __instance)
        {
            foreach (var mod in ModLoader.ModLoader.mods)
            {
                try { mod.Instance.OnShutdown(); }
                catch (Exception ex)
                {
                    Logger.Log($"Exception while calling {mod.ID} OnShutdown: {ex}", Logger.LogLevel.Error);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void UpdatePatch(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime;
            foreach (var mod in ModLoader.ModLoader.mods)
            {
                mod.ModContext.Scheduler.Update(delta);
            }
        }
    }

    // this patch is made for skipping intro (logos that show up when game's starting).
    // it checks for "skip_intro" boolean value in mods/settings.json. if it's true,
    // then we apply patch.
    [HarmonyPatch(typeof(BootManager))]
    class BootManagerPatch
    {
        static Logger Logger = new Logger("WML/BootManagerPatch");

        static bool Prepare()
        {
            return ModSettings.Instance.SkipIntro;
        }

        [HarmonyPostfix]
        [HarmonyPatch(MethodType.Constructor, new Type[] { typeof(Game1) })]
        static void Postfix(BootManager __instance)
        {
            try
            {
                __instance.RestartBootSequence();
            }
            catch (Exception ex)
            {
                 Logger.Log(ex.ToString(), Logger.LogLevel.Error);
            }
        }
    }

    [HarmonyPatch(typeof(WindowManager))]
    class WindowManagerPatch
    {
        // patching RunFile to open Mod List window. idk why this window type exists,
        // because in oneshot's source this window type literally does nothing at all.
        [HarmonyPrefix]
        [HarmonyPatch("RunFile")]
        static bool InterceptRunFile(object node, WindowManager __instance)
        {
            if (node is TWMFile file && file.program == LaunchableWindowType.DUMMY_FILE_FOR_TUTORIALS)
            {
                __instance.AddWindow(new ModListWindow());
            }
            return true;
        }

        // patching LoadDesktop method to create an icon with LaunchableWindowType of DUMMY_FILE_FOR_TUTORIALS.
        [HarmonyPostfix]
        [HarmonyPatch("LoadDesktop")]
        static void LoadDesktop_Postfix(TWMDesktopManager desktop, WindowManager __instance)
        {
            TWMFile tWMFile = new TWMFile("oneshot", "Mod List", LaunchableWindowType.DUMMY_FILE_FOR_TUTORIALS);
            if (!__instance.FileSystem.FileExists("/Mod List"))
                __instance.FileSystem.WriteFile("/", tWMFile);
        }
    }

    // this patch is made so we can get Game instance, and out of Game i can get GraphicsDevice,
    // which is important for rendering images (in my case, i was rendering mod icons).
    [HarmonyPatch(typeof(GraphicsManager))]
    [HarmonyPatch(MethodType.Constructor, new Type[] { typeof(Game) })]
    class GraphicsManagerCtorPatch
    {
        static void Prefix(Game mGame)
        {
            Globals.monoGame = mGame;
        }
    }

    [HarmonyPatch(typeof(LogManager))]
    class LogManagerPatch
    {
        private static string _lastErrorMessage = null;

        [HarmonyPostfix]
        [HarmonyPatch("Log")]
        static void Postfix(LogManager __instance, LogManager.LogLevel level, string message)
        {
            if (level == LogManager.LogLevel.Error)
            {
                _lastErrorMessage = message;
            }
            else if (level == LogManager.LogLevel.StackDump && _lastErrorMessage != null)
            {
                try
                {
                    File.Create(Path.Combine(Constants.GamePath, "logs", "crashed"));
                    string crashFile = Path.Combine(Constants.GamePath, "logs", $"crash-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
                    string content = "Oops! Game crashed!\n" +
                                    $"Crashed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n" +
                                    $"Error: {_lastErrorMessage}\n" +
                                    $"StackTrace:\n{message}";
                    File.WriteAllText(crashFile, content);
                }
                catch { }
                finally
                {
                    _lastErrorMessage = null;
                }
            }
        }
    }
}
