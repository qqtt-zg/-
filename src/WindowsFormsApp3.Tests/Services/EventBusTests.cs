using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Tests.Services
{
    public class EventBusTests
    {
        private EventBus _eventBus;
        private Mock<WindowsFormsApp3.Interfaces.ILogger> _mockLogger; // 明确指定使用Interfaces命名空间的ILogger

        public EventBusTests()
        {
            _mockLogger = new Mock<WindowsFormsApp3.Interfaces.ILogger>();
            _eventBus = new EventBus(_mockLogger.Object);
        }

        public class TestEvent
        {
            public string Message { get; set; }
        }

        [Fact]
        public void Publish_Should_Call_Sync_Subscribers()
        {
            // Arrange
            var eventReceived = false;
            var testEvent = new TestEvent { Message = "Hello" };

            _eventBus.Subscribe<TestEvent>(e => {
                eventReceived = true;
                Assert.Equal("Hello", e.Message);
            });

            // Act
            _eventBus.Publish(testEvent);

            // Assert
            Assert.True(eventReceived);
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task PublishAsync_Should_Call_Sync_And_Async_Subscribers()
        {
            // Arrange
            var syncReceived = false;
            var asyncReceived = false;
            var testEvent = new TestEvent { Message = "Hello Async" };

            _eventBus.Subscribe<TestEvent>(e => {
                syncReceived = true;
                Assert.Equal("Hello Async", e.Message);
            });

            _eventBus.Subscribe<TestEvent>(async e => {
                await Task.Delay(10); // 模拟异步操作
                asyncReceived = true;
                Assert.Equal("Hello Async", e.Message);
            });

            // Act
            await _eventBus.PublishAsync(testEvent);

            // Assert
            Assert.True(syncReceived);
            Assert.True(asyncReceived);
            _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Subscribe_And_Unsubscribe_Should_Work_Correctly()
        {
            // Arrange
            var callCount = 0;
            Action<TestEvent> handler = e => callCount++;

            // Act & Assert
            _eventBus.Subscribe(handler);
            Assert.Equal(1, _eventBus.GetSubscriberCount<TestEvent>());

            _eventBus.Publish(new TestEvent());
            Assert.Equal(1, callCount);

            _eventBus.Unsubscribe(handler);
            Assert.Equal(0, _eventBus.GetSubscriberCount<TestEvent>());

            _eventBus.Publish(new TestEvent());
            Assert.Equal(1, callCount); // 应该仍然是1，因为处理程序已被取消订阅
        }

        [Fact]
        public void ClearAllSubscribers_Should_Remove_All_Subscribers()
        {
            // Arrange
            _eventBus.Subscribe<TestEvent>(e => { });
            _eventBus.Subscribe<TestEvent>(async e => await Task.CompletedTask);

            Assert.Equal(2, _eventBus.GetSubscriberCount<TestEvent>());

            // Act
            _eventBus.ClearAllSubscribers();

            // Assert
            Assert.Equal(0, _eventBus.GetSubscriberCount<TestEvent>());
            _mockLogger.Verify(l => l.LogInformation("Cleared all event subscribers"), Times.Once);
        }

        [Fact]
        public void Publish_Should_Handle_Exceptions_Gracefully()
        {
            // Arrange
            var testEvent = new TestEvent { Message = "Error Test" };
            var exceptionThrown = false;

            _eventBus.Subscribe<TestEvent>(e => {
                throw new InvalidOperationException("Test exception");
            });

            // Act
            try
            {
                _eventBus.Publish(testEvent);
            }
            catch
            {
                exceptionThrown = true;
            }

            // Assert
            // 不应该抛出异常，因为EventBus应该捕获并记录它
            Assert.False(exceptionThrown);
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PublishAsync_Should_Handle_Exceptions_Gracefully()
        {
            // Arrange
            var testEvent = new TestEvent { Message = "Async Error Test" };

            _eventBus.Subscribe<TestEvent>(async e => {
                await Task.Delay(1);
                throw new InvalidOperationException("Test async exception");
            });

            // Act
            await _eventBus.PublishAsync(testEvent);

            // Assert
            _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Publish_With_Null_Event_Should_Throw_ArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _eventBus.Publish<TestEvent>(null));
        }

        [Fact]
        public async Task PublishAsync_With_Null_Event_Should_Throw_ArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _eventBus.PublishAsync<TestEvent>(null));
        }

        [Fact]
        public void Subscribe_With_Null_Handler_Should_Throw_ArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _eventBus.Subscribe<TestEvent>(null as Action<TestEvent>));
        }

        [Fact]
        public void Subscribe_Async_With_Null_Handler_Should_Throw_ArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _eventBus.Subscribe<TestEvent>(null as Func<TestEvent, Task>));
        }
    }
}