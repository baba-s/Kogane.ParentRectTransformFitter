using UnityEngine;

namespace UniParentRectTransformFitter
{
	/// <summary>
	/// RectTransform 型の拡張メソッドを管理するクラス
	/// </summary>
	internal static class RectTransformExt
	{
		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// 左端の位置を返します
		/// </summary>
		public static float GetAnchoredPositionLeft( this RectTransform self )
		{
			return self.anchoredPosition.x - self.sizeDelta.x * self.pivot.x * self.localScale.x;
		}

		/// <summary>
		/// 右端の位置を返します
		/// </summary>
		public static float GetAnchoredPositionRight( this RectTransform self )
		{
			return self.anchoredPosition.x + self.sizeDelta.x * ( 1 - self.pivot.x ) * self.localScale.x;
		}

		/// <summary>
		/// 下端の位置を返します
		/// </summary>
		public static float GetAnchoredPositionBottom( this RectTransform self )
		{
			return self.anchoredPosition.y - self.sizeDelta.y * self.pivot.y * self.localScale.y;
		}

		/// <summary>
		/// 上端の位置を返します
		/// </summary>
		public static float GetAnchoredPositionTop( this RectTransform self )
		{
			return self.anchoredPosition.y + self.sizeDelta.y * ( 1 - self.pivot.y ) * self.localScale.y;
		}

		/// <summary>
		/// 端を表す矩形を返します
		/// </summary>
		public static Rect GetAnchoredEdge( this RectTransform self )
		{
			var rect = new Rect
			{
				xMin = self.GetAnchoredPositionLeft(),
				xMax = self.GetAnchoredPositionRight(),
				yMin = self.GetAnchoredPositionBottom(),
				yMax = self.GetAnchoredPositionTop(),
			};

			return rect;
		}
	}
}