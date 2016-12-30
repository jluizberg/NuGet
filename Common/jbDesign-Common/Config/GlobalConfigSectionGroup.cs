// <copyright file="GlobalConfigSectionGroup.cs" company="jbDesign">
//     Copyright (c) jbDesign 2016. All rights reserved.
// </copyright>
// <summary>
// </summary>
namespace jbDesign.Config
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// Defines the global configuration object
    /// </summary>
    public class GlobalConfigSectionGroup : ConfigurationSectionGroup
    {
        /// <summary>
        /// Name of the container element
        /// </summary>
        public const string SectionNameContainer = "container";

        /// <summary>
        /// Gets the configuration section for the Unity container
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Method was not supposed to be static")]
        public UnityConfigurationSection Container
        {
            get
            {
                return (UnityConfigurationSection)GlobalConfig.Instance.ConfigSection.Sections[SectionNameContainer];
            }
        }
    }
}