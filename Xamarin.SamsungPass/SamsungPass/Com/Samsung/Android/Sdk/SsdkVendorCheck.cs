using System;
using Android.Runtime;

namespace Com.Samsung.Android.Sdk
{
	[Register (SsdkVendorCheck.JniName, DoNotGenerateAcw=true)]
	public class SsdkVendorCheck : Java.Lang.Object
	{
		public const string JniName = "com/samsung/android/sdk/SsdkVendorCheck";

		static readonly IntPtr _classRef = JNIEnv.FindClass (JniName);

		#region Static Methods/Fields
		static IntPtr _isSamsungDevice;
		public static bool IsSamsungDevice 
		{
			get {
				if (_isSamsungDevice == IntPtr.Zero)
					_isSamsungDevice = JNIEnv.GetStaticMethodID(_classRef,
						"isSamsungDevice", "()Z");

				return JNIEnv.GetStaticBooleanField(_classRef, _isSamsungDevice);
			}
		}
		#endregion
	}
}

