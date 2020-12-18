
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Functions.UnitTests.Services.CollectionCalendar
{
    [TestFixture]
    public class WhenParsingCollectionCalendarQueryString
    {
        private Dictionary<string, string> _queryStringDictionary;
        private string _validationMessage;

        [SetUp]
        public void Arrange()
        {
            _queryStringDictionary = new Dictionary<string, string>();
            _queryStringDictionary.Add("AcademicYear", "2021");
            _queryStringDictionary.Add("PeriodNumber", "1");
            _queryStringDictionary.Add("Active", "true");
        }

        [Test]
        public void Then_no_errors_are_triggered_for_a_valid_query_string()
        {
            var requestObject = CollectionCalendarQueryStringParser.ParseQueryString(_queryStringDictionary, out _validationMessage);

            requestObject.Should().NotBeNull();
            _validationMessage.Length.Should().Be(0);
        }

        [Test]
        public void Then_an_error_is_returned_if_academic_year_missing()
        {
            _queryStringDictionary.Remove("AcademicYear");
            var requestObject = CollectionCalendarQueryStringParser.ParseQueryString(_queryStringDictionary, out _validationMessage);

            requestObject.Should().BeNull();
            _validationMessage.Should().Contain("AcademicYear not set");
        }

        [TestCase("202A")]
        [TestCase("*&*^%^")]
        [TestCase("ABCD")]
        [TestCase("")]
        [TestCase(null)]
        public void Then_an_error_is_returned_if_academic_year_invalid(string academicYearValue)
        {
            _queryStringDictionary["AcademicYear"] = academicYearValue;
            var requestObject = CollectionCalendarQueryStringParser.ParseQueryString(_queryStringDictionary, out _validationMessage);

            requestObject.Should().BeNull();
            _validationMessage.Should().Contain("Invalid value for AcademicYear");
        }

        [Test]
        public void Then_an_error_is_returned_if_period_missing()
        {
            _queryStringDictionary.Remove("PeriodNumber");
            var requestObject = CollectionCalendarQueryStringParser.ParseQueryString(_queryStringDictionary, out _validationMessage);

            requestObject.Should().BeNull();
            _validationMessage.Should().Contain("PeriodNumber not set");
        }

        [TestCase("2A")]
        [TestCase("*&*^%^")]
        [TestCase("ABCD")]
        [TestCase("")]
        [TestCase(null)]
        public void Then_an_error_is_returned_if_period_invalid(string periodValue)
        {
            _queryStringDictionary["PeriodNumber"] = periodValue;
            var requestObject = CollectionCalendarQueryStringParser.ParseQueryString(_queryStringDictionary, out _validationMessage);

            requestObject.Should().BeNull();
            _validationMessage.Should().Contain("Invalid value for PeriodNumber");
        }

        [Test]
        public void Then_an_error_is_returned_if_active_missing()
        {
            _queryStringDictionary.Remove("Active");
            var requestObject = CollectionCalendarQueryStringParser.ParseQueryString(_queryStringDictionary, out _validationMessage);

            requestObject.Should().BeNull();
            _validationMessage.Should().Contain("Active not set");
        }

        [TestCase("A")]
        [TestCase("*&*^%^")]
        [TestCase("1")]
        [TestCase("")]
        [TestCase(null)]
        public void Then_an_error_is_returned_if_active_invalid(string activeValue)
        {
            _queryStringDictionary["Active"] = activeValue;
            var requestObject = CollectionCalendarQueryStringParser.ParseQueryString(_queryStringDictionary, out _validationMessage);

            requestObject.Should().BeNull();
            _validationMessage.Should().Contain("Invalid value for Active");
        }
    }
}
