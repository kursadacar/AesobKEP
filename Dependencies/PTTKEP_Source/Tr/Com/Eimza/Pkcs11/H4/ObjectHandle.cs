namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class ObjectHandle
	{
		private uint _objectId;

		public uint ObjectId
		{
			get
			{
				return _objectId;
			}
		}

		public ObjectHandle()
		{
			_objectId = 0u;
		}

		public ObjectHandle(uint objectId)
		{
			_objectId = objectId;
		}
	}
}
