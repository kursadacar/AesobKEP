namespace Aesob.KEP.Data
{
    internal class KepCredentials
    {
        public string IdNumber { get; private set; }
        public string AccountName { get; private set; }
        public string Password { get; private set; }
        public string PassCode { get; private set; }
        public string PIN { get; private set; }

        internal KepCredentials()
        {
            IdNumber = string.Empty;
            AccountName = string.Empty;
            Password = string.Empty;
            PassCode = string.Empty;
            PIN = string.Empty;
        }

        internal void SetId(string id)
        {
            IdNumber = id;
        }

        internal void SetAccountName(string accountName)
        {
            AccountName = accountName;
        }

        internal void SetPassword(string password)
        {
            Password = password;
        }

        internal void SetPassCode(string passCode)
        {
            PassCode = passCode;
        }

        internal void SetPIN(string pin)
        {
            PIN = pin;
        }
    }
}
