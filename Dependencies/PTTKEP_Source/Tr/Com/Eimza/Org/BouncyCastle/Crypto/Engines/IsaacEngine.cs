using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Engines
{
	internal class IsaacEngine : IStreamCipher
	{
		private static readonly int sizeL = 8;

		private static readonly int stateArraySize = sizeL << 5;

		private uint[] engineState;

		private uint[] results;

		private uint a;

		private uint b;

		private uint c;

		private int index;

		private byte[] keyStream = new byte[stateArraySize << 2];

		private byte[] workingKey;

		private bool initialised;

		public string AlgorithmName
		{
			get
			{
				return "ISAAC";
			}
		}

		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to ISAAC Init - " + parameters.GetType().Name, "parameters");
			}
			KeyParameter keyParameter = (KeyParameter)parameters;
			setKey(keyParameter.GetKey());
		}

		public byte ReturnByte(byte input)
		{
			if (index == 0)
			{
				isaac();
				keyStream = Pack.UInt32_To_BE(results);
			}
			byte result = (byte)(keyStream[index] ^ input);
			index = (index + 1) & 0x3FF;
			return result;
		}

		public void ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			if (!initialised)
			{
				throw new InvalidOperationException(AlgorithmName + " not initialised");
			}
			if (inOff + len > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + len > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			for (int i = 0; i < len; i++)
			{
				if (index == 0)
				{
					isaac();
					keyStream = Pack.UInt32_To_BE(results);
				}
				output[i + outOff] = (byte)(keyStream[index] ^ input[i + inOff]);
				index = (index + 1) & 0x3FF;
			}
		}

		public void Reset()
		{
			setKey(workingKey);
		}

		private void setKey(byte[] keyBytes)
		{
			workingKey = keyBytes;
			if (engineState == null)
			{
				engineState = new uint[stateArraySize];
			}
			if (results == null)
			{
				results = new uint[stateArraySize];
			}
			for (int i = 0; i < stateArraySize; i++)
			{
				engineState[i] = (results[i] = 0u);
			}
			a = (b = (c = 0u));
			index = 0;
			byte[] array = new byte[keyBytes.Length + (keyBytes.Length & 3)];
			Array.Copy(keyBytes, 0, array, 0, keyBytes.Length);
			for (int i = 0; i < array.Length; i += 4)
			{
				results[i >> 2] = Pack.LE_To_UInt32(array, i);
			}
			uint[] array2 = new uint[sizeL];
			for (int i = 0; i < sizeL; i++)
			{
				array2[i] = 2654435769u;
			}
			for (int i = 0; i < 4; i++)
			{
				mix(array2);
			}
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < stateArraySize; j += sizeL)
				{
					for (int k = 0; k < sizeL; k++)
					{
						array2[k] += ((i < 1) ? results[j + k] : engineState[j + k]);
					}
					mix(array2);
					for (int k = 0; k < sizeL; k++)
					{
						engineState[j + k] = array2[k];
					}
				}
			}
			isaac();
			initialised = true;
		}

		private void isaac()
		{
			b += ++c;
			for (int i = 0; i < stateArraySize; i++)
			{
				uint num = engineState[i];
				switch (i & 3)
				{
				case 0:
					a ^= a << 13;
					break;
				case 1:
					a ^= a >> 6;
					break;
				case 2:
					a ^= a << 2;
					break;
				case 3:
					a ^= a >> 16;
					break;
				}
				a += engineState[(i + 128) & 0xFF];
				uint num2 = (engineState[i] = engineState[(num >> 2) & 0xFF] + a + b);
				results[i] = (b = engineState[(num2 >> 10) & 0xFF] + num);
			}
		}

		private void mix(uint[] x)
		{
			x[0] ^= x[1] << 11;
			x[3] += x[0];
			x[1] += x[2];
			x[1] ^= x[2] >> 2;
			x[4] += x[1];
			x[2] += x[3];
			x[2] ^= x[3] << 8;
			x[5] += x[2];
			x[3] += x[4];
			x[3] ^= x[4] >> 16;
			x[6] += x[3];
			x[4] += x[5];
			x[4] ^= x[5] << 10;
			x[7] += x[4];
			x[5] += x[6];
			x[5] ^= x[6] >> 4;
			x[0] += x[5];
			x[6] += x[7];
			x[6] ^= x[7] << 8;
			x[1] += x[6];
			x[7] += x[0];
			x[7] ^= x[0] >> 9;
			x[2] += x[7];
			x[0] += x[1];
		}
	}
}
