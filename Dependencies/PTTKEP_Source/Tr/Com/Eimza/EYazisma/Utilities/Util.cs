using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma.Utilities
{
	public static class Util
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static CultureInfo TR = new CultureInfo("tr-TR", false);

		public static CultureInfo EN = new CultureInfo("en-US", false);

		public static readonly int TURKISH_ENCODING = 1254;

		public static void ChangePermissions(string Folder)
		{
			FileSystemAccessRule rule;
			try
			{
				rule = new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
			}
			catch (Exception)
			{
				rule = new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
			}
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(Folder);
				DirectorySecurity accessControl = directoryInfo.GetAccessControl();
				accessControl.ResetAccessRule(rule);
				accessControl.AddAccessRule(rule);
				directoryInfo.SetAccessControl(accessControl);
			}
			catch (Exception exception)
			{
				LOG.Info("Log Dosyasının Yazılacağı Klasörün Yetkilerini Kontrol Ediniz.Eğer Kullanıcının Logun Tutulduğu Klasör'e Yazma Yetkisi Varsa Bu Uyarıyı Dikkate Almayınız.", exception);
			}
		}
	}
}
