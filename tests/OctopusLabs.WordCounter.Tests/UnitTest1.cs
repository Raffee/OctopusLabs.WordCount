using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OctopusLabs.WordCounter.Services;

namespace OctopusLabs.WordCounter.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private TextSplitterService textSplitterService;
        private Mock<ITextReaderService> textReaderServiceMock;

        //[TestInitialize]
        //public void Init()
        //{
        //    textReaderServiceMock = new Mock<ITextReaderService>();
        //    textSplitterService = new TextSplitterService(textReaderServiceMock.Object);
        //}

        [TestMethod]
        public async void Words_Should_Be_Ordered_Decreasing_By_Count()
        {
            {
                //Arrange
                const string sampleUrl = "https://www.google.com";
                const string sampleText = "One occurance of This, but Three occurances of occurance, yes, occurance.";
                textReaderServiceMock.Setup(service => service.GetTextFromSource(sampleUrl)).ReturnsAsync(sampleText);

                //Act
                var countedWords = await textSplitterService.GetWordsFromUrlAsync(sampleUrl, 2);

                //Assert
                Assert.AreEqual("occurance", countedWords[0].Key);
                Assert.AreEqual("of", countedWords[1].Key);
            }
        }

        [TestMethod]
        public async void Words_Count_Should_Be_Correct()
        {
            {
                //Arrange
                const string sampleUrl = "https://www.google.com";
                const string sampleText = "One occurance of This, but Three occurances of occurance, yes, occurance.";
                textReaderServiceMock.Setup(service => service.GetTextFromSource(sampleUrl)).ReturnsAsync(sampleText);

                //Act
                var countedWords = await textSplitterService.GetWordsFromUrlAsync(sampleUrl, 2);

                //Assert
                Assert.AreEqual(3, countedWords[0].Value);
                Assert.AreEqual(2, countedWords[1].Value);
            }
        }
    }
}
