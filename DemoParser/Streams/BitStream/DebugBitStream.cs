﻿using System;
using System.Linq;

namespace DemoParser_Core.Streams.BitStream
{
	public class DebugBitStream : IBitStream
	{
		private readonly IBitStream A, B;

		public DebugBitStream(IBitStream a, IBitStream b)
		{
			this.A = a;
			this.B = b;
		}

		public void Initialize(System.IO.Stream stream)
		{
			throw new NotImplementedException();
		}

		void IDisposable.Dispose()
		{
			A.Dispose();
			B.Dispose();
		}

		private void Verify<T>(T a, T b)
		{
			if (!a.Equals(b)) {
				System.Diagnostics.Debug.Assert(false);
				throw new InvalidOperationException();
			}
		}

		public uint ReadInt(int bits)
		{
			var a = A.ReadInt(bits);
			var b = B.ReadInt(bits);
			Verify(a, b);
			return a;
		}

		public int ReadSignedInt(int bits)
		{
			var a = A.ReadSignedInt(bits);
			var b = B.ReadSignedInt(bits);
			Verify(a, b);
			return a;
		}

		public uint PeekInt(int bits)
		{
			var a = A.PeekInt(bits);
			var b = B.PeekInt(bits);
			Verify(a, b);
			return a;
		}

		public bool ReadBit()
		{
			var a = A.ReadBit();
			var b = B.ReadBit();
			Verify(a, b);
			return a;
		}

		public byte ReadByte()
		{
			var a = A.ReadByte();
			var b = B.ReadByte();
			Verify(a, b);
			return a;
		}

		public byte ReadByte(int bits)
		{
			var a = A.ReadByte(bits);
			var b = B.ReadByte(bits);
			Verify(a, b);
			return a;
		}

		public byte[] ReadBytes(int bytes)
		{
			var a = A.ReadBytes(bytes);
			var b = B.ReadBytes(bytes);
			Verify(a.SequenceEqual(b), true);
			return a;
		}

		public string ReadString()
		{
			var a = A.ReadString();
			var b = B.ReadString();
			Verify(a, b);
			return a;
		}

		public string ReadString(int size)
		{
			var a = A.ReadString(size);
			var b = B.ReadString(size);
			Verify(a, b);
			return a;
		}

		public uint ReadVarInt()
		{
			var a = A.ReadVarInt();
			var b = B.ReadVarInt();
			Verify(a, b);
			return a;
		}

		public uint ReadUBitInt()
		{
			var a = A.ReadUBitInt();
			var b = B.ReadUBitInt();
			Verify(a, b);
			return a;
		}
	}
}

