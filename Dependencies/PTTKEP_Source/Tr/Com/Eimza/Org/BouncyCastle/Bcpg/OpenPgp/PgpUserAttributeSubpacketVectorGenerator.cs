using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Attr;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpUserAttributeSubpacketVectorGenerator
	{
		private IList list = Platform.CreateArrayList();

		public virtual void SetImageAttribute(ImageAttrib.Format imageType, byte[] imageData)
		{
			if (imageData == null)
			{
				throw new ArgumentException("attempt to set null image", "imageData");
			}
			list.Add(new ImageAttrib(imageType, imageData));
		}

		public virtual PgpUserAttributeSubpacketVector Generate()
		{
			UserAttributeSubpacket[] array = new UserAttributeSubpacket[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = (UserAttributeSubpacket)list[i];
			}
			return new PgpUserAttributeSubpacketVector(array);
		}
	}
}
