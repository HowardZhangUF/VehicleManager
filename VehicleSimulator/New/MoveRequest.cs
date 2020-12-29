using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator.New
{
	public class MoveRequest
	{
		public int mX { get; private set; }
		public int mY { get; private set; }
		public int mToward { get; private set; }
		public bool mIsMoveBackward { get; private set; } // 是否使用後退的方式進去
		public bool mIsRequestToward { get; private set; } // 目標點是否有要求方向角

		public MoveRequest(int X, int Y, bool IsMoveBackward = false)
		{
			Set(X, Y, IsMoveBackward);
		}
		public MoveRequest(int X, int Y, int Toward, bool IsMoveBackward = false)
		{
			Set(X, Y, Toward, IsMoveBackward);
		}
		public void Set(int X, int Y, bool IsMoveBackward = false)
		{
			mX = X;
			mY = Y;
			mIsMoveBackward = IsMoveBackward;
			mIsRequestToward = false;
		}
		public void Set(int X, int Y, int Toward, bool IsMoveBackward = false)
		{
			mX = X;
			mY = Y;
			mToward = Toward;
			mIsMoveBackward = IsMoveBackward;
			mIsRequestToward = true;
		}
	}
}
