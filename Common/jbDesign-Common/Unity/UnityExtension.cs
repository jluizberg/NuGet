// <copyright file="UnityExtension.cs" company="jbDesign">
//     Copyright (c) jbDesign 2016. All rights reserved.
// </copyright>
// <summary>
// </summary>

namespace jbDesign.Unity
{
    using System;
    using System.Reflection;
    using jbDesign.Config;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Extension for Unity container
    /// </summary>
    public static class UnityExtension
    {
        /// <summary>
        /// Extension function to Unity, which register all annotated entities in alll assemblies.
        /// </summary>
        /// <param name="container">Container to register with.</param>
        /// <returns>The container itself, for allowing fluid configuration.</returns>
        public static UnityContainer RegisterDeclared(this UnityContainer container)
        {
            GlobalConfig.Instance.Logger.Debug("Registering types for all assemblies");

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                container.RegisterDeclared(assembly);
            }

            return container;
        }

        /// <summary>
        /// Extension function to Unity, which register all annotated entities in an assembly.
        /// </summary>
        /// <param name="container">Container to register with.</param>
        /// <param name="assembly">Assembly to be registered.</param>
        /// <returns>The container itself, for allowing fluid configuration.</returns>
        public static UnityContainer RegisterDeclared(this UnityContainer container, Assembly assembly)
        {
            if (assembly != null)
            {
                GlobalConfig.Instance.Logger.Debug("Registering types for assembly " + assembly.FullName);

                foreach (Type type in assembly.GetTypes())
                {
                    foreach (RegistrableAttribute reg in type.GetCustomAttributes(typeof(RegistrableAttribute), true))
                    {
                        reg.RegisterType(type);
                    }
                }
            }

            return container;
        }
    }
}