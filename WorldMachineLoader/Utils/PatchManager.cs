using HarmonyLib;
using System;
using System.Reflection;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Utils;
using WorldMachineLoader.Modding;

namespace WorldMachineLoader.Utils
{
    public class PatchManager
    {
        Logger Logger = new Logger("WML/PatchManager");

        public void ApplyAllPatches(Assembly assembly, string modId)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Static
                                                     | BindingFlags.Public
                                                     | BindingFlags.NonPublic))
                {
                    foreach (var attr in method.GetCustomAttributes<GamePatch>())
                    {
                        try
                        {
                            Logger.Log($"Applying patches from mod with ID \"/{modId}\"...", Logger.LogLevel.Info, Logger.VerbosityLevel.Detailed);
                            var original = AccessTools.Method(attr.TargetType, attr.MethodName, attr.ArgumentTypes);

                            Logger.Log($"Patching {original.Name} with {method.Name} as {attr.PatchType} from {assembly.FullName}", Logger.LogLevel.Info, Logger.VerbosityLevel.Detailed);

                            var harmony = new Harmony(modId);
                            var patch = new HarmonyMethod(method);

                            switch (attr.PatchType)
                            {
                                case PatchType.Prefix:
                                    harmony.Patch(original, prefix: patch);
                                    break;
                                case PatchType.Postfix:
                                    harmony.Patch(original, postfix: patch);
                                    break;
                                case PatchType.Transpiler:
                                    harmony.Patch(original, transpiler: patch);
                                    break;
                                case PatchType.Finalizer:
                                    harmony.Patch(original, finalizer: patch);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"Failed to patch {attr.TargetType}.{attr.MethodName} with {method.Name} from {assembly.FullName}!", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                            Logger.Log($"Stacktrace: {ex}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                        }
                    }
                }
            }
        }
    }
}
