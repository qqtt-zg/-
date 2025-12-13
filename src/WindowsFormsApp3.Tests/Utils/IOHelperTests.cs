using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp3.Utils;
using Xunit;

namespace WindowsFormsApp3.Tests.Utils
{
    /// <summary>
    /// IOHelper类单元测试
    /// </summary>
    public class IOHelperTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _testFile;

        public IOHelperTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), "IOHelperTests", Guid.NewGuid().ToString());
            _testFile = Path.Combine(_testDirectory, "test.txt");
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                try
                {
                    Directory.Delete(_testDirectory, true);
                }
                catch
                {
                    // 忽略清理错误
                }
            }
        }

        [Fact]
        public void EnsureDirectoryExists_WhenDirectoryDoesNotExist_ShouldCreateDirectory()
        {
            // Arrange
            string testPath = Path.Combine(_testDirectory, "new_directory");

            // Act
            IOHelper.EnsureDirectoryExists(testPath);

            // Assert
            Assert.True(Directory.Exists(testPath));
        }

        [Fact]
        public void EnsureDirectoryExists_WhenDirectoryAlreadyExists_ShouldNotThrow()
        {
            // Arrange
            Directory.CreateDirectory(_testDirectory);

            // Act & Assert
            var ex = Record.Exception(() => IOHelper.EnsureDirectoryExists(_testDirectory));
            Assert.Null(ex);
            Assert.True(Directory.Exists(_testDirectory));
        }

        [Fact]
        public void EnsureDirectoryExists_WhenPathIsEmpty_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => IOHelper.EnsureDirectoryExists(""));
            Assert.Throws<ArgumentException>(() => IOHelper.EnsureDirectoryExists(null));
            Assert.Throws<ArgumentException>(() => IOHelper.EnsureDirectoryExists("   "));
        }

        [Fact]
        public async Task EnsureDirectoryExistsAsync_WhenDirectoryDoesNotExist_ShouldCreateDirectory()
        {
            // Arrange
            string testPath = Path.Combine(_testDirectory, "async_directory");
            var cancellationToken = new CancellationToken();

            // Act
            await IOHelper.EnsureDirectoryExistsAsync(testPath, cancellationToken);

            // Assert
            Assert.True(Directory.Exists(testPath));
        }

        [Fact]
        public async Task EnsureDirectoryExistsAsync_WhenCancelled_ShouldThrowOperationCancelledException()
        {
            // Arrange
            string testPath = Path.Combine(_testDirectory, "cancelled_directory");
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => IOHelper.EnsureDirectoryExistsAsync(testPath, cts.Token));
        }

        [Fact]
        public void SafeMoveFile_WhenSourceFileExists_ShouldMoveFile()
        {
            // Arrange
            Directory.CreateDirectory(_testDirectory);
            File.WriteAllText(_testFile, "test content");
            string destinationPath = Path.Combine(_testDirectory, "moved.txt");

            // Act
            string result = IOHelper.SafeMoveFile(_testFile, destinationPath);

            // Assert
            Assert.False(File.Exists(_testFile));
            Assert.True(File.Exists(destinationPath));
            Assert.Equal(destinationPath, result);
            Assert.Equal("test content", File.ReadAllText(destinationPath));
        }

        [Fact]
        public void SafeMoveFile_WhenSourceFileDoesNotExist_ShouldThrowFileNotFoundException()
        {
            // Arrange
            string destinationPath = Path.Combine(_testDirectory, "moved.txt");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => IOHelper.SafeMoveFile(_testFile, destinationPath));
        }

        [Fact]
        public void SafeCopyFile_WhenSourceFileExists_ShouldCopyFile()
        {
            // Arrange
            Directory.CreateDirectory(_testDirectory);
            File.WriteAllText(_testFile, "test content");
            string destinationPath = Path.Combine(_testDirectory, "copied.txt");

            // Act
            string result = IOHelper.SafeCopyFile(_testFile, destinationPath);

            // Assert
            Assert.True(File.Exists(_testFile));
            Assert.True(File.Exists(destinationPath));
            Assert.Equal(destinationPath, result);
            Assert.Equal("test content", File.ReadAllText(destinationPath));
        }

        [Fact]
        public void SafeCopyFile_WithOverwrite_ShouldOverwriteExistingFile()
        {
            // Arrange
            Directory.CreateDirectory(_testDirectory);
            File.WriteAllText(_testFile, "original content");
            string destinationPath = Path.Combine(_testDirectory, "copied.txt");
            File.WriteAllText(destinationPath, "old content");

            // Act
            string result = IOHelper.SafeCopyFile(_testFile, destinationPath, overwrite: true);

            // Assert
            Assert.True(File.Exists(_testFile));
            Assert.True(File.Exists(destinationPath));
            Assert.Equal("original content", File.ReadAllText(destinationPath));
        }

        [Fact]
        public void HandleFileNameConflict_WhenFileDoesNotExist_ShouldReturnOriginalPath()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_testDirectory, "nonexistent.txt");

            // Act
            string result = IOHelper.HandleFileNameConflict(nonExistentFile);

            // Assert
            Assert.Equal(nonExistentFile, result);
        }

        [Fact]
        public void HandleFileNameConflict_WhenFileExists_ShouldReturnNewPath()
        {
            // Arrange
            Directory.CreateDirectory(_testDirectory);
            string existingFile = Path.Combine(_testDirectory, "conflict.txt");
            File.WriteAllText(existingFile, "content");

            // Act
            string result = IOHelper.HandleFileNameConflict(existingFile);

            // Assert
            Assert.NotEqual(existingFile, result);
            Assert.Contains("_1", result);
            Assert.False(File.Exists(result));
        }

        [Fact]
        public void HandleFileNameConflict_WhenMultipleFilesExist_ShouldReturnUniquePath()
        {
            // Arrange
            Directory.CreateDirectory(_testDirectory);
            string baseFile = Path.Combine(_testDirectory, "multiple.txt");
            File.WriteAllText(baseFile, "content");
            File.WriteAllText(Path.Combine(_testDirectory, "multiple_1.txt"), "content");
            File.WriteAllText(Path.Combine(_testDirectory, "multiple_2.txt"), "content");

            // Act
            string result = IOHelper.HandleFileNameConflict(baseFile);

            // Assert
            Assert.Contains("_3", result);
            Assert.False(File.Exists(result));
        }

        [Fact]
        public void IsFileAccessible_WhenFileExistsAndNotLocked_ShouldReturnTrue()
        {
            // Arrange
            Directory.CreateDirectory(_testDirectory);
            File.WriteAllText(_testFile, "test content");

            // Act
            bool result = IOHelper.IsFileAccessible(_testFile);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsFileAccessible_WhenFileDoesNotExist_ShouldReturnFalse()
        {
            // Act
            bool result = IOHelper.IsFileAccessible(_testFile);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(0, "0 B")]
        [InlineData(1024, "1 KB")]
        [InlineData(1048576, "1 MB")]
        [InlineData(1073741824, "1 GB")]
        [InlineData(1099511627776, "1 TB")]
        [InlineData(1536, "1.5 KB")]
        [InlineData(2621440, "2.5 MB")]
        public void FormatFileSize_ShouldReturnCorrectFormat(long bytes, string expected)
        {
            // Act
            string result = IOHelper.FormatFileSize(bytes);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SafeCopyFile_WhenSourceFileDoesNotExist_ShouldThrowFileNotFoundException()
        {
            // Arrange
            string destinationPath = Path.Combine(_testDirectory, "copied.txt");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => IOHelper.SafeCopyFile(_testFile, destinationPath));
        }
    }
}