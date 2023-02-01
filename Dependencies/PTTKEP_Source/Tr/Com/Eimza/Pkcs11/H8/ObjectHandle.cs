namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class ObjectHandle
	{
		private ulong _objectId;

		public ulong ObjectId
		{
			get
			{
				return _objectId;
			}
		}

		public ObjectHandle()
		{
			_objectId = 0uL;
		}

		public ObjectHandle(ulong objectId)
		{
			_objectId = objectId;
		}
	}
}
