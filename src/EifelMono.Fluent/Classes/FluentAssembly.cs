using System;
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

        public string Name
            => _Assembly?.GetName().Name ?? "";

        // <Version> 1.0.1 </Version>
        public string Version
            => CustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "";

        // <AssemblyVersion>1.0.2.0</AssemblyVersion>
        public string AssemblyVersion
            => _Assembly?.GetName()?.Version?.ToString() ?? "";
        // => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "";

        // <FileVersion>1.0.3.0</FileVersion>
        public string FileVersion
            => CustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "";

        // <Company>Company</Company>
        public string Company
            => CustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "";

        // <Copyright>Copyright</Copyright>
        public string Copyright
            => CustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? "";

        // <Title>Copyright</Title>
        public string Title
            => CustomAttribute<AssemblyTitleAttribute>()?.Title ?? "";

        // <Product>Copyright</Product>
        public string Product
            => CustomAttribute<AssemblyProductAttribute>()?.Product ?? "";

        public string Configuration
            => CustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ?? "";

        public string Culture
            => CustomAttribute<AssemblyCultureAttribute>()?.Culture ?? "";

        public string Description
            => CustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";

        public string Trademark
            => CustomAttribute<AssemblyTrademarkAttribute>()?.Trademark ?? "";

        public string TargetFrameworkName
            => CustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? "";
        public string TargetFrameworkDisplayName
            => CustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName ?? "";

        public override string ToString()
            => this.ToJson();
    }
}
