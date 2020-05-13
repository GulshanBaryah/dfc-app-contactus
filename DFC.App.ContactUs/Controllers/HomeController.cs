﻿using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace DFC.App.ContactUs.Controllers
{
    public class HomeController : BasePagesController<HomeController>
    {
        private readonly ServiceOpenDetailModel serviceOpenDetailModel;

        public HomeController(ILogger<HomeController> logger, ServiceOpenDetailModel serviceOpenDetailModel) : base(logger)
        {
            this.serviceOpenDetailModel = serviceOpenDetailModel;
        }

        [HttpGet]
        [Route("pages/home")]
        public IActionResult HomeView()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "home",
                BreadcrumbTitle = "contact us",
            };
            var viewModel = new HomeViewModel()
            {
                HtmlHead = new HtmlHeadViewModel
                {
                    CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{LocalPath}", UriKind.RelativeOrAbsolute),
                    Title = "Contact us | National Careers Service",
                },
                Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel),
                HomeBodyViewModel = new HomeBodyViewModel
                {
                    ServiceOpenDetailModel = serviceOpenDetailModel,
                },
            };

            Logger.LogWarning($"{nameof(HomeView)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/home/htmlhead")]
        [Route("pages/htmlhead")]
        public IActionResult HomeHtmlHead()
        {
            var viewModel = new HtmlHeadViewModel()
            {
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}", UriKind.RelativeOrAbsolute),
                Title = "Contact us | National Careers Service",
            };

            Logger.LogInformation($"{nameof(HomeHtmlHead)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/home/breadcrumb")]
        [Route("pages/breadcrumb")]
        public IActionResult HomeBreadcrumb()
        {
            var breadcrumbItemModel = new BreadcrumbItemModel
            {
                CanonicalName = "home",
                BreadcrumbTitle = "Contact us",
            };
            var viewModel = BuildBreadcrumb(RegistrationPath, breadcrumbItemModel);

            Logger.LogInformation($"{nameof(HomeBreadcrumb)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/home/body")]
        [Route("pages/body")]
        public IActionResult HomeBody()
        {
            var viewModel = new HomeBodyViewModel()
            {
                ServiceOpenDetailModel = serviceOpenDetailModel,
            };

            Logger.LogInformation($"{nameof(HomeBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpPost]
        [Route("pages/home/body")]
        [Route("pages/body")]
        public IActionResult HomeBody(HomeBodyViewModel? viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                switch (viewModel.SelectedOption)
                {
                    case HomeBodyViewModel.SelectOption.Webchat:
                        return Redirect($"/{WebchatRegistrationPath}/chat");
                    case HomeBodyViewModel.SelectOption.SendAMessage:
                        return Redirect($"/{RegistrationPath}/why-do-you-want-to-contact-us");
                    case HomeBodyViewModel.SelectOption.Callback:
                        return Redirect($"/{RegistrationPath}/request-callback");
                    case HomeBodyViewModel.SelectOption.Sendletter:
                        return Redirect($"/{RegistrationPath}/send-us-a-letter");
                }
            }

            viewModel = new HomeBodyViewModel()
            {
                ServiceOpenDetailModel = serviceOpenDetailModel,
            };

            Logger.LogInformation($"{nameof(HomeBody)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
