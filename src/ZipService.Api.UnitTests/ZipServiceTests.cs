namespace ZipService.Api.UnitTests
{
    public class ZipServiceTests
    {
        private readonly CancellationTokenSource _cts = new();

        [Fact]
        public void Write_ZipWithZeroFiles_ShouldBeValid()
        {
            var memoryStream = new MemoryStream();
            Zip.Write(memoryStream, 0);

            Assert.True(memoryStream.Length > 0);
        }

        [Fact]
        public async void WriteAsync_ZipWithZeroFiles_ShouldBeValid()
        {
            var memoryStream = new MemoryStream();
            await Zip.WriteAsync(memoryStream, 0, _cts.Token);

            Assert.True(memoryStream.Length > 0);
        }

        [Fact]
        public void Write_ZipWithTenFiles_ShouldBeValid()
        {
            var memoryStream = new MemoryStream();
            Zip.Write(memoryStream, 10);

            Assert.True(memoryStream.Length > 0);
        }

        [Fact]
        public async void WriteAsync_ZipWithTenFiles_ShouldBeValid()
        {
            var memoryStream = new MemoryStream();
            await Zip.WriteAsync(memoryStream, 10, _cts.Token);

            Assert.True(memoryStream.Length > 0);
        }
    }
}