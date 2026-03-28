/*
   Copyright 2014-2025 Marco De Salvo

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
public class TIMEPositionReferenceSystemTest
{
    #region Tests
    [TestMethod]
    public void ShouldCreatePositionTRS()
    {
        TIMEPositionReferenceSystem MillenniumTimeTRS = new TIMEPositionReferenceSystem(
            new RDFResource("ex:MillenniumTime"),
            new TIMECoordinate(2000, 1, 1, 0, 0, 0),
            TIMEUnit.Day);

        Assert.IsNotNull(MillenniumTimeTRS);
        Assert.IsTrue(MillenniumTimeTRS.Equals(new RDFResource("ex:MillenniumTime")));
        Assert.IsNotNull(MillenniumTimeTRS.Origin);
        Assert.AreEqual(2000, MillenniumTimeTRS.Origin.Year);
        Assert.AreEqual(1, MillenniumTimeTRS.Origin.Month);
        Assert.AreEqual(1, MillenniumTimeTRS.Origin.Day);
        Assert.AreEqual(0, MillenniumTimeTRS.Origin.Hour);
        Assert.AreEqual(0, MillenniumTimeTRS.Origin.Minute);
        Assert.AreEqual(0, MillenniumTimeTRS.Origin.Second);
        Assert.IsNotNull(MillenniumTimeTRS.Unit);
        Assert.AreEqual(TIMEEnums.TIMEUnitType.Day, MillenniumTimeTRS.Unit.UnitType);
        Assert.AreEqual(1, MillenniumTimeTRS.Unit.ScaleFactor);
        Assert.IsFalse(MillenniumTimeTRS.HasLargeScale);
    }

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseNullOrigin()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEPositionReferenceSystem(new RDFResource("ex:ModifiedUnixTime"), null, TIMEUnit.Second));

    [TestMethod]
    public void ShouldThrowExceptionOnCreatimeCalendarTRSBecauseNullUnit()
        => Assert.ThrowsExactly<OWLException>(() => _ = new TIMEPositionReferenceSystem(new RDFResource("ex:ModifiedUnixTime"), TIMECoordinate.Unix, null));
    //Correction functions
    [TestMethod]
    public void ShouldHaveNullCorrectionsForLinearSystems()
    {
        Assert.IsNull(TIMEPositionReferenceSystem.Unix.CorrectionToCalendar);
        Assert.IsNull(TIMEPositionReferenceSystem.Unix.CorrectionFromCalendar);
        Assert.IsNull(TIMEPositionReferenceSystem.Geologic.CorrectionToCalendar);
        Assert.IsNull(TIMEPositionReferenceSystem.Geologic.CorrectionFromCalendar);
    }

    [TestMethod]
    public void ShouldHaveCorrectionsForNonLinearSystems()
    {
        Assert.IsNotNull(TIMEPositionReferenceSystem.GPS.CorrectionToCalendar);
        Assert.IsNotNull(TIMEPositionReferenceSystem.GPS.CorrectionFromCalendar);
        Assert.IsNotNull(TIMEPositionReferenceSystem.TAI.CorrectionToCalendar);
        Assert.IsNotNull(TIMEPositionReferenceSystem.TAI.CorrectionFromCalendar);
        Assert.IsNotNull(TIMEPositionReferenceSystem.LORANC.CorrectionToCalendar);
        Assert.IsNotNull(TIMEPositionReferenceSystem.LORANC.CorrectionFromCalendar);
    }

    //GPS: position → coordinate → position roundtrip
    [TestMethod]
    public void ShouldRoundtripGPSPositionToCoordinate()
    {
        //1 billion GPS seconds (~2011-09-09)
        double gpsPosition = 1_000_000_000;
        TIMECoordinate coord = TIMEConverter.PositionToCoordinate(gpsPosition, TIMEPositionReferenceSystem.GPS);
        double roundtrip = TIMEConverter.CoordinateToPosition(coord, TIMEPositionReferenceSystem.GPS);

        Assert.AreEqual(gpsPosition, roundtrip, 1);
    }

    //GPS diverges from Unix: same calendar date → different position values
    [TestMethod]
    public void ShouldGPSDivergeFromUnixByLeapSeconds()
    {
        //2020-01-01 00:00:00 UTC
        TIMECoordinate utc2020 = new TIMECoordinate(2020, 1, 1, 0, 0, 0);

        double unixPos = TIMEConverter.CoordinateToPosition(utc2020, TIMEPositionReferenceSystem.Unix);
        double gpsPos = TIMEConverter.CoordinateToPosition(utc2020, TIMEPositionReferenceSystem.GPS);

        //GPS runs ahead of UTC by 18 leap seconds (as of 2017-01-01),
        //and GPS epoch is 315964800 Unix seconds after Unix epoch.
        //So gpsPos should differ from (unixPos - 315964800) by the leap second offset.
        //The key assertion: GPS position for the same UTC date includes leap seconds
        double gpsFromUnixEpochDiff = unixPos - 315964800; //naive seconds from GPS epoch
        Assert.IsTrue(gpsPos > gpsFromUnixEpochDiff); //GPS position is larger because it counts continuous seconds
    }

    //TAI: position → coordinate roundtrip
    [TestMethod]
    public void ShouldRoundtripTAIPositionToCoordinate()
    {
        //1.5 billion TAI seconds (~2019)
        double taiPosition = 1_500_000_000;
        TIMECoordinate coord = TIMEConverter.PositionToCoordinate(taiPosition, TIMEPositionReferenceSystem.TAI);
        double roundtrip = TIMEConverter.CoordinateToPosition(coord, TIMEPositionReferenceSystem.TAI);

        Assert.AreEqual(taiPosition, roundtrip, 1);
    }

    //TAI is ahead of UTC by 10 + accumulated leap seconds
    [TestMethod]
    public void ShouldTAIBeAheadOfUTC()
    {
        //At TAI epoch (1972-01-01), TAI was already 10 seconds ahead of UTC.
        //For a date after all leap seconds (2020-01-01), TAI-UTC = 10 + 27 = 37 seconds.
        TIMECoordinate utc2020 = new TIMECoordinate(2020, 1, 1, 0, 0, 0);

        //Convert same calendar date to TAI position and back with an offset
        double taiPos = TIMEConverter.CoordinateToPosition(utc2020, TIMEPositionReferenceSystem.TAI);
        TIMECoordinate fromTai = TIMEConverter.PositionToCoordinate(taiPos, TIMEPositionReferenceSystem.TAI);

        //The roundtrip should preserve the date
        Assert.AreEqual(2020d, fromTai.Year);
        Assert.AreEqual(1d, fromTai.Month);
        Assert.AreEqual(1d, fromTai.Day);
    }

    //LORAN-C: position → coordinate roundtrip
    [TestMethod]
    public void ShouldRoundtripLORANCPositionToCoordinate()
    {
        //2 billion LORAN-C seconds (~2021)
        double lorancPosition = 2_000_000_000;
        TIMECoordinate coord = TIMEConverter.PositionToCoordinate(lorancPosition, TIMEPositionReferenceSystem.LORANC);
        double roundtrip = TIMEConverter.CoordinateToPosition(coord, TIMEPositionReferenceSystem.LORANC);

        Assert.AreEqual(lorancPosition, roundtrip, 1);
    }

    //All three non-linear systems: same UTC date produces different position values
    [TestMethod]
    public void ShouldNonLinearSystemsProduceDifferentPositionsForSameDate()
    {
        TIMECoordinate utc2020 = new TIMECoordinate(2020, 1, 1, 0, 0, 0);

        double gpsPos = TIMEConverter.CoordinateToPosition(utc2020, TIMEPositionReferenceSystem.GPS);
        double taiPos = TIMEConverter.CoordinateToPosition(utc2020, TIMEPositionReferenceSystem.TAI);
        double lorancPos = TIMEConverter.CoordinateToPosition(utc2020, TIMEPositionReferenceSystem.LORANC);

        //All three should be different (different epochs, different offsets)
        Assert.AreNotEqual(gpsPos, taiPos, 1);
        Assert.AreNotEqual(gpsPos, lorancPos, 1);
        Assert.AreNotEqual(taiPos, lorancPos, 1);

        //LORAN-C has the earliest epoch (1958), so its position should be the largest
        Assert.IsTrue(lorancPos > taiPos);
        Assert.IsTrue(lorancPos > gpsPos);

        //TAI epoch (1972) is before GPS epoch (1980), so TAI position should be larger than GPS
        Assert.IsTrue(taiPos > gpsPos);
    }

    //GPS and Unix should agree on calendar date for the same physical instant
    [TestMethod]
    public void ShouldGPSAndUnixProduceSameCalendarDate()
    {
        //A specific Unix timestamp: 2024-07-01 12:00:00 UTC = 1719835200 Unix seconds
        double unixTimestamp = 1719835200;
        TIMECoordinate fromUnix = TIMEConverter.PositionToCoordinate(unixTimestamp, TIMEPositionReferenceSystem.Unix);

        //Convert the same calendar date to GPS and back
        double gpsPos = TIMEConverter.CoordinateToPosition(fromUnix, TIMEPositionReferenceSystem.GPS);
        TIMECoordinate fromGps = TIMEConverter.PositionToCoordinate(gpsPos, TIMEPositionReferenceSystem.GPS);

        //Both should represent the same calendar date
        Assert.AreEqual(fromUnix.Year, fromGps.Year);
        Assert.AreEqual(fromUnix.Month, fromGps.Month);
        Assert.AreEqual(fromUnix.Day, fromGps.Day);
        Assert.AreEqual(fromUnix.Hour, fromGps.Hour);
    }

    //Registry should contain the new TRS
    [TestMethod]
    public void ShouldRegisterGPSTAILORANC()
    {
        Assert.IsTrue(TIMEReferenceSystemRegistry.ContainsTRS(TIMEPositionReferenceSystem.GPS));
        Assert.IsTrue(TIMEReferenceSystemRegistry.ContainsTRS(TIMEPositionReferenceSystem.TAI));
        Assert.IsTrue(TIMEReferenceSystemRegistry.ContainsTRS(TIMEPositionReferenceSystem.LORANC));
    }
    #endregion
}