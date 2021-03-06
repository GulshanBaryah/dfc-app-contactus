﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IApiService
    {
        Task<string?> GetAsync(HttpClient httpClient, Uri url, string acceptHeader);

        Task<string?> GetAsync(HttpClient httpClient, string contentType, string acceptHeader);

        Task<string?> GetAsync(HttpClient httpClient, string contentType, string id, string acceptHeader);
    }
}