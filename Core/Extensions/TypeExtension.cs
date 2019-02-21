using System;

namespace Core.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// Test whether type is complex type and not system type
        /// </summary>
        /// <param name="nestedType"></param>
        /// <returns></returns>
        public static bool IsDefinedComplexType(this Type nestedType)
        {
            return !nestedType.IsSystemType() && nestedType.IsClass;
        }
        
        /// <summary>
        /// Test whether type is system defined type
        /// </summary>
        /// <param name="nestedType"></param>
        /// <returns></returns>
        public static bool IsSystemType(this Type nestedType)
        {
            return nestedType.Namespace.StartsWith("System");
        }
    }
}