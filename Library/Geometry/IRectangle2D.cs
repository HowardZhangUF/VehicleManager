
namespace Library
{
	public interface IRectangle2D
	{
		/// <summary>最大點。通常為 Rectangle 右上角的點</summary>
		IPoint2D mMaxPoint { get; }
		/// <summary>最小點。通常為 Rectangle 左下角的點</summary>
		IPoint2D mMinPoint { get; }
		/// <summary>X 最大值</summary>
		int mMaxX { get; }
		/// <summary>Y 最大值</summary>
		int mMaxY { get; }
		/// <summary>X 最小值</summary>
		int mMinX { get; }
		/// <summary>Y 最小值</summary>
		int mMinY { get; }
		/// <summary>寬 (X 方向) (MaxX - MinX)</summary>
		int mWidth { get; }
		/// <summary>高 (Y 方向) (MaxY - MinY)</summary>
		int mHeight { get; }

		/// <summary>設定最大點與最小點</summary>
		void Set(IPoint2D MaxPoint, IPoint2D MinPoint);
		/// <summary>將自身放大。以自身中心為基準， X 軸方向增加 OffsetX ， Y 軸方向增加 OffsetY</summary>
		void Amplify(int OffsetX, int OffsetY);
		/// <summary>判斷是否包含特定點</summary>
		bool IsIncludePoint(IPoint2D Point);
		string ToString();
	}
}
