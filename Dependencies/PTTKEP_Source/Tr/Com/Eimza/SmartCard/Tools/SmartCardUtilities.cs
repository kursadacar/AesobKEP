using System;
using System.Collections.Generic;
using System.Linq;
using Tr.Com.Eimza.Pkcs11.H;

namespace Tr.Com.Eimza.SmartCard.Tools
{
	public static class SmartCardUtilities
	{
		internal static SmartCardType GetSmartCardType(string terminalName)
		{
			SmartCardType.GetAllAtrHashesFromFile();
			return SmartCardType.GetCardTypeFromATR(GetAtrHashFromSmartCard(terminalName));
		}

		public static string GetAtrHashFromSmartCard(string terminalName)
		{
			byte[] atr = WinsCard.GetAtr(terminalName);
			return BitConverter.ToString(atr, 0, atr.Length).Replace("-", string.Empty);
		}

		internal static int GetSlotIndex(SmartCardType aCardType, string aTerminalName)
		{
			foreach (Slot slot in new Tr.Com.Eimza.Pkcs11.H.Pkcs11(aCardType.LibraryName, false).GetSlotList(true))
			{
				string text = slot.GetSlotInfo().SlotDescription.Trim();
				if (text.IndexOf(WinsCard.NULL_CHAR) > 0)
				{
					text = text.Substring(0, text.IndexOf(WinsCard.NULL_CHAR));
				}
				if (aTerminalName.Contains(text))
				{
					return Convert.ToInt32(slot.SlotId);
				}
			}
			return -1;
		}

		internal static Slot GetSlot(SmartCardType aCardType, string aTerminalName)
		{
			foreach (Slot slot in new Tr.Com.Eimza.Pkcs11.H.Pkcs11(aCardType.LibraryName, false).GetSlotList(true))
			{
				string text = slot.GetSlotInfo().SlotDescription.Trim();
				if (text.IndexOf(WinsCard.NULL_CHAR) > 0)
				{
					text = text.Substring(0, text.IndexOf(WinsCard.NULL_CHAR));
				}
				if (aTerminalName.Contains(text))
				{
					return slot;
				}
			}
			return null;
		}

		public static List<string> GetTerminalNames()
		{
			return WinsCard.GetTerminalList().ToList();
		}
	}
}
