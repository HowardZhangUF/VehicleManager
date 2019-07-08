using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using VehicleSimulator.Interface;
using static TrafficControlTest.Library.Library;
using static VehicleSimulator.Library.EventHandlerLibraryOfIVehicleSimulator;

namespace VehicleSimulator.Implement
{
	public class VehicleSimulatorInfo : IVehicleSimulatorInfo
	{
		public event EventHandlerIVehicleSimulator StateUpdated;

		public string mName
		{
			get
			{
				return _Name;
			}
			private set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_Name = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mState
		{
			get
			{
				return _State;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && _State != value)
				{
					_State = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public IPoint2D mPosition
		{
			get
			{
				return _Position;
			}
			set
			{
				if (value != null)
				{
					_Position = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mToward
		{
			get
			{
				return _Toward;
			}
			set
			{
				if (_Toward != value)
				{
					_Toward = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mTarget
		{
			get
			{
				return _Target;
			}
			set
			{
				if (_Target != value)
				{
					_Target = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mBufferTarget
		{
			get
			{
				return _BufferTarget;
			}
			set
			{
				if (_BufferTarget != value)
				{
					_BufferTarget = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mTranslationVelocity
		{
			get
			{
				return _TranslationVelocity;
			}
			set
			{
				if (_TranslationVelocity != value)
				{
					_TranslationVelocity = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mRotationVeloctiy
		{
			get
			{
				return _RotationVeloctiy;
			}
			set
			{
				if (_RotationVeloctiy != value)
				{
					_RotationVeloctiy = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mMapMatch
		{
			get
			{
				return _MapMatch;
			}
			set
			{
				if (_MapMatch != value)
				{
					_MapMatch = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mBattery
		{
			get
			{
				return _Battery;
			}
			set
			{
				if (_Battery != value)
				{
					_Battery = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public bool mPathBlocked
		{
			get
			{
				return _PathBlocked;
			}
			set
			{
				if (_PathBlocked != value)
				{
					_PathBlocked = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mAlarmMessage
		{
			get
			{
				return _AlarmMessage;
			}
			set
			{
				if (_AlarmMessage != value)
				{
					_AlarmMessage = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public int mSafetyFrameRadius
		{
			get
			{
				return _SafetyFrameRadius;
			}
			set
			{
				if (_SafetyFrameRadius != value)
				{
					_SafetyFrameRadius = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public bool mIsInterveneAvailable
		{
			get
			{
				return _IsInterveneAvailable;
			}
			set
			{
				if (_IsInterveneAvailable != value)
				{
					_IsInterveneAvailable = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public bool mIsIntervening
		{
			get
			{
				return _IsIntervening;
			}
			set
			{
				if (_IsIntervening != value)
				{
					_IsIntervening = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mInterveneCommand
		{
			get
			{
				return _InterveneCommand;
			}
			set
			{
				if (_InterveneCommand != value)
				{
					_InterveneCommand = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public IEnumerable<IPoint2D> mPath
		{
			get
			{
				return _Path;
			}
			private set
			{
				if (value != null)
				{
					_Path = value;
				}
			}
		}

		private string _Name = string.Empty;
		private string _State = string.Empty;
		private IPoint2D _Position = null;
		private double _Toward = 0.0f;
		private string _Target = string.Empty;
		private string _BufferTarget = string.Empty;
		private double _TranslationVelocity = 700.0f;
		private double _RotationVeloctiy = 700.0f;
		private double _MapMatch = 0.0f;
		private double _Battery = 73.1f;
		private bool _PathBlocked = false;
		private string _AlarmMessage = string.Empty;
		private int _SafetyFrameRadius = 500;
		private bool _IsInterveneAvailable = true;
		private bool _IsIntervening = false;
		private string _InterveneCommand = string.Empty;
		private IEnumerable<IPoint2D> _Path = null;

		public VehicleSimulatorInfo(string Name)
		{
			Set(Name);
			_Position = GenerateIPoint2D(0, 0);
			_Path = new List<IPoint2D>();
		}
		public void Set(string Name)
		{
			mName = Name;
		}
		public void StartMove(IEnumerable<IPoint2D> Path)
		{
			throw new NotImplementedException();
		}
		public void StopMove()
		{
			throw new NotImplementedException();
		}
		public void PauseMove()
		{
			throw new NotImplementedException();
		}
		public void ResumeMove()
		{
			throw new NotImplementedException();
		}

		protected virtual void RaiseEvent_StateUpdated(bool Sync = true)
		{
			if (Sync)
			{
				StateUpdated?.Invoke(DateTime.Now, mName, this);
			}
			else
			{
				Task.Run(() => StateUpdated?.Invoke(DateTime.Now, mName, this));
			}
		}
	}
}
