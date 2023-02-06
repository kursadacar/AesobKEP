using Tr.Com.Eimza.EYazisma;

namespace Aesob.KEP.Services
{
    public class PackageMailContent
    {
        public List<MailAttachment> Attachments { get; set; }

        public MailAttachment ImzaP7s { get; set; }

        public string KepSıraNo { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string From { get; set; }

        public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public List<string> Bcc { get; set; }

        public EYazismaPaketTur MailType { get; set; }

        public string MailTypeId { get; set; }
    }

    public class MailAttachment
    {
        public string Name { get; set; }
        public byte[] Value { get; set; }

        public static MailAttachment FromEk(Ek ek)
        {
            return new MailAttachment()
            {
                Name = ek.Adi,
                Value = ek.Degeri
            };
        }

        public static List<MailAttachment> FromMultipleEk(List<Ek> eks)
        {
            var result = new List<MailAttachment>();

            foreach (var ek in eks)
            {
                result.Add(MailAttachment.FromEk(ek));
            }

            return result;
        }
    }
}
