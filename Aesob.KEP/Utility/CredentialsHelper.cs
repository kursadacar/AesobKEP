using Aesob.KEP.Model;

namespace Aesob.KEP.Utility
{
    internal static class CredentialsHelper
    {
        internal static KepCredentials GetCredentials()
        {
            var creds = new KepCredentials();
            //Fatih
            // TC: 33727133874
            // Kep Şifre: Aesob123546.
            // Kep Parola: 357916284

            //Adlıhan
            // TC: 15793515860

            //Emine
            // TC: 15220537410
            // Kep Şifre: mPwUA2
            // Kep Parola: 465728

            creds.SetAccountName("antalyaesnafvesanatkarlarodalaribirligimeslekisinavmerkezi@hs01.kep.tr");
            creds.SetId("33727133874");
            creds.SetPassword("357916284");
            creds.SetPassCode("Aesob123456");

            creds.SetPIN("123456");

            return creds;
        }
    }
}
