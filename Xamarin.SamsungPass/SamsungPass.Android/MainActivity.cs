using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Samsung.Android.Sdk.Pass;
using System.Collections.Generic;
using Java.Lang;
using Android.Util;

namespace SamsungPass.Android
{
	[Activity (MainLauncher = true)]
	public class MainActivity : Activity
	{
		SpassFingerprint _spassFingerprint;
		Spass _spass;
		Context _context;
		ListView _listView;
		ArrayAdapter<string> _listAdapter;
		bool _onReadyIdentify = false;
		bool _onReadyEnroll = false;
		bool _isFeatureEnabled = false;

		#region Interface implementations

		class IdentifyListener : Java.Lang.Object, IIdentifyListener
		{
			SpassFingerprint _spassFingerprint;
			readonly Action<string> _logAction;
			readonly Action _stopIdentifyAction;

			public IdentifyListener(SpassFingerprint spassFingerprint,
				Action<string> logAction, Action stopIdentifyAction)
			{
				_spassFingerprint = spassFingerprint;
				_logAction = logAction;
				_stopIdentifyAction = stopIdentifyAction; 
			}

			public void OnFinished (int responseCode)
			{
				_logAction("identify finished : reason=" + MainActivity.GetventStatusName(responseCode));
				_stopIdentifyAction.Invoke ();
				var fingerprintIndex = 0;
				try 
				{
					fingerprintIndex = _spassFingerprint.IdentifiedFingerprintIndex;
				} 
				catch (IllegalStateException e) 
				{
					_logAction(e.Message);
				}
					
				if (responseCode == SpassFingerprint.StatusAuthentificationSuccess) 
				{
					_logAction("OnFinished() : Identify authentification Success with FingerprintIndex : " + fingerprintIndex);
				} 
				else if (responseCode == SpassFingerprint.StatusAuthentificationPasswordSuccess) 
				{
					_logAction("OnFinished() : Password authentification Success");
				} 
				else 
				{
					_logAction("OnFinished() : Authentification Fail for identify");
				}
			}

			public void OnReady ()
			{
				_logAction.Invoke ("Identify state is ready");
			}

			public void OnStarted ()
			{
				_logAction.Invoke ("User touched fingerprint sensor!");
			}
		}

		class RegisterListener : Java.Lang.Object, IRegisterListener
		{
			readonly Action<string> _logAction;
			readonly Action _unenrollAction;

			public RegisterListener(Action<string> logAction, Action unenrollAction)
			{
				_logAction = logAction;
				_unenrollAction = unenrollAction;
			}

			public void OnFinished ()
			{
				_unenrollAction.Invoke ();
				_logAction.Invoke("RegisterListener.onFinished()");
			}
		}

		#endregion

		#region Lifecycle methods

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Main);
			_context = this;
			_listAdapter = new ArrayAdapter<string>(this, Resource.Layout.ListEntry, Resource.Id.TextView01);
			_listView = FindViewById<ListView> (Resource.Id.listView1);

			if (_listView != null)
				_listView.Adapter = _listAdapter;
			
			_spass = new Spass();

			try {
				_spass.Initialize(this);
			} catch (SsdkUnsupportedException e) {
				Log("Exception: " + e);
				return;
			} catch (UnsupportedOperationException){
				Log("Fingerprint Service is not supported in the device");
				return;
			} catch (Java.Lang.Exception) {
				Log("Did you set the manifest permission?");
				return;
			}
				
			_isFeatureEnabled = _spass.IsFeatureEnabled(Spass.DeviceFingerprint);

			if (_isFeatureEnabled)
			{
				_spassFingerprint = new SpassFingerprint(this);
				Log("Fingerprint Service is supported in the device.");
				Log("SDK version : " + _spass.VersionName);
			} else { 
				LogClear();
				Log("Fingerprint Service is not supported in the device.");
				return;
			}

			FindViewById (Resource.Id.buttonHasRegisteredFinger).Click += (sender, e) => {
				LogClear();
				try {
					var hasRegisteredFinger = _spassFingerprint.HasRegisteredFinger;
					Log("HasRegisteredFinger = " + hasRegisteredFinger);
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonIdentify).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							try {
								_onReadyIdentify = true;
								_spassFingerprint.StartIdentify(new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false));
								Log("Please identify finger to verify you");
							} catch (SpassInvalidStateException m) {
								_onReadyIdentify = false;
								if (m.Type == SpassInvalidStateException.StatusOperationDenied) {
									Log("Exception: " + m.Message);
								}
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonShowIdentifyDialogWithPW).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							try {
								_onReadyIdentify = true;
								_spassFingerprint.StartIdentifyWithDialog(this, new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false), true);
								Log("Please identify finger to verify you");
							} catch (SpassInvalidStateException m) {
								_onReadyIdentify = false;
								if (m.Type == SpassInvalidStateException.StatusOperationDenied) {
									Log("Exception: " + m.Message);
								}
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonShowIdentifyDialogWithoutPW).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							try {
								_onReadyIdentify = true;
								_spassFingerprint.StartIdentifyWithDialog(this, new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false), false);
								Log("Please identify finger to verify you");
							} catch (SpassInvalidStateException m) {
								_onReadyIdentify = false;
								if (m.Type == SpassInvalidStateException.StatusOperationDenied) {
									Log("Exception: " + m.Message);
								}
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonCancel).Click += (sender, e) => {
				LogClear();
				try {
					if (_onReadyIdentify) {
						try {
							_spassFingerprint.CancelIdentify();
							Log("cancelIdentify is called");
						} catch (IllegalStateException ise) {
							Log(ise.Message);
						}
						_onReadyIdentify = false;
					} else {
						Log("Please request Identify first");
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonRegisterFinger).Click += (sender, e) => {
				LogClear();
				try {
					if (!_onReadyIdentify) {
						if (!_onReadyEnroll) {
							_onReadyEnroll = true;
							_spassFingerprint.RegisteredFinger(this, new RegisterListener(Log, () => _onReadyEnroll = false));
							Log("Jump to the Enroll screen");
						} else {
							Log("Please wait and try to register again");
						}
					} else {
						Log("Please cancel Identify first");
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonGetRegisteredFingerprintName).Click += (sender, e) => {
				LogClear();
				try {
					Log("=Fingerprint Name=");
					var list = _spassFingerprint.RegisteredFingerprintName;
					if (list == null) {
						Log("Registered fingerprint is not existed.");
					} else {
						for (var i=0; i < list.Size(); i++)
						{
							var index = list.KeyAt(i);
							var name = list.Get(index);
							Log("index " + index + ", Name is " + name);
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonGetRegisteredFingerprintID).Click += (sender, e) => {
				LogClear();
				try {
					if (_spass.IsFeatureEnabled(Spass.DeviceFingerprintUniqueId)) {
						SparseArray list;
						try {
							Log("=Fingerprint Unique ID=");
							list = _spassFingerprint.RegisteredFingerprintUniqueId;
							if (list == null) {
								Log("Registered fingerprint is not existed.");
							} else {
								for (var i=0; i < list.Size(); i++)
								{
									var index = list.KeyAt(i);
									var id = list.Get(index);
									Log("index " + index + ", Unique ID is " + id);
								}
							}
						} catch (IllegalStateException ise) {
							Log(ise.Message);
						}
					} else {
						Log("To get Fingerprint ID is not supported in the device");
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonIdentifyWithIndex).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							try {
								_onReadyIdentify = true;
								if (_spass.IsFeatureEnabled(Spass.DeviceFingerprintFingerIndex)) {
									var designatedFingers = new Java.Util.ArrayList();
									designatedFingers.Add(1);
									_spassFingerprint.SetIntendedFingerprintIndex(designatedFingers);
								}
								_spassFingerprint.StartIdentify(new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false));
								Log("Please identify fingerprint index 1 to verify you");
							} catch (SpassInvalidStateException ise) {
								_onReadyIdentify = false;
								if (ise.Type == SpassInvalidStateException.StatusOperationDenied) {
									Log("Exception: " + ise.Message);
								}
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonShowIdentifyDialogWithIndex).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							try {
								_onReadyIdentify = true;
								if (_spass.IsFeatureEnabled(Spass.DeviceFingerprintFingerIndex)) {
									var designatedFingers = new Java.Util.ArrayList();
									designatedFingers.Add(2);
									designatedFingers.Add(3);
									_spassFingerprint.SetIntendedFingerprintIndex(designatedFingers);
								}
								_spassFingerprint.StartIdentifyWithDialog(this, new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false), false);
								Log("Please identify fingerprint index 2 or 3 to verify you");
							} catch (SpassInvalidStateException ise) {
								_onReadyIdentify = false;
								if (ise.Type == SpassInvalidStateException.StatusOperationDenied) {
									Log("Exception: " + ise.Message);
								}
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonShowIdentifyDialogWithTitleNLogo).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							_onReadyIdentify = true;
							if (_spass.IsFeatureEnabled(Spass.DeviceFingerprintCustomizedDialog)) {
								try {
									_spassFingerprint.SetDialogTitle("Customized Dialog With Logo", 0x000000);
									_spassFingerprint.SetDialogIcon("Icon");
								} catch (IllegalStateException ise) {
									Log(ise.Message);
								}
							}
							try {
								_spassFingerprint.StartIdentifyWithDialog(this, new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false), false);
								Log("Please Identify fingerprint to verify you");
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonCustomizedDialogWithTransparency).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							_onReadyIdentify = true;
							if (_spass.IsFeatureEnabled(Spass.DeviceFingerprintCustomizedDialog)) {
								try {
									_spassFingerprint.SetDialogTitle("Customized Dialog With Transparency", 0x000000);
									_spassFingerprint.SetDialogBgTransparency(0);
								} catch (IllegalStateException ise) {
									Log(ise.Message);
								}
							}
							try {
								_spassFingerprint.StartIdentifyWithDialog(this, new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false), false);
								Log("Please Identify fingerprint to verify you");
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};

			FindViewById (Resource.Id.buttonCustomizedDialogWithSetDialogDismiss).Click += (sender, e) => {
				LogClear();
				try {
					if (!_spassFingerprint.HasRegisteredFinger) {
						Log("Please register finger first");
					} else {
						if (!_onReadyIdentify) {
							_onReadyIdentify = true;
							if (_spass.IsFeatureEnabled(Spass.DeviceFingerprintCustomizedDialog)) {
								try {
									_spassFingerprint.SetDialogTitle("Customized Dialog With Setting Dialog dismiss", 0x000000);
									_spassFingerprint.SetCanceledOnTouchOutside(true);
								} catch (IllegalStateException ise) {
									Log(ise.Message);
								}
							}
							try {
								_spassFingerprint.StartIdentifyWithDialog(this, new IdentifyListener (_spassFingerprint,
									Log, () => _onReadyIdentify = false), false);
								Log("Please Identify fingerprint to verify you");
							} catch (IllegalStateException ex) {
								_onReadyIdentify = false;
								Log("Exception: " + ex);
							}
						} else {
							Log("Please cancel Identify first");
						}
					}
				} catch (UnsupportedOperationException) {
					Log("Fingerprint Service is not supported in the device");
				}
			};
		}

		public override bool OnCreateOptionsMenu(IMenu menu) {
			// Inflate the menu; this adds items to the action bar if it is present.
			MenuInflater.Inflate(Resource.Menu.Main, menu);
			return true;
		}

		#endregion

		#region Other methods
		static string GetventStatusName(int responseCode) {
			if (responseCode == SpassFingerprint.StatusAuthentificationSuccess)
				return "STATUS_AUTHENTIFICATION_SUCCESS";
			else if (responseCode == SpassFingerprint.StatusAuthentificationPasswordSuccess)
				return "STATUS_AUTHENTIFICATION_PASSWORD_SUCCESS";
			else if (responseCode == SpassFingerprint.StatusTimeoutFailed)
				return "STATUS_TIMEOUT";
			else if (responseCode == SpassFingerprint.StatusTimeoutFailed)
				return "STATUS_SENSOR_ERROR";
			else if (responseCode == SpassFingerprint.StatusSensorFailed)
				return "STATUS_USER_CANCELLED";
			else if (responseCode == SpassFingerprint.StatusUserCancelled)
				return "STATUS_QUALITY_FAILED";
			else if (responseCode == SpassFingerprint.StatusQualityFailed)
				return "STATUS_USER_CANCELLED_BY_TOUCH_OUTSIDE";
			else
				return "STATUS_AUTHENTIFICATION_FAILED";
		}

		public void Log(string text) {
			RunOnUiThread(() =>
			{
				_listAdapter.Add(text);
				_listView.InvalidateViews();
				_listAdapter.NotifyDataSetChanged();
			});
		}

		public void LogClear() {
			if (_listAdapter != null) {
				_listAdapter.Clear ();
				_listAdapter.NotifyDataSetChanged();
			}
		}
		#endregion
	}
}


