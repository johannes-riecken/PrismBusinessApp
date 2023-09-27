//------------------------------------------------------------------------------

namespace AdventureWorks.WebServices.Strings {
    using System;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AdventureWorks.WebServices.Strings.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string ErrorCardNumberInvalidLength {
            get {
                return ResourceManager.GetString("ErrorCardNumberInvalidLength", resourceCulture);
            }
        }
        
        internal static string ErrorFirstNameRequired {
            get {
                return ResourceManager.GetString("ErrorFirstNameRequired", resourceCulture);
            }
        }
        
        internal static string ErrorInvalidZipCodeInState {
            get {
                return ResourceManager.GetString("ErrorInvalidZipCodeInState", resourceCulture);
            }
        }
        
        internal static string ErrorRegex {
            get {
                return ResourceManager.GetString("ErrorRegex", resourceCulture);
            }
        }
        
        internal static string ErrorRequired {
            get {
                return ResourceManager.GetString("ErrorRequired", resourceCulture);
            }
        }
        
        internal static string ErrorZipCodeInvalidLength {
            get {
                return ResourceManager.GetString("ErrorZipCodeInvalidLength", resourceCulture);
            }
        }
        
        internal static string InvalidAddress {
            get {
                return ResourceManager.GetString("InvalidAddress", resourceCulture);
            }
        }
        
        internal static string InvalidOrder {
            get {
                return ResourceManager.GetString("InvalidOrder", resourceCulture);
            }
        }
        
        internal static string InvalidPaymentMethod {
            get {
                return ResourceManager.GetString("InvalidPaymentMethod", resourceCulture);
            }
        }
        
        internal static string InvalidShoppingCart {
            get {
                return ResourceManager.GetString("InvalidShoppingCart", resourceCulture);
            }
        }
    }
}
