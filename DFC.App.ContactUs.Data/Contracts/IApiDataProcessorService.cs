﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IApiDataProcessorService
    {
        Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, Uri url)
            where TApiModel : class;

        Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, string contentType, string id)
          where TApiModel : class;

        Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, string contentType)
          where TApiModel : class;
    }
}