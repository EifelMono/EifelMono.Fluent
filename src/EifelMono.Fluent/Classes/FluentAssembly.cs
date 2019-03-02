using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Versioning;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;

namespace EifelMono.Fluent.Classes
{
    public class AssemblyInfo
    {
        protected Assembly _Assembly;

        public AssemblyInfo(Assembly assembly)
        {
            _Assembly = assembly;
        }
        public T CustomAttribute<T>() where T : Attribute
        {
            try
            {
                return _Assembly?.GetCustomAttribute<T>() ?? null;
            }
            catch
            {
                return null;
            }
        }

        private FilePath _location;

        public FilePath Location { get => _location ?? (_location = new FilePath(_Assembly.Location)); }

        [DefaultValue("")]
        public string Name
            => _Assembly?.GetName().Name ?? "";

        // <Version> 1.0.1 </Version>
        [DefaultValue("")]
        public string Version
            => CustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "";

        // <AssemblyVersion>1.0.2.0</AssemblyVersion>
        [DefaultValue("")]
        public string AssemblyVersion
            => _Assembly?.GetName()?.Version?.ToString() ?? "";
        // => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "";

        // <FileVersion>1.0.3.0</FileVersion>
        [DefaultValue("")]
        public string FileVersion
            => CustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "";

        // <Company>Company</Company>
        [DefaultValue("")]
        public string Company
            => CustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "";

        // <Copyright>Copyright</Copyright>
        [DefaultValue("")]
        public string Copyright
            => CustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? "";

        // <Title>Copyright</Title>
        [DefaultValue("")]
        public string Title
            => CustomAttribute<AssemblyTitleAttribute>()?.Title ?? "";

        // <Product>Copyright</Product>
        [DefaultValue("")]
        public string Product
            => CustomAttribute<AssemblyProductAttribute>()?.Product ?? "";

        [DefaultValue("")]
        public string Configuration
            => CustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ?? "";

        [DefaultValue("")]
        public string Culture
            => CustomAttribute<AssemblyCultureAttribute>()?.Culture ?? "";

        [DefaultValue("")]
        public string Description
            => CustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";

        [DefaultValue("")]
        public string Trademark
            => CustomAttribute<AssemblyTrademarkAttribute>()?.Trademark ?? "";

        [DefaultValue("")]
        public string TargetFrameworkName
            => CustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? "";

        [DefaultValue("")]
        public string TargetFrameworkDisplayName
            => CustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName ?? "";
        [DefaultValue("")]
        public override string ToString()
            => this.ToJson(defaults: false);
    }
}
