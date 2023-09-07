using System.IO.Compression;
using System.Security.Cryptography;

namespace ZipService.Api
{
    public static class Zip
    {
        // Write a zip file with the specified number of files, each with 1 MiB of random content.
        public static async Task WriteAsync(Stream stream, int files, CancellationToken cancellationToken)
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);

            var buffer = new byte[8192];  // 8 KiB chunk

            for (var i = 0; i < files; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                RandomNumberGenerator.Fill(buffer);
                var entryName = $"{i}";
                var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);

                await using var entryStream = entry.Open();

                for (var j = 0; j < 128; j++)  // 128 chunks * 8 KiB = 1 MiB
                {
                    await entryStream.WriteAsync(buffer, cancellationToken);
                }
            }
        }

        public static void Write(Stream stream, int files)
        {
            var seed = 123;
            var rnd = new Random(seed);
            // leaveOpen: true added
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
            {
                var buffer = new byte[0x100000];
                for (var i = 0; i < files; i++)
                {
                    rnd.NextBytes(buffer);
                    var name = i.ToString();
                    var entry = archive.CreateEntry(name, CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(buffer);
                    }
                }
            }
        }
    }
}
