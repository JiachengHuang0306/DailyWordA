using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DailyWordA.Library.Models;
using DailyWordA.Library.Services;
using DailyWordA.Library.ViewModels;

namespace DailyWordA.UnitTest.ViewModels
{
    public class MemoViewModelTest
    {
        private readonly Mock<IMemoStorage> _mockMemoStorage;
        private readonly MemoViewModel _viewModel;

        public MemoViewModelTest()
        {
            _mockMemoStorage = new Mock<IMemoStorage>();
            _viewModel = new MemoViewModel(_mockMemoStorage.Object);
        }

        [Fact]
        public async Task AddMemoAsync_ValidContent_AddsMemo()
        {
            // Arrange
            var testDate = DateTime.Now;
            _viewModel.SelectedDate = testDate;
            _viewModel.NewMemoContent = "Test Memo";

            // Act
            await _viewModel.AddMemoCommand.ExecuteAsync(null);

            // Assert
            _mockMemoStorage.Verify(
                m => m.SaveMemoAsync(It.Is<MemoObject>(memo =>
                    memo.Date == testDate.ToString("yyyy-MM-dd") &&
                    memo.Content == "Test Memo")),
                Times.Once);
            Assert.Equal(string.Empty, _viewModel.NewMemoContent);
        }

        [Fact]
        public async Task AddMemoAsync_EmptyContent_DoesNotAddMemo()
        {
            // Arrange
            _viewModel.NewMemoContent = string.Empty;

            // Act
            await _viewModel.AddMemoCommand.ExecuteAsync(null);

            // Assert
            _mockMemoStorage.Verify(m => m.SaveMemoAsync(It.IsAny<MemoObject>()), Times.Never);
        }

        [Fact]
        public async Task LoadMemosAsync_LoadsMemosFromStorage()
        {
            // Arrange
            var testDate = DateTime.Now;
            var testMemos = new[]
            {
                new MemoObject { Id = 1, Date = testDate.ToString("yyyy-MM-dd"), Content = "Memo 1" },
                new MemoObject { Id = 2, Date = testDate.ToString("yyyy-MM-dd"), Content = "Memo 2" }
            };

            _mockMemoStorage
                .Setup(m => m.GetMemosByDateAsync(testDate))
                .ReturnsAsync(testMemos);

            _viewModel.SelectedDate = testDate;

            // Act
            await _viewModel.LoadMemosCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal(2, _viewModel.MemoList.Count);
            Assert.Contains(_viewModel.MemoList, m => m.Content == "Memo 1");
            Assert.Contains(_viewModel.MemoList, m => m.Content == "Memo 2");
        }

        [Fact]
        public async Task DeleteMemoAsync_ValidMemo_RemovesMemo()
        {
            // Arrange
            var testMemo = new MemoObject { Id = 1, Content = "Test Memo" };
            _viewModel.MemoList.Add(testMemo);

            // Act
            await _viewModel.DeleteMemoCommand.ExecuteAsync(testMemo);

            // Assert
            _mockMemoStorage.Verify(m => m.DeleteMemoAsync(1), Times.Once);
            Assert.DoesNotContain(testMemo, _viewModel.MemoList);
        }

        
        
        [Fact]
        public void SelectedMemo_UpdatesNewMemoContent()
        {
            // Arrange
            var testMemo = new MemoObject { Content = "Test Content" };

            // Act
            _viewModel.SelectedMemo = testMemo;

            // Assert
            Assert.Equal("Test Content", _viewModel.NewMemoContent);
        }
    }
}