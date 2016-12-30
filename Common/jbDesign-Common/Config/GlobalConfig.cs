// <copyright file="GlobalConfig.cs" company="jbDesign">
//     Copyright (c) jbDesign 2016. All rights reserved.
// </copyright>
// <summary>
// </summary>

namespace jbDesign.Config
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Configuration;
    using jbDesign.Unity;
    using log4net;
    using log4net.Config;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Interface for the global configuration node.
    /// </summary>
    public interface IGlobalConfig : IConfigBase
    {
        /// <summary>
        /// Gets the main config section
        /// </summary>
        GlobalConfigSectionGroup ConfigSection { get; }
    }

    /// <summary>
    /// Global configuration object for the library.
    /// </summary>
    public sealed class GlobalConfig : IGlobalConfig, IDisposable
    {
        /// <summary>
        /// The singleton instance of the configuration
        /// </summary>
        private static IGlobalConfig instance;

        /// <summary>
        /// The container instance
        /// </summary>
        private IUnityContainer container;

        /// <summary>
        /// Delegate for initializing containers
        /// </summary>
        private OnContainerInitializedHandler onContainerInitialized;

        /// <summary>
        /// The global configuration data
        /// </summary>
        private GlobalConfigSectionGroup sectionGroup;

        /// <summary>
        /// Prevents a default instance of the<see cref="GlobalConfig" /> class from being created.
        /// </summary>
        private GlobalConfig()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GlobalConfig" /> class.
        /// </summary>
        ~GlobalConfig()
        {
            this.Dispose();
        }

        /// <summary>
        /// Delegate called after the container is initialized
        /// </summary>
        /// <param name="container">The new container</param>
        internal delegate void OnContainerInitializedHandler(object container);

        /// <summary>
        /// Gets a singleton configuration object
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Object is global and will never be disposed")]
        public static IGlobalConfig Instance
        {
            get
            {
                if (GlobalConfig.instance == null)
                {
                    GlobalConfig.instance = new GlobalConfig();
                }

                return GlobalConfig.instance;
            }
        }

        /// <summary>
        /// Gets the configuration data
        /// </summary>
        public GlobalConfigSectionGroup ConfigSection
        {
            get
            {
                if (this.sectionGroup == null)
                {
                    Configuration config;

                    try
                    {
                        config = HttpContext.Current != null ?
                            WebConfigurationManager.OpenWebConfiguration("~") :
                            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    }
                    catch (ConfigurationErrorsException)
                    {
                        config = ConfigurationManager.OpenMachineConfiguration();
                    }

                    this.sectionGroup = config.SectionGroups.OfType<GlobalConfigSectionGroup>().FirstOrDefault();
                }

                return this.sectionGroup;
            }
        }

        /// <summary>
        /// Gets or sets the container.
        /// If not defined, a new instance is created on first use.
        /// </summary>
        public IUnityContainer Container
        {
            get
            {
                if (this.container == null)
                {
                    this.InitializeContainer();
                }

                return this.container;
            }

            set
            {
                if (this.container != value)
                {
                    this.container = value;
                    ((UnityContainer)this.container).RegisterDeclared();

                    if (this.container != null && this.OnContainerInitialized != null)
                    {
                        this.OnContainerInitialized(this.container);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a Log4Net logger, configured as the calling class
        /// </summary>
        public ILog Logger
        {
            get
            {
                return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType);
            }
        }

        /// <summary>
        /// Gets or sets the deledate for container initialization
        /// </summary>
        internal OnContainerInitializedHandler OnContainerInitialized
        {
            get
            {
                return this.onContainerInitialized;
            }

            set
            {
                this.onContainerInitialized += value;

                if (value != null && this.container != null)
                {
                    value(this.container);
                }
            }
        }

        /// <summary>
        /// Releases any resources assigned
        /// </summary>
        public void Dispose()
        {
            this.Logger.Debug("Disposing GlobalConfig");

            if (this.container != null)
            {
                this.container.Dispose();
                GC.SuppressFinalize(this);
            }

            this.container = null;
        }

        /// <summary>
        /// Initializes the container
        /// </summary>
        private void InitializeContainer()
        {
            if (this.container == null)
            {
                this.Logger.Debug("Creating a new Unity container");

                this.container = new UnityContainer();

                if (this.ConfigSection != null &&
                    this.ConfigSection.Container != null &&
                    this.ConfigSection.Container.Containers != null &&
                    this.ConfigSection.Container.Containers.Count > 0)
                {
                    this.ConfigSection.Container.Configure(this.container);
                }

                ((UnityContainer)this.container).RegisterDeclared();

                if (this.OnContainerInitialized != null)
                {
                    this.Logger.Debug("Firing container initialization delegate");
                    this.OnContainerInitialized(this.container);
                }
            }
        }
    }
}