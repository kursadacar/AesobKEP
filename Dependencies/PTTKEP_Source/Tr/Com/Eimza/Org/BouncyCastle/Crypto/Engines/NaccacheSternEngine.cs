using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Engines
{
	internal class NaccacheSternEngine : IAsymmetricBlockCipher
	{
		private bool forEncryption;

		private NaccacheSternKeyParameters key;

		private IList[] lookup;

		private bool debug;

		public string AlgorithmName
		{
			get
			{
				return "NaccacheStern";
			}
		}

		public bool Debug
		{
			set
			{
				debug = value;
			}
		}

		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			key = (NaccacheSternKeyParameters)parameters;
			if (this.forEncryption)
			{
				return;
			}
			if (debug)
			{
				Console.WriteLine("Constructing lookup Array");
			}
			NaccacheSternPrivateKeyParameters naccacheSternPrivateKeyParameters = (NaccacheSternPrivateKeyParameters)key;
			IList smallPrimesList = naccacheSternPrivateKeyParameters.SmallPrimesList;
			lookup = new IList[smallPrimesList.Count];
			for (int i = 0; i < smallPrimesList.Count; i++)
			{
				BigInteger bigInteger = (BigInteger)smallPrimesList[i];
				int intValue = bigInteger.IntValue;
				lookup[i] = Platform.CreateArrayList(intValue);
				lookup[i].Add(BigInteger.One);
				if (debug)
				{
					Console.WriteLine("Constructing lookup ArrayList for " + intValue);
				}
				BigInteger bigInteger2 = BigInteger.Zero;
				for (int j = 1; j < intValue; j++)
				{
					bigInteger2 = bigInteger2.Add(naccacheSternPrivateKeyParameters.PhiN);
					BigInteger e = bigInteger2.Divide(bigInteger);
					lookup[i].Add(naccacheSternPrivateKeyParameters.G.ModPow(e, naccacheSternPrivateKeyParameters.Modulus));
				}
			}
		}

		public int GetInputBlockSize()
		{
			if (forEncryption)
			{
				return (key.LowerSigmaBound + 7) / 8 - 1;
			}
			return key.Modulus.BitLength / 8 + 1;
		}

		public int GetOutputBlockSize()
		{
			if (forEncryption)
			{
				return key.Modulus.BitLength / 8 + 1;
			}
			return (key.LowerSigmaBound + 7) / 8 - 1;
		}

		public byte[] ProcessBlock(byte[] inBytes, int inOff, int length)
		{
			if (key == null)
			{
				throw new InvalidOperationException("NaccacheStern engine not initialised");
			}
			if (length > GetInputBlockSize() + 1)
			{
				throw new DataLengthException("input too large for Naccache-Stern cipher.\n");
			}
			if (!forEncryption && length < GetInputBlockSize())
			{
				throw new InvalidCipherTextException("BlockLength does not match modulus for Naccache-Stern cipher.\n");
			}
			BigInteger bigInteger = new BigInteger(1, inBytes, inOff, length);
			if (debug)
			{
				Console.WriteLine("input as BigInteger: " + ((bigInteger != null) ? bigInteger.ToString() : null));
			}
			if (forEncryption)
			{
				return Encrypt(bigInteger);
			}
			IList list = Platform.CreateArrayList();
			NaccacheSternPrivateKeyParameters naccacheSternPrivateKeyParameters = (NaccacheSternPrivateKeyParameters)key;
			IList smallPrimesList = naccacheSternPrivateKeyParameters.SmallPrimesList;
			for (int i = 0; i < smallPrimesList.Count; i++)
			{
				BigInteger bigInteger2 = bigInteger.ModPow(naccacheSternPrivateKeyParameters.PhiN.Divide((BigInteger)smallPrimesList[i]), naccacheSternPrivateKeyParameters.Modulus);
				IList list2 = lookup[i];
				if (lookup[i].Count != ((BigInteger)smallPrimesList[i]).IntValue)
				{
					if (debug)
					{
						object obj = smallPrimesList[i];
						Console.WriteLine("Prime is " + ((obj != null) ? obj.ToString() : null) + ", lookup table has size " + list2.Count);
					}
					throw new InvalidCipherTextException("Error in lookup Array for " + ((BigInteger)smallPrimesList[i]).IntValue + ": Size mismatch. Expected ArrayList with length " + ((BigInteger)smallPrimesList[i]).IntValue + " but found ArrayList of length " + lookup[i].Count);
				}
				int num = list2.IndexOf(bigInteger2);
				if (num == -1)
				{
					if (debug)
					{
						object obj2 = smallPrimesList[i];
						Console.WriteLine("Actual prime is " + ((obj2 != null) ? obj2.ToString() : null));
						Console.WriteLine("Decrypted value is " + ((bigInteger2 != null) ? bigInteger2.ToString() : null));
						string[] obj3 = new string[5] { "LookupList for ", null, null, null, null };
						object obj4 = smallPrimesList[i];
						obj3[1] = ((obj4 != null) ? obj4.ToString() : null);
						obj3[2] = " with size ";
						obj3[3] = lookup[i].Count.ToString();
						obj3[4] = " is: ";
						Console.WriteLine(string.Concat(obj3));
						for (int j = 0; j < lookup[i].Count; j++)
						{
							Console.WriteLine(lookup[i][j]);
						}
					}
					throw new InvalidCipherTextException("Lookup failed");
				}
				list.Add(BigInteger.ValueOf(num));
			}
			return chineseRemainder(list, smallPrimesList).ToByteArray();
		}

		public byte[] Encrypt(BigInteger plain)
		{
			byte[] array = new byte[key.Modulus.BitLength / 8 + 1];
			byte[] array2 = key.G.ModPow(plain, key.Modulus).ToByteArray();
			Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
			if (debug)
			{
				BigInteger bigInteger = new BigInteger(array);
				Console.WriteLine("Encrypted value is:  " + ((bigInteger != null) ? bigInteger.ToString() : null));
			}
			return array;
		}

		public byte[] AddCryptedBlocks(byte[] block1, byte[] block2)
		{
			if (forEncryption)
			{
				if (block1.Length > GetOutputBlockSize() || block2.Length > GetOutputBlockSize())
				{
					throw new InvalidCipherTextException("BlockLength too large for simple addition.\n");
				}
			}
			else if (block1.Length > GetInputBlockSize() || block2.Length > GetInputBlockSize())
			{
				throw new InvalidCipherTextException("BlockLength too large for simple addition.\n");
			}
			BigInteger bigInteger = new BigInteger(1, block1);
			BigInteger bigInteger2 = new BigInteger(1, block2);
			BigInteger bigInteger3 = bigInteger.Multiply(bigInteger2);
			bigInteger3 = bigInteger3.Mod(key.Modulus);
			if (debug)
			{
				Console.WriteLine("c(m1) as BigInteger:....... " + ((bigInteger != null) ? bigInteger.ToString() : null));
				Console.WriteLine("c(m2) as BigInteger:....... " + ((bigInteger2 != null) ? bigInteger2.ToString() : null));
				BigInteger bigInteger4 = bigInteger3;
				Console.WriteLine("c(m1)*c(m2)%n = c(m1+m2)%n: " + ((bigInteger4 != null) ? bigInteger4.ToString() : null));
			}
			byte[] array = new byte[key.Modulus.BitLength / 8 + 1];
			byte[] array2 = bigInteger3.ToByteArray();
			Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
			return array;
		}

		public byte[] ProcessData(byte[] data)
		{
			if (debug)
			{
				Console.WriteLine();
			}
			if (data.Length > GetInputBlockSize())
			{
				int inputBlockSize = GetInputBlockSize();
				int outputBlockSize = GetOutputBlockSize();
				if (debug)
				{
					Console.WriteLine("Input blocksize is:  " + inputBlockSize + " bytes");
					Console.WriteLine("Output blocksize is: " + outputBlockSize + " bytes");
					Console.WriteLine("Data has length:.... " + data.Length + " bytes");
				}
				int num = 0;
				int num2 = 0;
				byte[] array = new byte[(data.Length / inputBlockSize + 1) * outputBlockSize];
				while (num < data.Length)
				{
					byte[] array2;
					if (num + inputBlockSize < data.Length)
					{
						array2 = ProcessBlock(data, num, inputBlockSize);
						num += inputBlockSize;
					}
					else
					{
						array2 = ProcessBlock(data, num, data.Length - num);
						num += data.Length - num;
					}
					if (debug)
					{
						Console.WriteLine("new datapos is " + num);
					}
					if (array2 != null)
					{
						array2.CopyTo(array, num2);
						num2 += array2.Length;
						continue;
					}
					if (debug)
					{
						Console.WriteLine("cipher returned null");
					}
					throw new InvalidCipherTextException("cipher returned null");
				}
				byte[] array3 = new byte[num2];
				Array.Copy(array, 0, array3, 0, num2);
				if (debug)
				{
					Console.WriteLine("returning " + array3.Length + " bytes");
				}
				return array3;
			}
			if (debug)
			{
				Console.WriteLine("data size is less then input block size, processing directly");
			}
			return ProcessBlock(data, 0, data.Length);
		}

		private static BigInteger chineseRemainder(IList congruences, IList primes)
		{
			BigInteger bigInteger = BigInteger.Zero;
			BigInteger bigInteger2 = BigInteger.One;
			for (int i = 0; i < primes.Count; i++)
			{
				bigInteger2 = bigInteger2.Multiply((BigInteger)primes[i]);
			}
			for (int j = 0; j < primes.Count; j++)
			{
				BigInteger bigInteger3 = (BigInteger)primes[j];
				BigInteger bigInteger4 = bigInteger2.Divide(bigInteger3);
				BigInteger val = bigInteger4.ModInverse(bigInteger3);
				BigInteger bigInteger5 = bigInteger4.Multiply(val);
				bigInteger5 = bigInteger5.Multiply((BigInteger)congruences[j]);
				bigInteger = bigInteger.Add(bigInteger5);
			}
			return bigInteger.Mod(bigInteger2);
		}
	}
}
