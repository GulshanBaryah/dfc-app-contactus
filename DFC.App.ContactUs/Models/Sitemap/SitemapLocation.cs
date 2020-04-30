﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DFC.App.ContactUs.Models
{
    public class SitemapLocation
    {
        public enum ChangeFrequencies
        {
            Always,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Never,
        }

        [XmlElement("loc")]
        public string? Url { get; set; }

        [XmlElement("changefreq")]
        public ChangeFrequencies? ChangeFrequency { get; set; }

        [XmlElement("lastmod")]
        public DateTime? LastModified { get; set; }

        [XmlElement("priority")]
        public double? Priority { get; set; }

        [XmlElement("image", Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
        public List<SitemapImage>? Images { get; set; }

        public bool ShouldSerializeChangeFrequency()
        {
            return ChangeFrequency.HasValue;
        }

        public bool ShouldSerializeLastModified()
        {
            return LastModified.HasValue;
        }

        public bool ShouldSerializePriority()
        {
            return Priority.HasValue;
        }

        public bool ShouldSerializeImages()
        {
            return Images != null && Images.Count > 0;
        }
    }
}
