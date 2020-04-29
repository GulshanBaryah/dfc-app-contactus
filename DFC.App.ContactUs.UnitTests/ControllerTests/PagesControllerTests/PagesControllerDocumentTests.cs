using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerDocumentTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<DocumentViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerDocumentJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<DocumentViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerBodyReturnsRedirectWhenAlternateArticleExists(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var expectedAlternativeResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedAlternativeResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeContentPageService.GetByAlternativeNameAsync(A<string>.Ignored)).Returns(expectedAlternativeResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.GetByAlternativeNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<RedirectResult>(result);

            statusResult.Url.Should().NotBeNullOrWhiteSpace();
            A.Equals(true, statusResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeContentPageService.GetByAlternativeNameAsync(A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.GetByAlternativeNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerDocumentReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
