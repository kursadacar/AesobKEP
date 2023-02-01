using System;

namespace ActiveUp.Net.Mail
{
	public interface IMailbox
	{
		IMailbox CreateChild(string mailboxName);

		IAsyncResult BeginCreateChild(string mailboxName, AsyncCallback callback);

		IMailbox EndCreateChild(IAsyncResult result);

		string Subscribe();

		IAsyncResult BeginSubscribe(AsyncCallback callback);

		string EndSubscribe(IAsyncResult result);

		string Unsubscribe();

		IAsyncResult BeginUnsubscribe(AsyncCallback callback);

		string EndUnsubscribe(IAsyncResult result);

		string Delete();

		IAsyncResult BeginDelete(AsyncCallback callback);

		string EndDelete(IAsyncResult result);

		string Rename(string newMailboxName);

		IAsyncResult BeginRename(string newMailboxName, AsyncCallback callback);

		string EndRename(IAsyncResult result);

		int[] Search(string query);

		IAsyncResult BeginSearch(string query, AsyncCallback callback);

		MessageCollection SearchParse(string query);

		IAsyncResult BeginSearchParse(string query, AsyncCallback callback);

		int[] Search(string charset, string query);

		IAsyncResult BeginSearch(string charset, string query, AsyncCallback callback);

		string EndSearch(IAsyncResult result);

		MessageCollection SearchParse(string charset, string query);

		IAsyncResult BeginSearchParse(string charset, string query, AsyncCallback callback);

		string EndSearchParse(IAsyncResult result);

		string AddFlags(int messageOrdinal, IFlagCollection flags);

		IAsyncResult BeginAddFlags(int messageOrdinal, IFlagCollection flags, AsyncCallback callback);

		string EndAddFlags(IAsyncResult result);

		string UidAddFlags(int uid, IFlagCollection flags);

		IAsyncResult BeginUidAddFlags(int uid, IFlagCollection flags, AsyncCallback callback);

		string EndUidAddFlags(IAsyncResult result);

		string RemoveFlags(int messageOrdinal, IFlagCollection flags);

		IAsyncResult BeginRemoveFlags(int messageOrdinal, IFlagCollection flags, AsyncCallback callback);

		string EndRemoveFlags(IAsyncResult result);

		string UidRemoveFlags(int uid, IFlagCollection flags);

		IAsyncResult BeginUidRemoveFlags(int uid, IFlagCollection flags, AsyncCallback callback);

		string EndUidRemoveFlags(IAsyncResult result);

		string SetFlags(int messageOrdinal, IFlagCollection flags);

		IAsyncResult BeginSetFlags(int messageOrdinal, IFlagCollection flags, AsyncCallback callback);

		string EndSetFlags(IAsyncResult result);

		string UidSetFlags(int uid, IFlagCollection flags);

		IAsyncResult BeginUidSetFlags(int uid, IFlagCollection flags, AsyncCallback callback);

		string EndUidSetFlags(IAsyncResult result);

		void AddFlagsSilent(int messageOrdinal, IFlagCollection flags);

		IAsyncResult BeginAddFlagsSilent(int messageOrdinal, IFlagCollection flags, AsyncCallback callback);

		void EndAddFlagsSilent(IAsyncResult result);

		void UidAddFlagsSilent(int uid, IFlagCollection flags);

		IAsyncResult BeginUidAddFlagsSilent(int uid, IFlagCollection flags, AsyncCallback callback);

		void EndUidAddFlagsSilent(IAsyncResult result);

		void RemoveFlagsSilent(int messageOrdinal, IFlagCollection flags);

		IAsyncResult BeginRemoveFlagsSilent(int messageOrdinal, IFlagCollection flags, AsyncCallback callback);

		void EndRemoveFlagsSilent(IAsyncResult result);

		void UidRemoveFlagsSilent(int uid, IFlagCollection flags);

		IAsyncResult BeginUidRemoveFlagsSilent(int uid, IFlagCollection flags, AsyncCallback callback);

		void EndUidRemoveFlagsSilent(IAsyncResult result);

		void SetFlagsSilent(int messageOrdinal, IFlagCollection flags);

		IAsyncResult BeginSetFlagsSilent(int messageOrdinal, IFlagCollection flags, AsyncCallback callback);

		void EndSetFlagsSilent(IAsyncResult result);

		void UidSetFlagsSilent(int uid, IFlagCollection flags);

		IAsyncResult BeginUidSetFlagsSilent(int uid, IFlagCollection flags, AsyncCallback callback);

		void EndUidSetFlagsSilent(IAsyncResult result);

		void CopyMessage(int messageOrdinal, string destinationMailboxName);

		IAsyncResult BeginCopyMessage(int messageOrdinal, string destinationMailboxName, AsyncCallback callback);

		void EndCopyMessage(IAsyncResult result);

		void UidCopyMessage(int uid, string destinationMailboxName);

		IAsyncResult BeginUidCopyMessage(int uid, string destinationMailboxName, AsyncCallback callback);

		void EndUidCopyMessage(IAsyncResult result);

		void MoveMessage(int messageOrdinal, string destinationMailboxName);

		IAsyncResult BeginMoveMessage(int messageOrdinal, string destinationMailboxName, AsyncCallback callback);

		void EndMoveMessage(IAsyncResult result);

		void UidMoveMessage(int uid, string destinationMailboxName);

		IAsyncResult BeginUidMoveMessage(int uid, string destinationMailboxName, AsyncCallback callback);

		void EndUidMoveMessage(IAsyncResult result);

		string Append(string messageLiteral);

		IAsyncResult BeginAppend(string messageLiteral, AsyncCallback callback);

		string Append(string messageLiteral, IFlagCollection flags);

		IAsyncResult BeginAppend(string messageLiteral, IFlagCollection flags, AsyncCallback callback);

		string Append(string messageLiteral, IFlagCollection flags, DateTime dateTime);

		IAsyncResult BeginAppend(string messageLiteral, IFlagCollection flags, DateTime dateTime, AsyncCallback callback);

		string Append(Message message);

		IAsyncResult BeginAppend(Message message, AsyncCallback callback);

		string Append(Message message, IFlagCollection flags);

		IAsyncResult BeginAppend(Message message, IFlagCollection flags, AsyncCallback callback);

		string Append(Message message, IFlagCollection flags, DateTime dateTime);

		IAsyncResult BeginAppend(Message message, IFlagCollection flags, DateTime dateTime, AsyncCallback callback);

		string Append(byte[] messageData);

		IAsyncResult BeginAppend(byte[] messageData, AsyncCallback callback);

		string Append(byte[] messageData, IFlagCollection flags);

		IAsyncResult BeginAppend(byte[] messageData, IFlagCollection flags, AsyncCallback callback);

		string Append(byte[] messageData, IFlagCollection flags, DateTime dateTime);

		IAsyncResult BeginAppend(byte[] messageData, IFlagCollection flags, DateTime dateTime, AsyncCallback callback);

		string EndAppend(IAsyncResult result);

		void Empty(bool expunge);

		IAsyncResult BeginEmpty(bool expunge, AsyncCallback callback);

		void EndEmpty(IAsyncResult result);

		void DeleteMessage(int messageOrdinal, bool expunge);

		IAsyncResult BeginDeleteMessage(int messageOrdinal, bool expunge, AsyncCallback callback);

		void EndDeleteMessage(IAsyncResult result);

		void UidDeleteMessage(int uid, bool expunge);

		IAsyncResult BeginUidDeleteMessage(int uid, bool expunge, AsyncCallback callback);

		void EndUidDeleteMessage(IAsyncResult result);
	}
}
