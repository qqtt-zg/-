using System;
using System.ComponentModel;
using Xunit;
using WindowsFormsApp3;
using WindowsFormsApp3.Commands;
using WindowsFormsApp3.Services;

namespace WindowsFormsApp3.Tests
{
    public class UndoRedoManagerTests
    {
        private IUndoRedoService _undoRedoService;
        
        // 测试命令类
        private class TestCommand : CommandBase
        {
            public bool ExecuteCalled { get; private set; }
            public bool UndoCalled { get; private set; }
            public bool RedoCalled { get; private set; }

            public TestCommand() : base("Test Command")
            {
            }

            protected override void OnExecute()
            {
                ExecuteCalled = true;
            }

            protected override void OnUndo()
            {
                UndoCalled = true;
            }
        }

        public UndoRedoManagerTests()
        {
            var mockLogger = new Moq.Mock<WindowsFormsApp3.Interfaces.ILogger>();
            _undoRedoService = new UndoRedoService(mockLogger.Object);
        }

        [Fact]
        public void ExecuteCommand_Should_Add_Command_To_Undo_Stack()
        {
            // Arrange
            var command = new TestCommand();

            // Act
            _undoRedoService.ExecuteCommand(command);

            // Assert
            Assert.True(_undoRedoService.CanUndo());
            Assert.False(_undoRedoService.CanRedo());
            Assert.Equal(1, _undoRedoService.CanUndoCount);
            Assert.Equal(0, _undoRedoService.CanRedoCount);
        }

        [Fact]
        public void ExecuteCommand_Should_Clear_Redo_Stack()
        {
            // Arrange
            var command1 = new TestCommand();
            var command2 = new TestCommand();
            
            _undoRedoService.ExecuteCommand(command1);
            _undoRedoService.Undo(); // 将command1移到redo栈
            
            // Act
            _undoRedoService.ExecuteCommand(command2);

            // Assert
            Assert.True(_undoRedoService.CanUndo());
            Assert.False(_undoRedoService.CanRedo()); // redo栈应该被清空
            Assert.Equal(1, _undoRedoService.CanUndoCount);
            Assert.Equal(0, _undoRedoService.CanRedoCount);
        }

        [Fact]
        public void Undo_Should_Move_Command_To_Redo_Stack()
        {
            // Arrange
            var command = new TestCommand();
            _undoRedoService.ExecuteCommand(command);

            // Act
            _undoRedoService.Undo();

            // Assert
            Assert.False(_undoRedoService.CanUndo());
            Assert.True(_undoRedoService.CanRedo());
            Assert.Equal(0, _undoRedoService.CanUndoCount);
            Assert.Equal(1, _undoRedoService.CanRedoCount);
            Assert.True(command.UndoCalled);
        }

        [Fact]
        public void Undo_Should_Not_Throw_When_No_Commands()
        {
            // Act & Assert
            var exception = Record.Exception(() => _undoRedoService.Undo());
            Assert.Null(exception);
        }

        [Fact]
        public void Redo_Should_Move_Command_Back_To_Undo_Stack()
        {
            // Arrange
            var command = new TestCommand();
            _undoRedoService.ExecuteCommand(command);
            _undoRedoService.Undo();

            // Act
            _undoRedoService.Redo();

            // Assert
            Assert.True(_undoRedoService.CanUndo());
            Assert.False(_undoRedoService.CanRedo());
            Assert.Equal(1, _undoRedoService.CanUndoCount);
            Assert.Equal(0, _undoRedoService.CanRedoCount);
            Assert.True(command.RedoCalled);
        }

        [Fact]
        public void Redo_Should_Not_Throw_When_No_Commands()
        {
            // Act & Assert
            var exception = Record.Exception(() => _undoRedoService.Redo());
            Assert.Null(exception);
        }

        [Fact]
        public void Clear_Should_Clear_Both_Stacks()
        {
            // Arrange
            var command = new TestCommand();
            _undoRedoService.ExecuteCommand(command);

            // Act
            _undoRedoService.ClearHistory();

            // Assert
            Assert.False(_undoRedoService.CanUndo());
            Assert.False(_undoRedoService.CanRedo());
            Assert.Equal(0, _undoRedoService.CanUndoCount);
            Assert.Equal(0, _undoRedoService.CanRedoCount);
        }

      
        [Fact]
        public void GetUndoHistory_Should_Return_Command_Names()
        {
            // Arrange
            var command1 = new TestCommand();
            var command2 = new TestCommand();
            _undoRedoService.ExecuteCommand(command1);
            _undoRedoService.ExecuteCommand(command2);

            // Act
            var history = _undoRedoService.GetUndoHistory();

            // Assert
            Assert.Contains("Test Command", history);
            var historyList = new System.Collections.Generic.List<string>(history);
            Assert.Equal(2, historyList.Count);
        }

        [Fact]
        public void GetRedoHistory_Should_Return_Command_Names()
        {
            // Arrange
            var command1 = new TestCommand();
            var command2 = new TestCommand();
            _undoRedoService.ExecuteCommand(command1);
            _undoRedoService.ExecuteCommand(command2);
            _undoRedoService.Undo();
            _undoRedoService.Undo();

            // Act
            var history = _undoRedoService.GetRedoHistory();

            // Assert
            Assert.Contains("Test Command", history);
            var historyList = new System.Collections.Generic.List<string>(history);
            Assert.Equal(2, historyList.Count);
        }

        [Fact]
        public void CommandEvents_Should_Be_Raised_Correctly()
        {
            // Arrange
            var command = new TestCommand();
            var stateChangedCount = 0;

            _undoRedoService.StateChanged += (sender, e) =>
            {
                stateChangedCount++;
            };

            // Act
            _undoRedoService.ExecuteCommand(command);
            _undoRedoService.Undo();
            _undoRedoService.Redo();

            // Assert
            Assert.True(stateChangedCount >= 3); // 至少触发3次状态变化
        }
    }
}