using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf
{
	internal abstract class CommitmentTypeIdentifier
	{
		public static readonly DerObjectIdentifier ProofOfOrigin = PkcsObjectIdentifiers.IdCtiEtsProofOfOrigin;

		public static readonly DerObjectIdentifier ProofOfReceipt = PkcsObjectIdentifiers.IdCtiEtsProofOfReceipt;

		public static readonly DerObjectIdentifier ProofOfDelivery = PkcsObjectIdentifiers.IdCtiEtsProofOfDelivery;

		public static readonly DerObjectIdentifier ProofOfSender = PkcsObjectIdentifiers.IdCtiEtsProofOfSender;

		public static readonly DerObjectIdentifier ProofOfApproval = PkcsObjectIdentifiers.IdCtiEtsProofOfApproval;

		public static readonly DerObjectIdentifier ProofOfCreation = PkcsObjectIdentifiers.IdCtiEtsProofOfCreation;
	}
}
