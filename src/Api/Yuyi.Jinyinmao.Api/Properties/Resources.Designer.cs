﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Yuyi.Jinyinmao.Api.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Yuyi.Jinyinmao.Api.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 恭喜您成功注册金银猫！如非本人操作，请致电4008556333.
        /// </summary>
        internal static string Sms_SignUpSuccessful {
            get {
                return ResourceManager.GetString("Sms_SignUpSuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 验证码 {0}，{1}分钟内有效。请勿泄漏。如非本人操作，请致电4008556333【工作人员不会向您索取验证码】.
        /// </summary>
        internal static string Sms_VeriCode {
            get {
                return ResourceManager.GetString("Sms_VeriCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 您正在修改登录密码，验证码 {0}，{1}分钟内有效。请勿泄漏。如非本人操作，请致电4008556333【工作人员不会向您索取验证码】.
        /// </summary>
        internal static string Sms_VeriCode_ResetLoginPawword {
            get {
                return ResourceManager.GetString("Sms_VeriCode_ResetLoginPawword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 您正在修改支付密码，验证码 {0}，{1}分钟内有效。请勿泄漏。如非本人操作，请致电4008556333【工作人员不会向您索取验证码】.
        /// </summary>
        internal static string Sms_VeriCode_ResetPaymentPawword {
            get {
                return ResourceManager.GetString("Sms_VeriCode_ResetPaymentPawword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 您正在注册金银猫账户，验证码 {0}，{1}分钟内有效。请勿泄漏。如非本人操作，请致电4008556333【工作人员不会向您索取验证码】.
        /// </summary>
        internal static string Sms_VeriCode_SignUp {
            get {
                return ResourceManager.GetString("Sms_VeriCode_SignUp", resourceCulture);
            }
        }
    }
}
