﻿using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.SitemapControllerTests
{
    [Trait("Category", "Sitemap Controller Unit Tests")]
    public class SitemapControllerViewTests : BaseSitemapControllerTests
    {
        [Fact]
        public async Task SitemapControllerViewReturnsSuccess()
        {
            // Arrange
            const int resultsCount = 3;
            var expectedResults = A.CollectionOfFake<ContentPageModel>(resultsCount);
            var controller = BuildSitemapController();

            expectedResults[0].IncludeInSitemap = true;
            expectedResults[0].CanonicalName = "default-article";
            expectedResults[1].IncludeInSitemap = false;
            expectedResults[1].CanonicalName = "not-in-sitemap";
            expectedResults[2].IncludeInSitemap = true;
            expectedResults[2].CanonicalName = "in-sitemap";

            A.CallTo(() => FakeContentPageService.GetAllAsync()).Returns(expectedResults);

            // Act
            var result = await controller.SitemapView().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetAllAsync()).MustHaveHappenedOnceExactly();

            var contentResult = Assert.IsType<ContentResult>(result);

            contentResult.ContentType.Should().Be(MediaTypeNames.Application.Xml);

            controller.Dispose();
        }

        [Fact]
        public async Task SitemapControllerViewReturnsSuccessWhenNoData()
        {
            // Arrange
            const int resultsCount = 0;
            var expectedResults = A.CollectionOfFake<ContentPageModel>(resultsCount);
            var controller = BuildSitemapController();

            A.CallTo(() => FakeContentPageService.GetAllAsync()).Returns(expectedResults);

            // Act
            var result = await controller.SitemapView().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetAllAsync()).MustHaveHappenedOnceExactly();

            var contentResult = Assert.IsType<ContentResult>(result);

            contentResult.ContentType.Should().Be(MediaTypeNames.Application.Xml);

            controller.Dispose();
        }
    }
}
