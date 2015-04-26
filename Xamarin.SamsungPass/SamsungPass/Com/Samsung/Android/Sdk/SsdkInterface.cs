using System;
using Android.Runtime;
using Android.Content;

namespace Com.Samsung.Android.Sdk
{
	[Register (ISsdkInterfaceInvoker.JniName, DoNotGenerateAcw=true)]
	public interface ISdkInterface : IJavaObject {
		[Register ("initialize", "(Landroid/content/Context;)V", "GetInitializeHandler:Com.Samsung.Android.Sdk.ISsdkInterfaceInvoker, SamsungPass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
		void Initialize (Context context);

		[Register ("isFeatureEnabled", "(I)Z", "GetIsFeatureEnabledHandler:Com.Samsung.Android.Sdk.ISsdkInterfaceInvoker, SamsungPass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
		bool IsFeatureEnabled (int feature);

		[Register ("getVersionCode", "()I", "GetVersionCodeHandler:Com.Samsung.Android.Sdk.ISsdkInterfaceInvoker, SamsungPass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
		int GetVersionCode();

		[Register ("getVersionName", "()Ljava/lang/String;", "GetVersionNameHandler:Com.Samsung.Android.Sdk.ISsdkInterfaceInvoker, SamsungPass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
		string GetVersionName();
	}

	public class ISsdkInterfaceInvoker : Java.Lang.Object, ISdkInterface {

		public const string JniName = "com/samsung/android/sdk/SsdkInterface";

		IntPtr _instanceClassRef;

		public ISsdkInterfaceInvoker (IntPtr handle, JniHandleOwnership transfer)
			: base (handle, transfer)
		{
			IntPtr lref = JNIEnv.GetObjectClass (Handle);
			_instanceClassRef = JNIEnv.NewGlobalRef (lref);
			JNIEnv.DeleteLocalRef (lref);
		}

		protected override void Dispose (bool disposing)
		{
			if (_instanceClassRef != IntPtr.Zero)
				JNIEnv.DeleteGlobalRef (_instanceClassRef);
			_instanceClassRef = IntPtr.Zero;
			base.Dispose (disposing);
		}

		protected override Type ThresholdType {
			get {return typeof (ISsdkInterfaceInvoker);}
		}

		protected override IntPtr ThresholdClass {
			get {return _instanceClassRef;}
		}

		public static ISsdkInterfaceInvoker GetObject (IntPtr handle, JniHandleOwnership transfer)
		{
			return new ISsdkInterfaceInvoker (handle, transfer);
		}

		#region Interface Methods
		IntPtr _initialize;
		static Delegate _initializeCallback;
		public void Initialize(Context context)
		{
			if (_initialize == IntPtr.Zero)
				_initialize = JNIEnv.GetMethodID (_instanceClassRef, "initialize", "(Landroid/content/Context;)V");
			JNIEnv.CallVoidMethod (Handle, _initialize, new JValue(context));
		}

		#pragma warning disable 0169
		static Delegate GetInitializeHandler ()
		{
			if (_initializeCallback == null)
				_initializeCallback = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, Context>) InvokeInitialize);
			return _initializeCallback;
		}

		static void InvokeInitialize (IntPtr jnienv, IntPtr lrefThis, Context context)
		{
			var __this = Java.Lang.Object.GetObject<ISdkInterface>(lrefThis, JniHandleOwnership.DoNotTransfer);
			__this.Initialize (context);
		}
		#pragma warning restore 0169

		IntPtr _isFeatureEnabled;
		static Delegate _isFeatureEnabledCallback;

		public bool IsFeatureEnabled(int feature)
		{
			if (_isFeatureEnabled == IntPtr.Zero)
				_isFeatureEnabled = JNIEnv.GetMethodID (_instanceClassRef, "isFeatureEnabled", "(I)Z");
			return JNIEnv.CallBooleanMethod (Handle, _isFeatureEnabled,
				new JValue (feature));
		}

		#pragma warning disable 0169
		static Delegate GetIsFeatureEnabledHandler ()
		{
			if (_isFeatureEnabledCallback == null)
				_isFeatureEnabledCallback = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, int>) InvokeIsFeatureEnabled);
			return _isFeatureEnabledCallback;
		}

		static void InvokeIsFeatureEnabled (IntPtr jnienv, IntPtr lrefThis, int feature)
		{
			var __this = Java.Lang.Object.GetObject<ISdkInterface>(lrefThis, JniHandleOwnership.DoNotTransfer);
			__this.IsFeatureEnabled (feature);
		}
		#pragma warning restore 0169

		IntPtr _getVersionCode;
		static Delegate _versionCodeCallback;
		public int GetVersionCode()
		{
			if (_getVersionCode == IntPtr.Zero)
				_getVersionCode = JNIEnv.GetMethodID (_instanceClassRef, "getVersionCode", "()I");
			return JNIEnv.CallIntMethod (Handle, _getVersionCode);
		}

		#pragma warning disable 0169
		static Delegate GetVersionCodeHandler ()
		{
			if (_versionCodeCallback == null)
				_versionCodeCallback = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) InvokeGetVersionCode);
			return _versionCodeCallback;
		}

		static void InvokeGetVersionCode (IntPtr jnienv, IntPtr lrefThis)
		{
			var __this = Java.Lang.Object.GetObject<ISdkInterface>(lrefThis, JniHandleOwnership.DoNotTransfer);
			__this.GetVersionCode();
		}
		#pragma warning restore 0169

		IntPtr _getVersionName;
		static Delegate _versionNameCallback;
		public string GetVersionName()
		{
			if (_getVersionName == IntPtr.Zero)
				_getVersionName = JNIEnv.GetMethodID (_instanceClassRef, "getVersionName", "()Ljava/lang/String;");

			var resultPtr = JNIEnv.CallObjectMethod(Handle, _getVersionName);
			return new Java.Lang.Object(resultPtr, JniHandleOwnership.TransferLocalRef).JavaCast<Java.Lang.String>().ToString();
		}

		#pragma warning disable 0169
		static Delegate GetVersionNameHandler ()
		{
			if (_versionNameCallback == null)
				_versionNameCallback = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr>) InvokeGetVersionName);
			return _versionNameCallback;
		}

		static void InvokeGetVersionName (IntPtr jnienv, IntPtr lrefThis)
		{
			var __this = Java.Lang.Object.GetObject<ISdkInterface>(lrefThis, JniHandleOwnership.DoNotTransfer);
			__this.GetVersionName();
		}
		#pragma warning restore 0169

		#endregion
	}
}

