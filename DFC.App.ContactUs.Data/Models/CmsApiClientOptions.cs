﻿namespace DFC.App.ContactUs.Data.Models
{
    public class CmsApiClientOptions : ClientOptionsModel
    {
        public string SummaryEndpoint { get; set; } = "content/getcontent/api/execute/page/";
    }
}
