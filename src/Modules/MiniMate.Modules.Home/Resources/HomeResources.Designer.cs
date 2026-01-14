namespace MiniMate.Modules.Home.Resources
{
    using System;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class HomeResources
    {
        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal HomeResources()
        {
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MiniMate.Modules.Home.Resources.HomeResources", typeof(HomeResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        public static string PageTitle
        {
            get
            {
                return ResourceManager.GetString("PageTitle", resourceCulture);
            }
        }

        public static string Greeting
        {
            get
            {
                return ResourceManager.GetString("Greeting", resourceCulture);
            }
        }

        public static string ExploreToday
        {
            get
            {
                return ResourceManager.GetString("ExploreToday", resourceCulture);
            }
        }
    }
}
