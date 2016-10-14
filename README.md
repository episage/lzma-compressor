# Easy Compression

	using(Stream inStream = new MemoryStream(arrayToCompress))
	using(Stream outStream = new FileStream(@"compressed.lzma", FileMode.Create))
	{
		SevenZip.Helper.Compress(inStream, outStream);
	}

# Easy Decompression

	using(Stream inStream = new FileStream(@"compressed.lzma", FileMode.Open))
	using(Stream outStream = new MemoryStream())
	{
		SevenZip.Helper.Decompress(inStream, outStream);
	}

# Requirements
	
 - .NET 2.0 or greater
	
# Usage

Add this project to your Visual Studio solution. 
Reference it from your Visual Studio project and use `SevenZip.Helper` namespace.

# Demo

	using System;
	using System.IO;

	namespace ConsoleApplication12
	{
		class Program
		{
			static void Main(string[] args)
			{
				var bytesToCompress = GetBytes("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam tincidunt dui at ligula ullamcorper, at sagittis magna molestie. Curabitur quis magna nec lacus feugiat iaculis. In ut orci non nisl rutrum ultricies. Aliquam ex sapien, dapibus id cursus ac, malesuada nec risus. Integer vulputate rutrum viverra. Donec risus risus, tempus vel ipsum at, facilisis malesuada nunc. Duis non quam sed mi finibus venenatis sed in arcu. Nam eget ornare dui. Praesent ligula massa, varius eget risus sed, euismod interdum velit. Nulla hendrerit velit ut augue dapibus, et interdum nisl accumsan. Nunc convallis consequat nibh, eu facilisis lorem lacinia id. Donec vitae massa nulla. Maecenas a nisi consectetur, semper eros eget, congue ipsum. Suspendisse nec est eu tellus facilisis accumsan.");

				using (Stream inStream = new MemoryStream(bytesToCompress)) // compres in-memory byte array
				using (Stream outStream = new FileStream(@"compressed.lzma", FileMode.Create)) // to file stream
				{
					Console.WriteLine("Size of uncompressed stream = {0} bytes.", inStream.Length);

					SevenZip.Helper.Compress(inStream, outStream);
				}


				using (Stream inStream = new FileStream(@"compressed.lzma", FileMode.Open)) // decompress file stream
				using (Stream outStream = new MemoryStream()) // to memory stream and later on to bytes array
				{
					Console.WriteLine("Size of compressed stream = {0} bytes.", inStream.Length);

					SevenZip.Helper.Decompress(inStream, outStream);

					
					var decompressedBytes = StreamToByteArray(outStream);
					// here you have the original bytes back

					Console.WriteLine("LZMA compressed text {0:0.00} times.", outStream.Length / (decimal)inStream.Length);
				}


				Console.Write("Press any key to continue . . . ");
				Console.ReadKey(true);
			}

			static byte[] GetBytes(string str)
			{
				byte[] bytes = new byte[str.Length * sizeof(char)];
				System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
				return bytes;
			}

			static string GetString(byte[] bytes)
			{
				char[] chars = new char[bytes.Length / sizeof(char)];
				System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
				return new string(chars);
			}

			static byte[] StreamToByteArray(Stream ms)
			{
				byte[] bytes = new byte[ms.Length];
				ms.Seek(0, SeekOrigin.Begin);
				ms.Read(bytes, 0, bytes.Length);
				return bytes;
			}
		}
	}


Output:

	Size of uncompressed stream = 1568 bytes.
	Size of compressed stream = 536 bytes.
	LZMA compressed text 2.93 times.
	Press any key to continue . . .
	
# More info

LZMA helper class written in C# for compression of arbitrary Streams. Based on LZMA SDK ver. 9.22 from 7-zip.

You can either add the project to your solution or download the latest LZMA SDK version from http://www.7-zip.org/sdk.html and put it together with SevenZipHelper.cs file.

# License

GNU LGPL

![7-zip logo](http://bit.ly/YWOR13)
