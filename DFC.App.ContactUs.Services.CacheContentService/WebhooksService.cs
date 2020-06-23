﻿using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.CacheContentService
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly IEventMessageService<EmailModel> emailModelEventMessageService;
        private readonly IEmailCacheReloadService emailReloadService;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            IEventMessageService<EmailModel> emailModelEventMessageService,
            IEmailCacheReloadService emailReloadService)
        {
            this.logger = logger;
            this.emailModelEventMessageService = emailModelEventMessageService;
            this.emailReloadService = emailReloadService;
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (!Guid.TryParse(url.Segments.LastOrDefault(), out Guid id))
            {
                throw new InvalidDataException($"Invalid id '{id}' received for Event Id: {eventId}");
            }

            var contentType = url.Segments[url.Segments.Length - 2].Trim('/').ToLowerInvariant();

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    switch (contentType.ToLowerInvariant())
                    {
                        case "email":
                            return await emailModelEventMessageService.DeleteAsync(id).ConfigureAwait(false);
                        default:
                            logger.LogInformation($"{nameof(WebhookCacheOperation.Delete)} Event Id: {eventId} does not require processing in this application");
                            return HttpStatusCode.OK;
                    }

                case WebhookCacheOperation.CreateOrUpdate:
                    switch (contentType.ToLowerInvariant())
                    {
                        case "email":
                            await emailReloadService.ReloadCacheItem(url).ConfigureAwait(false);
                            return HttpStatusCode.OK;
                        default:
                            logger.LogInformation($"{nameof(WebhookCacheOperation.CreateOrUpdate)} Event Id: {eventId} does not require processing in this application");
                            return HttpStatusCode.OK;
                    }

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }
    }
}
