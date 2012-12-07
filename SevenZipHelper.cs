using System;
using System.IO;

namespace SevenZip
{
	public static class Helper
	{
		#region Settings
		
		static Int32 dictionary = 1 << 23; // 8 MB

		static Int32 posStateBits = 2;
		static Int32 litContextBits = 3;
		static Int32 litPosBits = 0;
		static Int32 algorithm = 2;
		static Int32 numFastBytes = 128;

		static bool eos = false;
		
		static CoderPropID[] propIDs =
		{
			CoderPropID.DictionarySize,
			CoderPropID.PosStateBits,
			CoderPropID.LitContextBits,
			CoderPropID.LitPosBits,
			CoderPropID.Algorithm,
			CoderPropID.NumFastBytes,
			CoderPropID.MatchFinder,
			CoderPropID.EndMarker
		};
		
		static object[] properties =
		{
			(Int32)(dictionary),
			(Int32)(posStateBits),
			(Int32)(litContextBits),
			(Int32)(litPosBits),
			(Int32)(algorithm),
			(Int32)(numFastBytes),
			"bt4",
			eos
		};
		
		#endregion
		
		/// <summary>
		/// Compress stream using LZMA.
		/// Remeber to set desirable positions in input and output streams.
		/// </summary>
		/// <param name="inStream">Input stream</param>
		/// <param name="outStream">Output stream</param>
		public static void Compress(Stream inStream, ref Stream outStream)
		{
			Compression.LZMA.Encoder encoder = new Compression.LZMA.Encoder();
			
			encoder.SetCoderProperties(propIDs, properties);
			encoder.WriteCoderProperties(outStream);
			
			Int64 fileSize = inStream.Length;
			for (int i = 0; i < 8; i++)
				outStream.WriteByte((Byte)(fileSize >> (8 * i)));
			
			encoder.Code(inStream, outStream, -1, -1, null);
		}

		/// <summary>
		/// Decompress LZMA compressed stream into outStream.
		/// Remeber to set desirable positions in input and output streams.
		/// </summary>
		/// <param name="inStream">Input stream</param>
		/// <param name="outStream">Output stream</param>
		public static void Decompress(Stream inStream, ref Stream outStream)
		{
			byte[] properties = new byte[5];
			if (inStream.Read(properties, 0, 5) != 5)
				throw (new Exception("Input stream is too short."));
			
			Compression.LZMA.Decoder decoder = new Compression.LZMA.Decoder();
			decoder.SetDecoderProperties(properties);
			
			long outSize = 0;
			for (int i = 0; i <8; i++)
			{
				int v = inStream.ReadByte();
				if (v < 0)
					throw (new Exception("Input stream is too short."));
				outSize |= ((long)(byte)v) << (8 * i);
			}
			
			long compressedSize = inStream.Length - inStream.Position;
			decoder.Code(inStream, outStream, compressedSize, outSize, null);
		}
	}
}
