using System;
using NUnit.Framework;
using WeSketch;

namespace WeSketch.Tests
{
    /// <summary>
    /// Cleans Up Data Objects Before and After Tests
    /// </summary>
    [SetUpFixture]
    public class StartupTests
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var db = new WeSketchDataContext();
            db.CleanTestingObjects();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            var db = new WeSketchDataContext();
            db.CleanTestingObjects();
        }
    }
}
