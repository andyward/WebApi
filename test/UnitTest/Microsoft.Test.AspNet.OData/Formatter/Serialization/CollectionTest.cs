﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.Test.AspNet.OData.TestCommon.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.Test.AspNet.OData.Formatter.Serialization
{
    public class CollectionTest
    {
        private readonly ODataMediaTypeFormatter _formatter;

        public CollectionTest()
        {
            _formatter = new ODataMediaTypeFormatter(new ODataPayloadKind[] { ODataPayloadKind.Collection });
            _formatter.Request = GetSampleRequest();
            _formatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));
            _formatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationXml));
        }

        [Fact]
        public async Task ArrayOfIntsSerializesAsOData()
        {
            // Arrange
            ObjectContent<int[]> content = new ObjectContent<int[]>(new int[] { 10, 20, 30, 40, 50 }, _formatter,
                MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(Resources.ArrayOfInt32, await content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ArrayOfBooleansSerializesAsOData()
        {
            // Arrange
            ObjectContent<bool[]> content = new ObjectContent<bool[]>(new bool[] { true, false, true, false },
                _formatter, MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(Resources.ArrayOfBoolean, await content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ListOfStringsSerializesAsOData()
        {
            // Arrange
            List<string> listOfStrings = new List<string>();
            listOfStrings.Add("Frank");
            listOfStrings.Add("Steve");
            listOfStrings.Add("Tom");
            listOfStrings.Add("Chandler");

            ObjectContent<List<string>> content = new ObjectContent<List<string>>(listOfStrings, _formatter,
                MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(Resources.ListOfString, await content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ListOfDatesSerializesAsOData()
        {
            // Arrange
            const string expect =
                "{" +
                    "\"@odata.context\":\"http://localhost/$metadata#Collection(Edm.Date)\",\"value\":[" +
                    "\"0001-01-01\",\"2015-02-26\",\"9999-12-31\"" +
                    "]" +
                "}";

            List<Date> listOfDates = new List<Date>();
            listOfDates.Add(Date.MinValue);
            listOfDates.Add(new Date(2015, 2, 26));
            listOfDates.Add(Date.MaxValue);

            ObjectContent<List<Date>> content = new ObjectContent<List<Date>>(listOfDates, _formatter,
                MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(expect, await content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ListOfNullableDatesSerializesAsOData()
        {
            // Arrange
            const string expect =
                "{" +
                    "\"@odata.context\":\"http://localhost/$metadata#Collection(Edm.Date)\",\"value\":[" +
                    "\"0001-01-01\",\"2015-02-26\",null,\"9999-12-31\"" +
                    "]" +
                "}";

            List<Date?> listOfDates = new List<Date?>();
            listOfDates.Add(Date.MinValue);
            listOfDates.Add(new Date(2015, 2, 26));
            listOfDates.Add(null);
            listOfDates.Add(Date.MaxValue);

            ObjectContent<List<Date?>> content = new ObjectContent<List<Date?>>(listOfDates, _formatter,
                MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(expect, await content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ListOfTimeOfDaysSerializesAsOData()
        {
            // Arrange
            const string expect =
                "{" +
                    "\"@odata.context\":\"http://localhost/$metadata#Collection(Edm.TimeOfDay)\",\"value\":[" +
                    "\"00:00:00.0000000\",\"01:02:03.0040000\",\"23:59:59.9999999\"" +
                    "]" +
                "}";

            List<TimeOfDay> listOfDates = new List<TimeOfDay>();
            listOfDates.Add(TimeOfDay.MinValue);
            listOfDates.Add(new TimeOfDay(1, 2, 3, 4));
            listOfDates.Add(TimeOfDay.MaxValue);

            ObjectContent<List<TimeOfDay>> content = new ObjectContent<List<TimeOfDay>>(listOfDates, _formatter,
                MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(expect, await content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ListOfNullableTimeOfDaysSerializesAsOData()
        {
            // Arrange
            const string expect =
                "{" +
                    "\"@odata.context\":\"http://localhost/$metadata#Collection(Edm.TimeOfDay)\",\"value\":[" +
                    "\"00:00:00.0000000\",\"01:02:03.0040000\",null,\"23:59:59.9999999\"" +
                    "]" +
                "}";

            List<TimeOfDay?> listOfDates = new List<TimeOfDay?>();
            listOfDates.Add(TimeOfDay.MinValue);
            listOfDates.Add(new TimeOfDay(1, 2, 3, 4));
            listOfDates.Add(null);
            listOfDates.Add(TimeOfDay.MaxValue);

            ObjectContent<List<TimeOfDay?>> content = new ObjectContent<List<TimeOfDay?>>(listOfDates, _formatter,
                MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(expect, await content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ListOfDateTimeSerializesAsOData()
        {
            // Arrange
            DateTime dt1 = new DateTime(1978, 11, 15, 01, 12, 13, DateTimeKind.Local);
            DateTime dt2 = new DateTime(2014, 10, 27, 12, 25, 26, DateTimeKind.Local);
            List<DateTime> listOfDateTime = new List<DateTime> { dt1, dt2 };

            ObjectContent<List<DateTime>> content = new ObjectContent<List<DateTime>>(listOfDateTime,
                _formatter, MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            dynamic result = JObject.Parse(await content.ReadAsStringAsync());

            Assert.Equal(2, result["value"].Count);
            DateTimeOffset dto = (DateTimeOffset)result["value"][0];
            Assert.Equal(new DateTimeOffset(dt1), dto);

            dto = (DateTimeOffset)result["value"][1];
            Assert.Equal(new DateTimeOffset(dt2), dto);
        }

        [Fact]
        public async Task ListOfNullableDateTimeSerializesAsOData()
        {
            // Arrange
            DateTime dt1 = new DateTime(1978, 11, 15, 01, 12, 13, DateTimeKind.Local);
            DateTime dt2 = new DateTime(2014, 10, 27, 12, 25, 26, DateTimeKind.Local);
            List<DateTime?> listOfDateTime = new List<DateTime?> { dt1, null, dt2 };

            ObjectContent<List<DateTime?>> content = new ObjectContent<List<DateTime?>>(listOfDateTime,
                _formatter, MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            dynamic result = JObject.Parse(await content.ReadAsStringAsync());

            Assert.Equal(3, result["value"].Count);
            DateTimeOffset? dto = (DateTimeOffset?)result["value"][0];
            Assert.Equal(new DateTimeOffset(dt1), dto.Value);

            dto = (DateTimeOffset?)result["value"][1];
            Assert.Null(dto);

            dto = (DateTimeOffset?)result["value"][2];
            Assert.Equal(new DateTimeOffset(dt2), dto.Value);
        }

        [Fact]
        public async Task ListOfDateTimeSerializesAsOData_CustomTimeZone()
        {
            // Arrange
            const string expect =
                "{" +
                    "\"@odata.context\":\"http://localhost/$metadata#Collection(Edm.DateTimeOffset)\",\"value\":[" +
                    "\"1978-11-14T17:12:13-08:00\",\"2014-10-27T04:25:26-08:00\"" +
                    "]" +
                "}";

            List<DateTime> listOfDateTime = new List<DateTime>();
            listOfDateTime.Add(new DateTime(1978, 11, 15, 01, 12, 13, DateTimeKind.Utc));
            listOfDateTime.Add(new DateTime(2014, 10, 27, 12, 25, 26, DateTimeKind.Utc));

            _formatter.Request.GetConfiguration()
                .SetTimeZoneInfo(TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            ObjectContent<List<DateTime>> content = new ObjectContent<List<DateTime>>(listOfDateTime,
                _formatter, MediaTypeHeaderValue.Parse(ODataMediaTypes.ApplicationJsonODataMinimalMetadata));

            // Act & Assert
            JsonAssert.Equal(expect, await content.ReadAsStringAsync());
        }

        private static HttpRequestMessage GetSampleRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/property");
            request.EnableODataDependencyInjectionSupport();
            request.GetConfiguration().Routes.MapFakeODataRoute();
            return request;
        }

        private static IEdmModel GetSampleModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.ComplexType<Person>();

            // Employee is derived from Person. Employee has a property named manager it's Employee type.
            // It's not allowed to build inheritance complex type because a recursive loop of complex types is not allowed.
            builder.Ignore<Employee>();
            return builder.GetEdmModel();
        }
    }
}