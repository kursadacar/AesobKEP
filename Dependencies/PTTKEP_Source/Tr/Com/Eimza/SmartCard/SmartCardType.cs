using System;
using System.Collections.Generic;
using System.Reflection;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.SmartCard.SmartCardTemplates;

namespace Tr.Com.Eimza.SmartCard
{
	internal class SmartCardType
	{
		internal ICardTemplate _template;

		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static Dictionary<string, List<string>> CardATRs = new Dictionary<string, List<string>>();

		internal static Dictionary<string, SmartCardType> Cards = new Dictionary<string, SmartCardType>();

		internal static SmartCardType ALADDIN = new SmartCardType("eTPKCS11.dll", "ALADDIN", typeof(AladdinTemplate));

		internal static SmartCardType AEPKEYPER = new SmartCardType("bp201w32HSM.dll", "AEPKEYPER", typeof(AEPKeyperTemplate));

		internal static SmartCardType AKIS = new SmartCardType("akisp11.dll", "AKIS", typeof(AkisTemplate));

		internal static SmartCardType ATIKKG = new SmartCardType("atikKg_jni.dll", "ATIKKG", typeof(AtikkgTemplate));

		internal static SmartCardType ATIKHSM = new SmartCardType(IntPtr.Size.Equals(4) ? "uhsmpkcs11.dll" : "uhsmpkcs11x64.dll", "ATIKHSM", typeof(AtikHsmTemplate));

		internal static SmartCardType CARDOS = new SmartCardType("cmp11.dll", "CARDOS", typeof(CardosTemplate));

		internal static SmartCardType DATAKEY = new SmartCardType("dkck201.dll", "DATAKEY", typeof(DataKeyTemplate));

		internal static SmartCardType GEMPLUS = new SmartCardType("gclib.dll", "GEMPLUS", typeof(GemPlusTemplate));

		internal static SmartCardType KEYCORP = new SmartCardType("spmp1132.dll", "KEYCORP", typeof(KeyCorpTemplate));

		internal static SmartCardType NCIPHER = new SmartCardType(IntPtr.Size.Equals(4) ? "cknfast.dll" : "cknfast-64.dll", "NCIPHER", typeof(NCipherTemplate));

		internal static SmartCardType NETID = new SmartCardType("iidp11.dll", "Net-ID", typeof(NetIDTemplate));

		internal static SmartCardType PKCS11 = new SmartCardType("kpkcs11hash.dll", "PKCS11", typeof(Pkcs11Template));

		internal static SmartCardType SAFESIGN = new SmartCardType("aetpkss1.dll", "SAFESIGN", typeof(SafeSignTemplate));

		internal static SmartCardType SEFIROT = new SmartCardType("pkcs11sfr.dll", "SEFIROT", typeof(SefirotTemplate));

		internal static SmartCardType OMNIKEY = new SmartCardType("siecap11.dll", "OMNIKEY", typeof(OmniKeyTemplate));

		internal static SmartCardType UTIMACO = new SmartCardType("cs2_pkcs11.dll", "UTIMACO", typeof(UtimacoTemplate));

		internal static SmartCardType UTIMACO_R2 = new SmartCardType("cs_pkcs11_R2.dll", "UTIMACO_R2", typeof(UtimacoTemplate));

		internal static SmartCardType UNKNOWN = new SmartCardType("", "UNKNOWN", typeof(DefaultCardTemplate));

		private static readonly bool IsPlatform32Bit = IntPtr.Size.Equals(4);

		internal string LibraryName { get; set; }

		internal string Name { get; set; }

		internal Type TemplateClass { get; set; }

		internal ICardTemplate Template
		{
			get
			{
				return _template;
			}
			set
			{
				_template = value;
			}
		}

		internal static void GetAllAtrHashesFromFile()
		{
			List<CardTypeConfig> list = new SmartCardConfigParser().ReadConfig();
			if (list != null)
			{
				ApplyCardTypeConfig(list);
			}
		}

		private SmartCardType(CardTypeConfig config)
		{
			SetLibName(config.Lib, config.Lib32, config.Lib64);
			Name = config.Name;
			TemplateClass = typeof(DefaultCardTemplate);
			Template = new DefaultCardTemplate();
			try
			{
				Cards.Add(Name, this);
				CardATRs.Add(Name, config.Atrs);
			}
			catch (ArgumentException)
			{
			}
		}

		internal SmartCardType(string libraryName, string name, Type templateClass)
		{
			LibraryName = libraryName;
			Name = name;
			TemplateClass = templateClass;
			Cards.Add(Name, this);
		}

		internal static SmartCardType GetCardType(string aLibName)
		{
			foreach (SmartCardType value in Cards.Values)
			{
				if (value.LibraryName.Equals(aLibName))
				{
					return value;
				}
			}
			return UNKNOWN;
		}

		internal static SmartCardType GetCardTypeFromATR(string aATRHex)
		{
			foreach (SmartCardType value4 in Cards.Values)
			{
				List<string> value;
				CardATRs.TryGetValue(value4.Name, out value);
				if (value != null)
				{
					foreach (string item in value)
					{
						if (item.Equals(aATRHex, StringComparison.OrdinalIgnoreCase))
						{
							return value4;
						}
					}
				}
				string[] array = null;
				try
				{
					array = value4.GetCardTemplate().GetATRHashes().ToArray();
				}
				catch (SystemException exception)
				{
					LOG.Error("Akıllı karta ait default atr değerleri bulunamadı.Kartınız çalışmıyorsa config dosyasına atr değerini ekleyiniz...", exception);
				}
				if (array == null)
				{
					continue;
				}
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].Equals(aATRHex, StringComparison.OrdinalIgnoreCase))
					{
						return value4;
					}
				}
			}
			string value2 = "55454B4145";
			string value3 = "75656B6165";
			if (!aATRHex.Contains(value2) && !aATRHex.Contains(value3))
			{
				return null;
			}
			return AKIS;
		}

		internal ICardTemplate GetCardTemplate()
		{
			if (Template == null)
			{
				try
				{
					ConstructorInfo constructor = TemplateClass.GetConstructor(new Type[0]);
					Template = constructor.Invoke(null) as ICardTemplate;
				}
				catch (Exception)
				{
					try
					{
						ConstructorInfo constructor2 = TemplateClass.GetConstructor(new Type[1] { typeof(SmartCardType) });
						Template = constructor2.Invoke(new object[1] { this }) as ICardTemplate;
					}
					catch (Exception ex)
					{
						throw new SystemException(ex.Message, ex);
					}
				}
			}
			return Template;
		}

		internal static void ApplyCardTypeConfig(List<CardTypeConfig> configList)
		{
			try
			{
				foreach (CardTypeConfig config in configList)
				{
					if (!Cards.ContainsKey(config.Name))
					{
						continue;
					}
					SmartCardType value;
					Cards.TryGetValue(config.Name, out value);
					foreach (string atr in config.Atrs)
					{
						if (!value.GetCardTemplate().GetATRHashes().Contains(atr))
						{
							value.GetCardTemplate().GetATRHashes().Add(atr);
						}
					}
					value.SetLibName(config.Lib, config.Lib32, config.Lib64);
					List<string> value2 = new List<string>();
					if (!CardATRs.TryGetValue(value.Name, out value2))
					{
						CardATRs.Add(value.Name, config.Atrs);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void SetLibName(string aLibName, string aLib32Name, string aLib64Name)
		{
			LibraryName = aLibName;
			if (aLib32Name != null && IsPlatform32Bit)
			{
				LibraryName = aLib32Name;
			}
			if (aLib64Name != null && !IsPlatform32Bit)
			{
				LibraryName = aLib64Name;
			}
		}
	}
}
