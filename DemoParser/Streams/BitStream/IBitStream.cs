using System;
using System.IO;

namespace DemoParser_Core.Streams.BitStream
{
	public interface IBitStream : IDisposable
	{
		void Initialize(Stream stream);

		uint ReadInt(int bits);
		uint PeekInt(int bits);
		int ReadSignedInt(int numBits);
		bool ReadBit();
		byte[] ReadBits(int bits);
		byte ReadByte();
		byte ReadByte(int bits);
		byte[] ReadBytes(int bytes);
		float ReadFloat();
	}
}

