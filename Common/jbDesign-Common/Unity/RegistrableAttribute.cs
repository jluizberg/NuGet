// <copyright file="RegistrableAttribute.cs" company="jbDesign">
//     Copyright (c) jbDesign 2016. All rights reserved.
// </copyright>
// <summary>
// </summary>

namespace jbDesign.Unity
{
    using System;
    using Config;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Represents a basic abstract annotation to be extended.
    /// Defines a class that was goign to be registered automatically to the Unity container.
    /// </summary>
    public abstract class RegistrableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the<see cref="RegistrableAttribute" /> class.
        /// </summary>
        /// <param name="typeFrom">Type to register from</param>
        protected RegistrableAttribute(Type typeFrom)
        {
            this.TypeFrom = typeFrom;
            this.LifetimeManagerType = LifetimeManagerType.Transient;
        }

        /// <summary>
        /// Gets or sets the lifetime manager class
        /// </summary>
        public LifetimeManagerType LifetimeManagerType { get; set; }

        /// <summary>
        /// Gets or sets the name assigned to the type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type to be used in registration from.
        /// </summary>
        public Type TypeFrom { get; protected set; }

        /// <summary>
        /// Gets the type mapped to registration
        /// </summary>
        /// <param name="type">The type this entity is being mapped to.</param>
        /// <returns>The type to register with.</returns>
        public virtual Type GetMappedTypeFrom(Type type)
        {
            return this.TypeFrom;
        }

        /// <summary>
        /// Register a class in the container using the specified values
        /// </summary>
        /// <param name="type">Class to be registered.</param>
        public virtual void RegisterType(Type type)
        {
            if (type != null &&
                this.GetMappedTypeFrom(type).IsAssignableFrom(type))
            {
                GlobalConfig.Instance.Container.RegisterType(this.GetMappedTypeFrom(type), type, this.Name, this.UsingLifetimeManager(type));
            }
        }

        /// <summary>
        /// Declare the type of lifetime manager to be used in registration.
        /// </summary>
        /// <param name="type">The type this entity is being mapped to.</param>
        /// <returns>The lifetime manager to be used.</returns>
        protected virtual LifetimeManager UsingLifetimeManager(Type type)
        {
            switch (LifetimeManagerType)
            {
                case LifetimeManagerType.None:
                    return WithLifetime.None(this.GetMappedTypeFrom(type));

                case LifetimeManagerType.ExternallyControlled:
                    return WithLifetime.ExternallyControlled(this.GetMappedTypeFrom(type));

                case LifetimeManagerType.ContainerControlled:
                    return WithLifetime.ContainerControlled(this.GetMappedTypeFrom(type));

                case LifetimeManagerType.PerThread:
                    return WithLifetime.PerThread(this.GetMappedTypeFrom(type));

                case LifetimeManagerType.PerResolve:
                    return WithLifetime.PerResolve(this.GetMappedTypeFrom(type));

                case LifetimeManagerType.Hierarchical:
                    return WithLifetime.Hierarchical(this.GetMappedTypeFrom(type));

                case LifetimeManagerType.Transient:
                    return WithLifetime.Transient(this.GetMappedTypeFrom(type));
            }

            return null;
        }
    }
}