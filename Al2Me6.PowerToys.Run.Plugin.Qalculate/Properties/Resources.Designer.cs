﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Al2Me6.PowerToys.Run.Plugin.Qalculate.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Al2Me6.PowerToys.Run.Plugin.Qalculate.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not copy to clipboard.
        /// </summary>
        public static string copy_failed {
            get {
                return ResourceManager.GetString("copy_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Copy result to clipboard.
        /// </summary>
        public static string copy_to_clipboard {
            get {
                return ResourceManager.GetString("copy_to_clipboard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to evaluate input.
        /// </summary>
        public static string failure_title {
            get {
                return ResourceManager.GetString("failure_title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Advanced unit-aware calculator. `qalc.exe` must be available from the PATH. e.g., &quot;7g/cm^3 * 1 in^3&quot;, &quot;integrate(x * sqrt(x))&quot;, &quot;75 oF to oC&quot;, &quot;0o1234 to hex&quot;..
        /// </summary>
        public static string plugin_description {
            get {
                return ResourceManager.GetString("plugin_description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Qalculate.
        /// </summary>
        public static string plugin_name {
            get {
                return ResourceManager.GetString("plugin_name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not execute Qalculate.
        /// </summary>
        public static string qalc_exec_error {
            get {
                return ResourceManager.GetString("qalc_exec_error", resourceCulture);
            }
        }
    }
}