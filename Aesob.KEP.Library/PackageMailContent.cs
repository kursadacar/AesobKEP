using Aesob.Web.Library.Email;
using Tr.Com.Eimza.EYazisma;

namespace Aesob.KEP.Library
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

        public bool IsOutgoing { get; set; }
    }
}
