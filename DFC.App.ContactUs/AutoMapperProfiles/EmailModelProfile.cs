﻿using AutoMapper;
using DFC.App.ContactUs.Data.Models;
using System;
using System.Linq;

namespace DFC.App.ContactUs.AutoMapperProfiles
{
    public class EmailModelProfile : Profile
    {
        public EmailModelProfile()
        {
            CreateMap<EmailApiDataModel, EmailModel>()
               .ForMember(d => d.Id, s => s.MapFrom(a => Guid.Parse(a.Url!.Segments.Last())))
               .ForMember(d => d.Body, s => s.MapFrom(a => a.Body))
               .ForMember(d => d.Etag, s => s.Ignore())
               .ForMember(d => d.ParentId, s => s.Ignore())
               .ForMember(d => d.TraceId, s => s.Ignore())
               .ForMember(d => d.PartitionKey, s => s.Ignore());
        }
    }
}