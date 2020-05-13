
namespace TrafficControlTest.Module.General
{
	public interface IRectangle2D
	{
		/// <summary>最大點。通常為 Rectangle 右上角的點</summary>
		IPoint2D mMaxPoint { get; set; }
		/// <summary>最小點。通常為 Rectangle 左下角的點</summary>
		IPoint2D mMinPoint { get; set; }
		int mMaxX { get; }
		int mMaxY { get; }
		int mMinX { get; }
		int mMinY { get; }
		int mWidth { get; }
		int mHeight { get; }

		void Set(IPoint2D MaxPoint, IPoint2D MinPoint);
		/// <summary>將自身放大。以自身中心為基準， X 軸方向增加 OffsetX ， Y 軸方向增加 OffsetY</summary>
		void Amplify(int OffsetX, int OffsetY);
		/// <summary>判斷是否包含特定點</summary>
		bool IsIncludePoint(IPoint2D Point);
		string ToString();
	}
}
