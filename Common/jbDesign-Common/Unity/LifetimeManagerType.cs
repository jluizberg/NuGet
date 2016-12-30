// <copyright file="LifetimeManagerType.cs" company="jbDesign">
//     Copyright (c) jbDesign 2016. All rights reserved.
// </copyright>
// <summary>
// </summary>

namespace jbDesign.Unity
{
    /// <summary>
    /// Indicates the types of LifetimeManager to be applyed to a registration
    /// </summary>
    public enum LifetimeManagerType
    {
        /// <summary>
        /// No Lifetime Manager
        /// </summary>
        None = 0,

        /// <summary>
        /// Externally Controlled Manager
        /// </summary>
        ExternallyControlled,

        /// <summary>
        /// Container Controlled Manager
        /// </summary>
        ContainerControlled,

        /// <summary>
        /// Object Per Thread
        /// </summary>
        PerThread,

        /// <summary>
        /// Object Per Call
        /// </summary>
        PerResolve,

        /// <summary>
        /// Hierarquical Manager
        /// </summary>
        Hierarchical,

        /// <summary>
        /// Transient Manager
        /// </summary>
        Transient
    }
}