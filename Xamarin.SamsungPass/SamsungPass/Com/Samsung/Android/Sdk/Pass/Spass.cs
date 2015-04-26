using System;
using Android.Runtime;
using Android.Content;

namespace Com.Samsung.Android.Sdk.Pass
{
    [Register (Spass.JniName, DoNotGenerateAcw=true)]
	public class Spass : Java.Lang.Object, ISdkInterface
    {
        public const string JniName = "com/samsung/android/sdk/pass/Spass";

        #region JavaObject
        static readonly IntPtr _classRef = JNIEnv.FindClass (JniName);
		static readonly IntPtr _interfaceClassRef = JNIEnv.FindClass (ISsdkInterfaceInvoker.JniName);

        static IntPtr _constructor;

        [Register (".ctor", "()V", "")]
		public Spass () : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (Handle != IntPtr.Zero)
                return;

            //Create Android Callable Wrapper for derived type
            if (GetType () != typeof (Spass)) 
            {
                SetHandle (JNIEnv.CreateInstance (GetType (), "()V"), JniHandleOwnership.TransferLocalRef);
                return;
            }
                
            //Create the Handle from the constructor
            if (_constructor == IntPtr.Zero)
                _constructor = JNIEnv.GetMethodID (_classRef, "<init>", "()V");
            SetHandle (JNIEnv.NewObject (_classRef, _constructor), JniHandleOwnership.TransferLocalRef);
        }

        public Spass (IntPtr handle, JniHandleOwnership transfer)
            : base (handle, transfer)
        {
        }

        protected override Type ThresholdType {
            get { return typeof (Spass); }
        }

        protected override IntPtr ThresholdClass {
            get { return _classRef; }
        }
        #endregion

        #region Static Methods/Fields
        static IntPtr _deviceFingerprint;
        public static int DeviceFingerprint 
        {
            get {
                if (_deviceFingerprint == IntPtr.Zero)
                    _deviceFingerprint = JNIEnv.GetStaticFieldID(_classRef,
                        "DEVICE_FINGERPRINT", "I");

                return JNIEnv.GetStaticIntField(_classRef, _deviceFingerprint);
            }
        }

        static IntPtr _deviceFingerprintCustomizedDialog;
        public static int DeviceFingerprintCustomizedDialog 
        {
            get {
                if (_deviceFingerprintCustomizedDialog == IntPtr.Zero)
                    _deviceFingerprintCustomizedDialog = JNIEnv.GetStaticFieldID(_classRef,
                        "DEVICE_FINGERPRINT_CUSTOMIZED_DIALOG", "I");

                return JNIEnv.GetStaticIntField(_classRef, _deviceFingerprintCustomizedDialog);
            }
        }

        static IntPtr _deviceFingerprintFingerIndex;
        public static int DeviceFingerprintFingerIndex 
        {
            get {
                if (_deviceFingerprintFingerIndex == IntPtr.Zero)
                    _deviceFingerprintFingerIndex = JNIEnv.GetStaticFieldID(_classRef,
                        "DEVICE_FINGERPRINT_FINGER_INDEX", "I");

                return JNIEnv.GetStaticIntField(_classRef, _deviceFingerprintFingerIndex);
            }
        }
                   
        static IntPtr _deviceFingerprintUniqueId;
        public static int DeviceFingerprintUniqueId 
        {
            get {
                if (_deviceFingerprintUniqueId == IntPtr.Zero)
                    _deviceFingerprintUniqueId = JNIEnv.GetStaticFieldID(_classRef,
                        "DEVICE_FINGERPRINT_UNIQUE_ID", "I");

                return JNIEnv.GetStaticIntField(_classRef, _deviceFingerprintUniqueId);
            }
        }

        #endregion

        #region Instance Methods/Fields
		IntPtr _getVersionCode;
		public int GetVersionCode()
        {
			if (_getVersionCode == IntPtr.Zero)
				_getVersionCode = JNIEnv.GetMethodID (_interfaceClassRef, "getVersionCode", "()I");

			return JNIEnv.CallIntMethod(Handle, _getVersionCode);
        }

		IntPtr _getVersionName;
		public string GetVersionName()
        {
			if (_getVersionName == IntPtr.Zero)
				_getVersionName = JNIEnv.GetMethodID (_interfaceClassRef, "getVersionName", "()Ljava/lang/String;");

			var resultPtr = JNIEnv.CallObjectMethod(Handle, _getVersionName);
			return new Java.Lang.Object(resultPtr, JniHandleOwnership.TransferLocalRef).JavaCast<Java.Lang.String>().ToString();
        }
			
        IntPtr _initialize;
		public void Initialize(Context context)
        {
            if (_initialize == IntPtr.Zero)
				_initialize = JNIEnv.GetMethodID (_interfaceClassRef, "initialize", "(Landroid/content/Context;)V");

			JNIEnv.CallVoidMethod (Handle, _initialize, new JValue(context));
        }

        IntPtr _isFeatureEnabled;
        public bool IsFeatureEnabled(int feature)
        {
            if (_isFeatureEnabled == IntPtr.Zero)
                _isFeatureEnabled = JNIEnv.GetMethodID (_classRef, "isFeatureEnabled", "(I)Z");

            return JNIEnv.CallBooleanMethod(Handle, _isFeatureEnabled, new JValue(feature));
        }
        #endregion
    }
}

