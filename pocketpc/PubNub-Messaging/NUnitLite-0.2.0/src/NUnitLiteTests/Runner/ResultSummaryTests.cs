// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;
using NUnitLite.Tests;

namespace NUnitLite.Runner.Tests
{
    [TestFixture]
    public class ResultSummaryTests : TestCase
    {
        public ResultSummaryTests(string name) : base(name) { }

        public void testSummarizeEmptySuite()
        {
            TestResult result = new TestResult( new TestSuite("Empty Suite") );
            CreateSummaryAndVerify(result, 0, 0, 0, 0);
        }

        public void testSummarizeSingleTestCase()
        {
            TestResult result = new TestResult(new DummyTestCase( "TheTest" ));
            result.Success();
            CreateSummaryAndVerify(result, 1, 0, 0, 0);
        }

        public void testSummarizeErrorTests()
        {
            CreateSummaryAndVerify("EEEEE", 5, 5, 0, 0);
        }

        public void testSummarizeFailedTests()
        {
            CreateSummaryAndVerify("FFFFF", 5, 0, 5, 0);
        }

        public void testSummarizeNotRunTests()
        {
            CreateSummaryAndVerify("NNNNN", 5, 0, 0, 5);
        }

        public void testSummarizeSuccessTests()
        {
            CreateSummaryAndVerify("SSSSS", 5, 0, 0, 0);
        }

        public void testSummarizeTestsWithMixedResults()
        {
            CreateSummaryAndVerify("SNSSFSESFS", 10, 1, 2, 1);
        }

        #region Helper Methods
        private TestResult CreateResults(string recipe)
        {
            TestResult result = new TestResult(new TestSuite("From Recipe"));

            foreach (char c in recipe)
            {
                TestResult r = new TestResult(new TestCase("test"));
                switch (c)
                {
                    case 'S':
                        r.Success();
                        break;
                    case 'F':
                        r.Failure("failed!");
                        break;
                    case 'E':
                        r.Error(new Exception("error!"));
                        break;
                    default:
                        break;
                }

                result.AddResult(r);
            }

            if (recipe.IndexOfAny(new char[] { 'E', 'F' }) >= 0)
                result.Failure("Errors");
            else
                result.Success();

            return result;
        }

        private void CreateSummaryAndVerify(string recipe, int tests, int errors, int failures, int notrun)
        {
            CreateSummaryAndVerify( CreateResults(recipe), tests, errors, failures, notrun);
        }

        private void CreateSummaryAndVerify(TestResult result, int tests, int errors, int failures, int notrun)
        {
            ResultSummary summary = new ResultSummary(result);
            Assert.That(summary.TestCount, Is.EqualTo(tests));
            Assert.That(summary.ErrorCount, Is.EqualTo(errors));
            Assert.That(summary.FailureCount, Is.EqualTo(failures));
            Assert.That(summary.NotRunCount, Is.EqualTo(notrun));
        }
        #endregion
    }
}
