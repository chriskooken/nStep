using System;

namespace nStep.Framework.WorldViews
{
	public interface IWorldViewDictionary : System.Collections.Generic.IDictionary<Type,Object>
	{
		void Import(IProvideWorldView worldViewProvider);
		T GetWorldViewOfType<T>() where T : IAmWorldView;
	}
}