// <copyright file="ConfigBase.cs" company="jbDesign">
//     Copyright (c) jbDesign 2016 All rights reserved.
// </copyright>
// <summary>
// </summary>
namespace jbDesign.Config
{
    using System.Configuration;
    using System.Diagnostics;
    using log4net;
    using log4net.Config;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Base interface for configuration resources.
    /// </summary>
    public interface IConfigBase
    {
        /// <summary>
        /// Gets or sets the container instance for the configuration.
        /// If the container is not defined on initialization, a new instance will be created at first use.
        /// </summary>
        IUnityContainer Container { get; set; }

        /// <summary>
        /// Gets the global logger (Log4Net), pointing to the requesting class using the stack trace.
        /// </summary>
        ILog Logger { get; }
    }

    /// <summary>
    /// Base abstract object to the inherited by all configurations.
    /// </summary>
    public abstract class ConfigBase : IConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigBase" /> class.
        /// </summary>
        protected ConfigBase()
            : base()
        {
            ((GlobalConfig)GlobalConfig.Instance).OnContainerInitialized += this.ContainerInitialized;
        }

        /// <summary>
        /// Gets or sets the container instance for the configuration.
        /// If the container is not defined on initialization, a new instance will be created on first use.
        /// </summary>
        public virtual IUnityContainer Container
        {
            get { return GlobalConfig.Instance.Container; }
            set { GlobalConfig.Instance.Container = value; }
        }

        /// <summary>
        /// Gets the global logger (Log4Net), pointing to the requesting class using the stack trace.
        /// </summary>
        public ILog Logger
        {
            get
            {
                return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType);
            }
        }

        /// <summary>
        /// Delegate for container initialization
        /// </summary>
        /// <param name="container">Novo container criado.</param>
        public abstract void ContainerInitialized(object container);
    }
}