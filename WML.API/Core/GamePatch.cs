using System;

namespace WorldMachineLoader.API.Core
{
    public enum PatchType
    {
        Prefix,
        Postfix,
        Transpiler,
        Finalizer
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GamePatch : Attribute
    {
        public Type TargetType { get; }
        public string MethodName { get; }
        public PatchType PatchType { get; }
    
        public GamePatch(Type targetType, string methodName, PatchType patchType)
        {
            TargetType = targetType;
            MethodName = methodName;
            PatchType = patchType;
        }
    }
}
