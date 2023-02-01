using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsServerProtocol : TlsProtocol
	{
		protected TlsServer mTlsServer;

		internal TlsServerContextImpl mTlsServerContext;

		protected TlsKeyExchange mKeyExchange;

		protected TlsCredentials mServerCredentials;

		protected CertificateRequest mCertificateRequest;

		protected short mClientCertificateType = -1;

		protected TlsHandshakeHash mPrepareFinishHash;

		protected override TlsContext Context
		{
			get
			{
				return mTlsServerContext;
			}
		}

		internal override AbstractTlsContext ContextAdmin
		{
			get
			{
				return mTlsServerContext;
			}
		}

		protected override TlsPeer Peer
		{
			get
			{
				return mTlsServer;
			}
		}

		public TlsServerProtocol(Stream stream, SecureRandom secureRandom)
			: base(stream, secureRandom)
		{
		}

		public TlsServerProtocol(Stream input, Stream output, SecureRandom secureRandom)
			: base(input, output, secureRandom)
		{
		}

		public virtual void Accept(TlsServer tlsServer)
		{
			if (tlsServer == null)
			{
				throw new ArgumentNullException("tlsServer");
			}
			if (mTlsServer != null)
			{
				throw new InvalidOperationException("'Accept' can only be called once");
			}
			mTlsServer = tlsServer;
			mSecurityParameters = new SecurityParameters();
			mSecurityParameters.entity = 0;
			mTlsServerContext = new TlsServerContextImpl(mSecureRandom, mSecurityParameters);
			mSecurityParameters.serverRandom = TlsProtocol.CreateRandomBlock(tlsServer.ShouldUseGmtUnixTime(), mTlsServerContext.NonceRandomGenerator);
			mTlsServer.Init(mTlsServerContext);
			mRecordStream.Init(mTlsServerContext);
			mRecordStream.SetRestrictReadVersion(false);
			CompleteHandshake();
		}

		protected override void CleanupHandshake()
		{
			base.CleanupHandshake();
			mKeyExchange = null;
			mServerCredentials = null;
			mCertificateRequest = null;
			mPrepareFinishHash = null;
		}

		protected override void HandleHandshakeMessage(byte type, byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream(data);
			switch (type)
			{
			case 1:
				if (mConnectionState == 0)
				{
					ReceiveClientHelloMessage(memoryStream);
					mConnectionState = 1;
					SendServerHelloMessage();
					mConnectionState = 2;
					IList serverSupplementalData = mTlsServer.GetServerSupplementalData();
					if (serverSupplementalData != null)
					{
						SendSupplementalDataMessage(serverSupplementalData);
					}
					mConnectionState = 3;
					mKeyExchange = mTlsServer.GetKeyExchange();
					mKeyExchange.Init(Context);
					mServerCredentials = mTlsServer.GetCredentials();
					Certificate certificate = null;
					if (mServerCredentials == null)
					{
						mKeyExchange.SkipServerCredentials();
					}
					else
					{
						mKeyExchange.ProcessServerCredentials(mServerCredentials);
						certificate = mServerCredentials.Certificate;
						SendCertificateMessage(certificate);
					}
					mConnectionState = 4;
					if (certificate == null || certificate.IsEmpty)
					{
						mAllowCertificateStatus = false;
					}
					if (mAllowCertificateStatus)
					{
						CertificateStatus certificateStatus = mTlsServer.GetCertificateStatus();
						if (certificateStatus != null)
						{
							SendCertificateStatusMessage(certificateStatus);
						}
					}
					mConnectionState = 5;
					byte[] array = mKeyExchange.GenerateServerKeyExchange();
					if (array != null)
					{
						SendServerKeyExchangeMessage(array);
					}
					mConnectionState = 6;
					if (mServerCredentials != null)
					{
						mCertificateRequest = mTlsServer.GetCertificateRequest();
						if (mCertificateRequest != null)
						{
							mKeyExchange.ValidateCertificateRequest(mCertificateRequest);
							SendCertificateRequestMessage(mCertificateRequest);
							TlsUtilities.TrackHashAlgorithms(mRecordStream.HandshakeHash, mCertificateRequest.SupportedSignatureAlgorithms);
						}
					}
					mConnectionState = 7;
					SendServerHelloDoneMessage();
					mConnectionState = 8;
					mRecordStream.HandshakeHash.SealHashAlgorithms();
					break;
				}
				throw new TlsFatalAlert(10);
			case 23:
				if (mConnectionState == 8)
				{
					mTlsServer.ProcessClientSupplementalData(TlsProtocol.ReadSupplementalDataMessage(memoryStream));
					mConnectionState = 9;
					break;
				}
				throw new TlsFatalAlert(10);
			case 11:
			{
				short num = mConnectionState;
				if ((uint)(num - 8) <= 1u)
				{
					if (mConnectionState < 9)
					{
						mTlsServer.ProcessClientSupplementalData(null);
					}
					if (mCertificateRequest == null)
					{
						throw new TlsFatalAlert(10);
					}
					ReceiveCertificateMessage(memoryStream);
					mConnectionState = 10;
					break;
				}
				throw new TlsFatalAlert(10);
			}
			case 16:
			{
				short num = mConnectionState;
				if ((uint)(num - 8) <= 2u)
				{
					if (mConnectionState < 9)
					{
						mTlsServer.ProcessClientSupplementalData(null);
					}
					if (mConnectionState < 10)
					{
						if (mCertificateRequest == null)
						{
							mKeyExchange.SkipClientCredentials();
						}
						else
						{
							if (TlsUtilities.IsTlsV12(Context))
							{
								throw new TlsFatalAlert(10);
							}
							if (TlsUtilities.IsSsl(Context))
							{
								if (mPeerCertificate == null)
								{
									throw new TlsFatalAlert(10);
								}
							}
							else
							{
								NotifyClientCertificate(Certificate.EmptyChain);
							}
						}
					}
					ReceiveClientKeyExchangeMessage(memoryStream);
					mConnectionState = 11;
					break;
				}
				throw new TlsFatalAlert(10);
			}
			case 15:
				if (mConnectionState == 11)
				{
					if (!ExpectCertificateVerifyMessage())
					{
						throw new TlsFatalAlert(10);
					}
					ReceiveCertificateVerifyMessage(memoryStream);
					mConnectionState = 12;
					break;
				}
				throw new TlsFatalAlert(10);
			case 20:
			{
				short num = mConnectionState;
				if ((uint)(num - 11) <= 1u)
				{
					if (mConnectionState < 12 && ExpectCertificateVerifyMessage())
					{
						throw new TlsFatalAlert(10);
					}
					ProcessFinishedMessage(memoryStream);
					mConnectionState = 13;
					if (mExpectSessionTicket)
					{
						SendNewSessionTicketMessage(mTlsServer.GetNewSessionTicket());
						SendChangeCipherSpecMessage();
					}
					mConnectionState = 14;
					SendFinishedMessage();
					mConnectionState = 15;
					mConnectionState = 16;
					break;
				}
				throw new TlsFatalAlert(10);
			}
			default:
				throw new TlsFatalAlert(10);
			}
		}

		protected override void HandleWarningMessage(byte description)
		{
			if (description == 41)
			{
				if (TlsUtilities.IsSsl(Context) && mCertificateRequest != null)
				{
					NotifyClientCertificate(Certificate.EmptyChain);
				}
			}
			else
			{
				base.HandleWarningMessage(description);
			}
		}

		protected virtual void NotifyClientCertificate(Certificate clientCertificate)
		{
			if (mCertificateRequest == null)
			{
				throw new InvalidOperationException();
			}
			if (mPeerCertificate != null)
			{
				throw new TlsFatalAlert(10);
			}
			mPeerCertificate = clientCertificate;
			if (clientCertificate.IsEmpty)
			{
				mKeyExchange.SkipClientCredentials();
			}
			else
			{
				mClientCertificateType = TlsUtilities.GetClientCertificateType(clientCertificate, mServerCredentials.Certificate);
				mKeyExchange.ProcessClientCertificate(clientCertificate);
			}
			mTlsServer.NotifyClientCertificate(clientCertificate);
		}

		protected virtual void ReceiveCertificateMessage(MemoryStream buf)
		{
			Certificate clientCertificate = Certificate.Parse(buf);
			TlsProtocol.AssertEmpty(buf);
			NotifyClientCertificate(clientCertificate);
		}

		protected virtual void ReceiveCertificateVerifyMessage(MemoryStream buf)
		{
			DigitallySigned digitallySigned = DigitallySigned.Parse(Context, buf);
			TlsProtocol.AssertEmpty(buf);
			try
			{
				byte[] hash = ((!TlsUtilities.IsTlsV12(Context)) ? mSecurityParameters.SessionHash : mPrepareFinishHash.GetFinalHash(digitallySigned.Algorithm.Hash));
				AsymmetricKeyParameter publicKey = PublicKeyFactory.CreateKey(mPeerCertificate.GetCertificateAt(0).SubjectPublicKeyInfo);
				TlsSigner tlsSigner = TlsUtilities.CreateTlsSigner((byte)mClientCertificateType);
				tlsSigner.Init(Context);
				if (!tlsSigner.VerifyRawSignature(digitallySigned.Algorithm, digitallySigned.Signature, publicKey, hash))
				{
					throw new TlsFatalAlert(51);
				}
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(51, alertCause);
			}
		}

		protected virtual void ReceiveClientHelloMessage(MemoryStream buf)
		{
			ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(buf);
			if (protocolVersion.IsDtls)
			{
				throw new TlsFatalAlert(47);
			}
			byte[] clientRandom = TlsUtilities.ReadFully(32, buf);
			if (TlsUtilities.ReadOpaque8(buf).Length > 32)
			{
				throw new TlsFatalAlert(47);
			}
			int num = TlsUtilities.ReadUint16(buf);
			if (num < 2 || ((uint)num & (true ? 1u : 0u)) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			mOfferedCipherSuites = TlsUtilities.ReadUint16Array(num / 2, buf);
			int num2 = TlsUtilities.ReadUint8(buf);
			if (num2 < 1)
			{
				throw new TlsFatalAlert(47);
			}
			mOfferedCompressionMethods = TlsUtilities.ReadUint8Array(num2, buf);
			mClientExtensions = TlsProtocol.ReadExtensions(buf);
			mSecurityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension(mClientExtensions);
			ContextAdmin.SetClientVersion(protocolVersion);
			mTlsServer.NotifyClientVersion(protocolVersion);
			mSecurityParameters.clientRandom = clientRandom;
			mTlsServer.NotifyOfferedCipherSuites(mOfferedCipherSuites);
			mTlsServer.NotifyOfferedCompressionMethods(mOfferedCompressionMethods);
			if (Arrays.Contains(mOfferedCipherSuites, 255))
			{
				mSecureRenegotiation = true;
			}
			byte[] extensionData = TlsUtilities.GetExtensionData(mClientExtensions, 65281);
			if (extensionData != null)
			{
				mSecureRenegotiation = true;
				if (!Arrays.ConstantTimeAreEqual(extensionData, TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes)))
				{
					throw new TlsFatalAlert(40);
				}
			}
			mTlsServer.NotifySecureRenegotiation(mSecureRenegotiation);
			if (mClientExtensions != null)
			{
				mTlsServer.ProcessClientExtensions(mClientExtensions);
			}
		}

		protected virtual void ReceiveClientKeyExchangeMessage(MemoryStream buf)
		{
			mKeyExchange.ProcessClientKeyExchange(buf);
			TlsProtocol.AssertEmpty(buf);
			mPrepareFinishHash = mRecordStream.PrepareToFinish();
			mSecurityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash(Context, mPrepareFinishHash, null);
			TlsProtocol.EstablishMasterSecret(Context, mKeyExchange);
			mRecordStream.SetPendingConnectionState(Peer.GetCompression(), Peer.GetCipher());
			if (!mExpectSessionTicket)
			{
				SendChangeCipherSpecMessage();
			}
		}

		protected virtual void SendCertificateRequestMessage(CertificateRequest certificateRequest)
		{
			HandshakeMessage handshakeMessage = new HandshakeMessage(13);
			certificateRequest.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		protected virtual void SendCertificateStatusMessage(CertificateStatus certificateStatus)
		{
			HandshakeMessage handshakeMessage = new HandshakeMessage(22);
			certificateStatus.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		protected virtual void SendNewSessionTicketMessage(NewSessionTicket newSessionTicket)
		{
			if (newSessionTicket == null)
			{
				throw new TlsFatalAlert(80);
			}
			HandshakeMessage handshakeMessage = new HandshakeMessage(4);
			newSessionTicket.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		protected virtual void SendServerHelloMessage()
		{
			HandshakeMessage handshakeMessage = new HandshakeMessage(2);
			ProtocolVersion serverVersion = mTlsServer.GetServerVersion();
			if (!serverVersion.IsEqualOrEarlierVersionOf(Context.ClientVersion))
			{
				throw new TlsFatalAlert(80);
			}
			mRecordStream.ReadVersion = serverVersion;
			mRecordStream.SetWriteVersion(serverVersion);
			mRecordStream.SetRestrictReadVersion(true);
			ContextAdmin.SetServerVersion(serverVersion);
			TlsUtilities.WriteVersion(serverVersion, handshakeMessage);
			handshakeMessage.Write(mSecurityParameters.serverRandom);
			TlsUtilities.WriteOpaque8(TlsUtilities.EmptyBytes, handshakeMessage);
			int selectedCipherSuite = mTlsServer.GetSelectedCipherSuite();
			if (!Arrays.Contains(mOfferedCipherSuites, selectedCipherSuite) || selectedCipherSuite == 0 || selectedCipherSuite == 255 || !TlsUtilities.IsValidCipherSuiteForVersion(selectedCipherSuite, serverVersion))
			{
				throw new TlsFatalAlert(80);
			}
			mSecurityParameters.cipherSuite = selectedCipherSuite;
			byte selectedCompressionMethod = mTlsServer.GetSelectedCompressionMethod();
			if (!Arrays.Contains(mOfferedCompressionMethods, selectedCompressionMethod))
			{
				throw new TlsFatalAlert(80);
			}
			mSecurityParameters.compressionAlgorithm = selectedCompressionMethod;
			TlsUtilities.WriteUint16(selectedCipherSuite, handshakeMessage);
			TlsUtilities.WriteUint8(selectedCompressionMethod, handshakeMessage);
			mServerExtensions = mTlsServer.GetServerExtensions();
			if (mSecureRenegotiation)
			{
				byte[] extensionData = TlsUtilities.GetExtensionData(mServerExtensions, 65281);
				if (extensionData == null)
				{
					mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(mServerExtensions);
					mServerExtensions[65281] = TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes);
				}
			}
			if (mSecurityParameters.extendedMasterSecret)
			{
				mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(mServerExtensions);
				TlsExtensionsUtilities.AddExtendedMasterSecretExtension(mServerExtensions);
			}
			if (mServerExtensions != null)
			{
				mSecurityParameters.encryptThenMac = TlsExtensionsUtilities.HasEncryptThenMacExtension(mServerExtensions);
				mSecurityParameters.maxFragmentLength = ProcessMaxFragmentLengthExtension(mClientExtensions, mServerExtensions, 80);
				mSecurityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension(mServerExtensions);
				mAllowCertificateStatus = !mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(mServerExtensions, 5, 80);
				mExpectSessionTicket = !mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(mServerExtensions, 35, 80);
				TlsProtocol.WriteExtensions(handshakeMessage, mServerExtensions);
			}
			if (mSecurityParameters.maxFragmentLength >= 0)
			{
				int plaintextLimit = 1 << 8 + mSecurityParameters.maxFragmentLength;
				mRecordStream.SetPlaintextLimit(plaintextLimit);
			}
			mSecurityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm(Context, mSecurityParameters.CipherSuite);
			mSecurityParameters.verifyDataLength = 12;
			handshakeMessage.WriteToRecordStream(this);
			mRecordStream.NotifyHelloComplete();
		}

		protected virtual void SendServerHelloDoneMessage()
		{
			byte[] array = new byte[4];
			TlsUtilities.WriteUint8(14, array, 0);
			TlsUtilities.WriteUint24(0, array, 1);
			WriteHandshakeMessage(array, 0, array.Length);
		}

		protected virtual void SendServerKeyExchangeMessage(byte[] serverKeyExchange)
		{
			HandshakeMessage handshakeMessage = new HandshakeMessage(12, serverKeyExchange.Length);
			handshakeMessage.Write(serverKeyExchange);
			handshakeMessage.WriteToRecordStream(this);
		}

		protected virtual bool ExpectCertificateVerifyMessage()
		{
			if (mClientCertificateType >= 0)
			{
				return TlsUtilities.HasSigningCapability((byte)mClientCertificateType);
			}
			return false;
		}
	}
}
