using System.IO;

namespace Tr.Com.Eimza.Log4Net.ObjectRenderer
{
	public interface IObjectRenderer
	{
		void RenderObject(RendererMap rendererMap, object obj, TextWriter writer);
	}
}
