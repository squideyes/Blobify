﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blobify.Shared.Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Blobify.Shared.Tests.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to /SOURCE:C:\Windows /REGEX:.txt$ /RECURSE /CONTAINER:BADCONTAINERNAME /PATH:AAA/BBB /LOGLEVEL:WARN.
        /// </summary>
        public static string BadContainerName {
            get {
                return ResourceManager.GetString("BadContainerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /SOURCE:C:\Windows /REGEX:.txt$ /RECURSE /CONTAINER:files /PATH:AAA/BBB /LOGLEVEL:XXX.
        /// </summary>
        public static string BadLogLevel {
            get {
                return ResourceManager.GetString("BadLogLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /PARAMS:SomePath\GoodAllBlobifyParams.txt&gt;.
        /// </summary>
        public static string BadParams {
            get {
                return ResourceManager.GetString("BadParams", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /SOURCE:C:\Windows /REGEX:.txt$ /RECURSE /CONTAINER:files /PATH:C:\AAA\BBB /LOGLEVEL:WARN.
        /// </summary>
        public static string BadPath {
            get {
                return ResourceManager.GetString("BadPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /SOURCE:C:\Windows /REGEX:[a- /RECURSE /CONTAINER:files /PATH:AAA/BBB /LOGLEVEL:WARN.
        /// </summary>
        public static string BadRegex {
            get {
                return ResourceManager.GetString("BadRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /SOURCE:SomePath&gt; /REGEX:.txt$ /RECURSE /CONTAINER:files /PATH:AAA/BBB /LOGLEVEL:WARN.
        /// </summary>
        public static string BadSource {
            get {
                return ResourceManager.GetString("BadSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /SOURCE:C:\Windows /REGEX:.txt$ /RECURSE /CONTAINER:files /PATH:AAA/BBB /LOGLEVEL:WARN.
        /// </summary>
        public static string GoodBlobifyWithAllOptions {
            get {
                return ResourceManager.GetString("GoodBlobifyWithAllOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /SOURCE:C:\Windows /CONTAINER:files.
        /// </summary>
        public static string GoodBlobifyWithMinOptions {
            get {
                return ResourceManager.GetString("GoodBlobifyWithMinOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /CONN:DefaultEndpointsProtocol=https;AccountName=someco;AccountKey=O21totdNsyxIgHMPIq0jVyBhjxkikfeVkOPfCzzdExvc9Yl4VxTXuC0VBfu275etxQnY/tzBqArmTwpoQyPn0w==;.
        /// </summary>
        public static string GoodConnNoLogLevel {
            get {
                return ResourceManager.GetString("GoodConnNoLogLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /CONN:DefaultEndpointsProtocol=https;AccountName=someco;AccountKey=O21totdNsyxIgHMPIq0jVyBhjxkikfeVkOPfCzzdExvc9Yl4VxTXuC0VBfu275etxQnY/tzBqArmTwpoQyPn0w==; /LOGLEVEL:WARN.
        /// </summary>
        public static string GoodConnWithLogLevel {
            get {
                return ResourceManager.GetString("GoodConnWithLogLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /NOCONN.
        /// </summary>
        public static string GoodNoConnNoLogLevel {
            get {
                return ResourceManager.GetString("GoodNoConnNoLogLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /NOCONN /LOGLEVEL:WARN.
        /// </summary>
        public static string GoodNoConnWithLogLevel {
            get {
                return ResourceManager.GetString("GoodNoConnWithLogLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /PARAMS:ParamsFiles\GoodAllBlobifyParams.txt.
        /// </summary>
        public static string GoodParams {
            get {
                return ResourceManager.GetString("GoodParams", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /PARAMS:ParamsFiles\GoodAllBlobifyParams.txt /LOGLEVEL:WARN.
        /// </summary>
        public static string GoodParamsWithLogLevel {
            get {
                return ResourceManager.GetString("GoodParamsWithLogLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /SOURCE:C:\Windows /REGEX:.txt$ /XXX /CONTAINER:files /PATH:AAA/BBB /LOGLEVEL:WARN.
        /// </summary>
        public static string UnregognizedOptionIgnored {
            get {
                return ResourceManager.GetString("UnregognizedOptionIgnored", resourceCulture);
            }
        }
    }
}
