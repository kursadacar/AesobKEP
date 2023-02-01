using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class NaccacheSternKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		private static readonly int[] smallPrimes = new int[101]
		{
			3, 5, 7, 11, 13, 17, 19, 23, 29, 31,
			37, 41, 43, 47, 53, 59, 61, 67, 71, 73,
			79, 83, 89, 97, 101, 103, 107, 109, 113, 127,
			131, 137, 139, 149, 151, 157, 163, 167, 173, 179,
			181, 191, 193, 197, 199, 211, 223, 227, 229, 233,
			239, 241, 251, 257, 263, 269, 271, 277, 281, 283,
			293, 307, 311, 313, 317, 331, 337, 347, 349, 353,
			359, 367, 373, 379, 383, 389, 397, 401, 409, 419,
			421, 431, 433, 439, 443, 449, 457, 461, 463, 467,
			479, 487, 491, 499, 503, 509, 521, 523, 541, 547,
			557
		};

		private NaccacheSternKeyGenerationParameters param;

		public void Init(KeyGenerationParameters parameters)
		{
			param = (NaccacheSternKeyGenerationParameters)parameters;
		}

		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			int strength = param.Strength;
			SecureRandom random = param.Random;
			int certainty = param.Certainty;
			bool isDebug = param.IsDebug;
			if (isDebug)
			{
				Console.WriteLine("Fetching first " + param.CountSmallPrimes + " primes.");
			}
			IList arr = findFirstPrimes(param.CountSmallPrimes);
			arr = permuteList(arr, random);
			BigInteger bigInteger = BigInteger.One;
			BigInteger bigInteger2 = BigInteger.One;
			for (int i = 0; i < arr.Count / 2; i++)
			{
				bigInteger = bigInteger.Multiply((BigInteger)arr[i]);
			}
			for (int j = arr.Count / 2; j < arr.Count; j++)
			{
				bigInteger2 = bigInteger2.Multiply((BigInteger)arr[j]);
			}
			BigInteger bigInteger3 = bigInteger.Multiply(bigInteger2);
			int num = strength - bigInteger3.BitLength - 48;
			BigInteger bigInteger4 = generatePrime(num / 2 + 1, certainty, random);
			BigInteger bigInteger5 = generatePrime(num / 2 + 1, certainty, random);
			long num2 = 0L;
			if (isDebug)
			{
				Console.WriteLine("generating p and q");
			}
			BigInteger val = bigInteger4.Multiply(bigInteger).ShiftLeft(1);
			BigInteger val2 = bigInteger5.Multiply(bigInteger2).ShiftLeft(1);
			BigInteger bigInteger6;
			BigInteger bigInteger7;
			BigInteger bigInteger8;
			BigInteger bigInteger9;
			while (true)
			{
				num2++;
				bigInteger6 = generatePrime(24, certainty, random);
				bigInteger7 = bigInteger6.Multiply(val).Add(BigInteger.One);
				if (!bigInteger7.IsProbablePrime(certainty))
				{
					continue;
				}
				while (true)
				{
					bigInteger8 = generatePrime(24, certainty, random);
					if (!bigInteger6.Equals(bigInteger8))
					{
						bigInteger9 = bigInteger8.Multiply(val2).Add(BigInteger.One);
						if (bigInteger9.IsProbablePrime(certainty))
						{
							break;
						}
					}
				}
				if (!bigInteger3.Gcd(bigInteger6.Multiply(bigInteger8)).Equals(BigInteger.One))
				{
					Console.WriteLine("sigma.gcd(_p.mult(_q)) != 1!\n _p: " + ((bigInteger6 != null) ? bigInteger6.ToString() : null) + "\n _q: " + ((bigInteger8 != null) ? bigInteger8.ToString() : null));
					continue;
				}
				if (bigInteger7.Multiply(bigInteger9).BitLength >= strength)
				{
					break;
				}
				if (isDebug)
				{
					Console.WriteLine("key size too small. Should be " + strength + " but is actually " + bigInteger7.Multiply(bigInteger9).BitLength);
				}
			}
			if (isDebug)
			{
				Console.WriteLine("needed " + num2 + " tries to generate p and q.");
			}
			BigInteger bigInteger10 = bigInteger7.Multiply(bigInteger9);
			BigInteger bigInteger11 = bigInteger7.Subtract(BigInteger.One).Multiply(bigInteger9.Subtract(BigInteger.One));
			num2 = 0L;
			if (isDebug)
			{
				Console.WriteLine("generating g");
			}
			BigInteger bigInteger12;
			while (true)
			{
				IList list = Platform.CreateArrayList();
				for (int k = 0; k != arr.Count; k++)
				{
					BigInteger val3 = (BigInteger)arr[k];
					BigInteger e = bigInteger11.Divide(val3);
					do
					{
						num2++;
						bigInteger12 = generatePrime(strength, certainty, random);
					}
					while (bigInteger12.ModPow(e, bigInteger10).Equals(BigInteger.One));
					list.Add(bigInteger12);
				}
				bigInteger12 = BigInteger.One;
				for (int l = 0; l < arr.Count; l++)
				{
					BigInteger bigInteger13 = (BigInteger)list[l];
					BigInteger val4 = (BigInteger)arr[l];
					bigInteger12 = bigInteger12.Multiply(bigInteger13.ModPow(bigInteger3.Divide(val4), bigInteger10)).Mod(bigInteger10);
				}
				bool flag = false;
				for (int m = 0; m < arr.Count; m++)
				{
					if (bigInteger12.ModPow(bigInteger11.Divide((BigInteger)arr[m]), bigInteger10).Equals(BigInteger.One))
					{
						if (isDebug)
						{
							object obj = arr[m];
							string obj2 = ((obj != null) ? obj.ToString() : null);
							BigInteger bigInteger14 = bigInteger12;
							Console.WriteLine("g has order phi(n)/" + obj2 + "\n g: " + ((bigInteger14 != null) ? bigInteger14.ToString() : null));
						}
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
				if (bigInteger12.ModPow(bigInteger11.ShiftRight(2), bigInteger10).Equals(BigInteger.One))
				{
					if (isDebug)
					{
						BigInteger bigInteger15 = bigInteger12;
						Console.WriteLine("g has order phi(n)/4\n g:" + ((bigInteger15 != null) ? bigInteger15.ToString() : null));
					}
					continue;
				}
				if (bigInteger12.ModPow(bigInteger11.Divide(bigInteger6), bigInteger10).Equals(BigInteger.One))
				{
					if (isDebug)
					{
						BigInteger bigInteger16 = bigInteger12;
						Console.WriteLine("g has order phi(n)/p'\n g: " + ((bigInteger16 != null) ? bigInteger16.ToString() : null));
					}
					continue;
				}
				if (bigInteger12.ModPow(bigInteger11.Divide(bigInteger8), bigInteger10).Equals(BigInteger.One))
				{
					if (isDebug)
					{
						BigInteger bigInteger17 = bigInteger12;
						Console.WriteLine("g has order phi(n)/q'\n g: " + ((bigInteger17 != null) ? bigInteger17.ToString() : null));
					}
					continue;
				}
				if (bigInteger12.ModPow(bigInteger11.Divide(bigInteger4), bigInteger10).Equals(BigInteger.One))
				{
					if (isDebug)
					{
						BigInteger bigInteger18 = bigInteger12;
						Console.WriteLine("g has order phi(n)/a\n g: " + ((bigInteger18 != null) ? bigInteger18.ToString() : null));
					}
					continue;
				}
				if (!bigInteger12.ModPow(bigInteger11.Divide(bigInteger5), bigInteger10).Equals(BigInteger.One))
				{
					break;
				}
				if (isDebug)
				{
					BigInteger bigInteger19 = bigInteger12;
					Console.WriteLine("g has order phi(n)/b\n g: " + ((bigInteger19 != null) ? bigInteger19.ToString() : null));
				}
			}
			if (isDebug)
			{
				Console.WriteLine("needed " + num2 + " tries to generate g");
				Console.WriteLine();
				Console.WriteLine("found new NaccacheStern cipher variables:");
				Console.WriteLine("smallPrimes: " + CollectionUtilities.ToString(arr));
				string[] obj3 = new string[5] { "sigma:...... ", null, null, null, null };
				obj3[1] = ((bigInteger3 != null) ? bigInteger3.ToString() : null);
				obj3[2] = " (";
				obj3[3] = bigInteger3.BitLength.ToString();
				obj3[4] = " bits)";
				Console.WriteLine(string.Concat(obj3));
				Console.WriteLine("a:.......... " + ((bigInteger4 != null) ? bigInteger4.ToString() : null));
				Console.WriteLine("b:.......... " + ((bigInteger5 != null) ? bigInteger5.ToString() : null));
				Console.WriteLine("p':......... " + ((bigInteger6 != null) ? bigInteger6.ToString() : null));
				Console.WriteLine("q':......... " + ((bigInteger8 != null) ? bigInteger8.ToString() : null));
				Console.WriteLine("p:.......... " + ((bigInteger7 != null) ? bigInteger7.ToString() : null));
				Console.WriteLine("q:.......... " + ((bigInteger9 != null) ? bigInteger9.ToString() : null));
				Console.WriteLine("n:.......... " + ((bigInteger10 != null) ? bigInteger10.ToString() : null));
				Console.WriteLine("phi(n):..... " + ((bigInteger11 != null) ? bigInteger11.ToString() : null));
				BigInteger bigInteger20 = bigInteger12;
				Console.WriteLine("g:.......... " + ((bigInteger20 != null) ? bigInteger20.ToString() : null));
				Console.WriteLine();
			}
			return new AsymmetricCipherKeyPair(new NaccacheSternKeyParameters(false, bigInteger12, bigInteger10, bigInteger3.BitLength), new NaccacheSternPrivateKeyParameters(bigInteger12, bigInteger10, bigInteger3.BitLength, arr, bigInteger11));
		}

		private static BigInteger generatePrime(int bitLength, int certainty, SecureRandom rand)
		{
			return new BigInteger(bitLength, certainty, rand);
		}

		private static IList permuteList(IList arr, SecureRandom rand)
		{
			IList list = Platform.CreateArrayList(arr.Count);
			foreach (object item in arr)
			{
				int index = rand.Next(list.Count + 1);
				list.Insert(index, item);
			}
			return list;
		}

		private static IList findFirstPrimes(int count)
		{
			IList list = Platform.CreateArrayList(count);
			for (int i = 0; i != count; i++)
			{
				list.Add(BigInteger.ValueOf(smallPrimes[i]));
			}
			return list;
		}
	}
}
