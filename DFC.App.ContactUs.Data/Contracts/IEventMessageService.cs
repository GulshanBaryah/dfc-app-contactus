﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IEventMessageService<TModel>
        where TModel : class
    {
        Task<IList<TModel>?> GetAllCachedCanonicalNamesAsync();

        Task<HttpStatusCode> CreateAsync(TModel? upsertDocumentModel);

        Task<HttpStatusCode> UpdateAsync(TModel? upsertDocumentModel);

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}