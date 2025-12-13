using Xunit;

namespace WindowsFormsApp3.Tests
{
    public class BasicTests
    {
        [Fact]
        public void Addition_Test()
        {
            // 这是一个简单的测试，验证1+1=2
            int result = 1 + 1;
            Assert.Equal(2, result);
        }

        [Fact]
        public void String_Test()
        {
            // 这是另一个简单的测试，验证字符串操作
            string str = "Hello";
            str += " World";
            Assert.Equal("Hello World", str);
        }
    }
}