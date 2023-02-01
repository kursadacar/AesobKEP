namespace Tr.Com.Eimza.EYazisma.Config
{
	public class Boyut
	{
		public enum Suffix
		{
			KB,
			MB
		}

		private Suffix _cins;

		private float _size;

		public float Size
		{
			get
			{
				return _size;
			}
			set
			{
				_size = value;
			}
		}

		public Suffix Cins
		{
			get
			{
				return _cins;
			}
			set
			{
				_cins = value;
			}
		}

		public static Boyut Default
		{
			get
			{
				return new Boyut();
			}
		}

		public Boyut(Suffix cins = Suffix.MB, float size = 20f)
		{
			_cins = cins;
			_size = size;
		}
	}
}
