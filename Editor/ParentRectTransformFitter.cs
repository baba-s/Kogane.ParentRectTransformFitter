using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniParentRectTransformFitter
{
	/// <summary>
	/// 親オブジェクトが子オブジェクトをすべて内包するように親オブジェクトの位置とサイズを調整するエディタ拡張
	/// </summary>
	internal static class ParentRectTransformFitter
	{
		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// 親オブジェクトが子オブジェクトをすべて内包するように
		/// 親オブジェクトの位置とサイズを調整します
		/// </summary>
		[MenuItem( "CONTEXT/RectTransform/Fit Children" )]
		private static void FitChildren( MenuCommand command )
		{
			var parent             = ( RectTransform ) command.context;
			var childrenWithParent = parent.GetComponentsInChildren<RectTransform>();

			var children = childrenWithParent
					.Where( x => x != parent )
					.ToArray()
				;

			// 子オブジェクトが存在しない場合は何もしません
			if ( children.Length <= 0 ) return;

			// Undo できるように親と子のすべての RectTransform を登録します
			// 親だけ登録すると Undo が正常に動作しないのですべての子も登録しています
			Undo.RecordObjects( childrenWithParent.OfType<Object>().ToArray(), "Fit" );

			// 一番端の位置
			var xMin = float.MaxValue;
			var xMax = float.MinValue;
			var yMin = float.MaxValue;
			var yMax = float.MinValue;

			// 後で子オブジェクトの親を元に戻すためのキャッシュ
			var cachedParents = new Dictionary<RectTransform, Transform>();

			foreach ( var child in children )
			{
				// 子オブジェクトの端の位置を取得するためには
				// 親オブジェクトと子オブジェクトを同じ階層に移動する必要があるため、
				// 親オブジェクトと子オブジェクトを同じ階層に移動します
				// 後で子オブジェクトの親を元に戻すためキャッシュにも登録しておきます
				cachedParents.Add( child, child.parent );
				child.SetParent( parent.parent );

				// 子オブジェクトの端の位置を取得します
				var childEdge = child.GetAnchoredEdge();

				// 一番端の位置を計算します
				xMin = Mathf.Min( xMin, childEdge.xMin );
				xMax = Mathf.Max( xMax, childEdge.xMax );
				yMin = Mathf.Min( yMin, childEdge.yMin );
				yMax = Mathf.Max( yMax, childEdge.yMax );
			}

			// 一番端の位置を取得できたので
			// 親オブジェクトの位置とサイズを端の位置に合わせます
			var parentPivot = parent.pivot;
			var width       = xMax - xMin;
			var height      = yMax - yMin;

			parent.anchoredPosition = new Vector2
			(
				xMin + width * parentPivot.x,
				yMin + height * parentPivot.y
			);
			parent.sizeDelta = new Vector2( width, height );

			// 子オブジェクトの親を元に戻します
			foreach ( var child in children )
			{
				child.SetParent( cachedParents[ child ] );
			}
		}
	}
}