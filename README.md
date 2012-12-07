LZMA SDK Helper
===============

LZMA helper class written in C# for compression of arbitrary Streams. Based on LZMA SDK ver. 9.22 from 7-zip.

You can either add the project to your solution or download the latest LZMA SDK version from http://www.7-zip.org/sdk.html and put it together with SevenZipHelper.cs file.

Example usage
-------------
	using(Stream inStream = new MemoryStream(new byte[]{ 0x23, 0x44, 0xff, 0x12, 0xA4, 0x3F, 0x11, 0x00, 0x00, 0x00, 0x22, 0x22, 0x22 }))
	{
		using(Stream outStream = new FileStream(@"compressed.lzma", FileMode.Create))
		{
			SevenZip.Helper.Compress(inStream, outStream);
		}
	}
	
	using(Stream inStream = new FileStream(@"compressed.lzma", FileMode.Open))
	{
		using(Stream outStream = new MemoryStream())
		{
			SevenZip.Helper.Decompress(inStream, outStream);
			
			byte[] bytes = new byte[outStream.Length];
			outStream.Seek(0, SeekOrigin.Begin);
			outStream.Read(bytes, 0, bytes.Length);
			
			Console.WriteLine(BitConverter.ToString(bytes, 0, bytes.Length));
		}
	}
	
	
	Console.Write("Press any key to continue . . . ");
	Console.ReadKey(true);


WARNING
-------
Remember to uncheck for arithmetic overflow/underflow under compilation settings.