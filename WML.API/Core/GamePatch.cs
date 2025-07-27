using System;

namespace WorldMachineLoader.API.Core
{
    /// <summary>
    /// Specifies how and when a method patch will be applied.
    /// </summary>
    public enum PatchType
    {
        /// <summary>Run this code before the original method executes.</summary>
        Prefix,

        /// <summary>Run this code after the original method executes.</summary>
        Postfix,

        /// <summary>Rewrite the method's IL at runtime to change its behavior.</summary>
        Transpiler,

        /// <summary>Run this code if the original method throws an exception.</summary>
        Finalizer
    }
    
    /// <summary>
    /// Indicates that the decorated method should be used as a patch for a game method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GamePatch : Attribute
    {
        /// <summary>Gets the type containing the target method to patch.</summary>
        public Type TargetType { get; }

        /// <summary>Gets the name of the method on <see cref="TargetType"/> that will be patched.</summary>
        public string MethodName { get; }
        
        /// <summary>Gets the <see cref="PatchType"/> indicating how patch is applied.</summary>
        public PatchType PatchType { get; }

        /// <summary>
        /// Gets the list of parameter types of the original method in the exact declaration order.
        /// If no types are specified, this defaults to <see cref="Type.EmptyTypes"/>
        /// </summary>
        public Type[] ArgumentTypes { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePatch"/> attribute.
        /// </summary>
        /// <param name="targetType">The type containing the method to patch.</param>
        /// <param name="methodName">The name of the method on target type to patch.</param>
        /// <param name="patchType">The type of a patch to apply.</param>
        public GamePatch(Type targetType, string methodName, PatchType patchType)
        {
            TargetType = targetType;
            MethodName = methodName;
            PatchType = patchType;
            ArgumentTypes = Type.EmptyTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePatch"/> attribute.
        /// </summary>
        /// <param name="targetType">The type containing the method to patch.</param>
        /// <param name="methodName">The name of the method on target type to patch.</param>
        /// <param name="patchType">The type of a patch to apply.</param>
        /// <param name="argumentTypes">The list of parameter types of the original method.</param>
        public GamePatch(Type targetType, string methodName, PatchType patchType, params Type[] argumentTypes)
        {
            TargetType = targetType;
            MethodName = methodName;
            PatchType = patchType;
            ArgumentTypes = argumentTypes ?? Type.EmptyTypes;
        }
    }
}
