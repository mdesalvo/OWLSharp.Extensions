/*
   Copyright 2014-2026 Marco De Salvo

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OWLSharp.Extensions.TIME;
using RDFSharp.Model;

namespace OWLSharp.Extensions.Test.TIME;

[TestClass]
public class TIMECalendarReferenceSystemTest
{
    #region Tests
    [TestMethod]
    public void ShouldCreateCalendarTRS()
    {
        TIMECalendarReferenceSystem ModifiedGregorianTRS = new TIMECalendarReferenceSystem(
            new RDFResource("ex:ModifiedGregorian"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 24, 7, [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30]));

        Assert.IsNotNull(ModifiedGregorianTRS);
        Assert.IsTrue(ModifiedGregorianTRS.Equals(new RDFResource("ex:ModifiedGregorian")));
        Assert.IsNotNull(ModifiedGregorianTRS.Metrics);
        Assert.IsTrue(ModifiedGregorianTRS.Metrics.HasExactMetric);
        Assert.AreEqual(60u, ModifiedGregorianTRS.Metrics.SecondsInMinute);
        Assert.AreEqual(60u, ModifiedGregorianTRS.Metrics.MinutesInHour);
        Assert.AreEqual(24u, ModifiedGregorianTRS.Metrics.HoursInDay);
        Assert.AreEqual(12u, ModifiedGregorianTRS.Metrics.MonthsInYear);
        Assert.AreEqual(360u, ModifiedGregorianTRS.Metrics.DaysInYear);
    }

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseNullMetrics()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMECalendarReferenceSystem(new RDFResource("ex:ModifiedGregorian"), null));

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseInvalidMetrics1()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMECalendarReferenceSystem(new RDFResource("ex:ModifiedGregorian"),
            new TIMECalendarReferenceSystemMetrics(0, 60, 24, 7, [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30])));

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseInvalidMetrics2()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMECalendarReferenceSystem(new RDFResource("ex:ModifiedGregorian"),
            new TIMECalendarReferenceSystemMetrics(60, 0, 24, 7, [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30])));

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseInvalidMetrics3()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMECalendarReferenceSystem(new RDFResource("ex:ModifiedGregorian"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 0, 7, [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30])));

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseInvalidMetrics4()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMECalendarReferenceSystem(new RDFResource("ex:ModifiedGregorian"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 24, 7, null)));

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseInvalidMetrics5()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMECalendarReferenceSystem(new RDFResource("ex:ModifiedGregorian"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 24, 7, [])));

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseInvalidMetrics6()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMECalendarReferenceSystem(new RDFResource("ex:ModifiedGregorian"),
            new TIMECalendarReferenceSystemMetrics(60, 60, 24, 7, [30, 30, 0, 31])));
    //SecondsPerDay
    [TestMethod]
    public void ShouldHaveCorrectSecondsPerDay()
    {
        Assert.AreEqual(86400, TIMECalendarReferenceSystem.Gregorian.Metrics.SecondsPerDay);
        Assert.AreEqual(86400, TIMECalendarReferenceSystem.Julian.Metrics.SecondsPerDay);
    }

    [TestMethod]
    public void ShouldHaveCustomSecondsPerDay()
    {
        //A calendar with 100 seconds/minute, 100 minutes/hour, 10 hours/day
        TIMECalendarReferenceSystemMetrics metrics = new TIMECalendarReferenceSystemMetrics(100, 100, 10, 5, [30, 30, 30]);
        Assert.AreEqual(100000, metrics.SecondsPerDay);
    }

    #endregion
}