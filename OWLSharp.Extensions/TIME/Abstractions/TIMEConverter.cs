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

using RDFSharp.Model;
using System;
using System.Linq;

namespace OWLSharp.Extensions.TIME
{
    /// <summary>
    /// TIMEConverter is a utility class that provides conversion and normalization operations for temporal entities
    /// within the OWL-TIME framework. It enables bidirectional transformations between positional time representations
    /// (numeric values in a TRS) and coordinate-based representations (decomposed calendar components),
    /// handles temporal extent calculations from durations, and ensures coordinate normalization according to specific
    /// calendar system metrics. The class acts as the computational engine for temporal arithmetic and coordinate
    /// manipulation across different temporal reference systems.
    /// </summary>
    public static class TIMEConverter
    {
        #region Methods
        /// <summary>
        /// Converts a numeric time position (expressed in a positional TRS) into a calendar coordinate representation (expressed in a calendar TRS),
        /// handling both large-scale (year-level) and little-scale (second-level) temporal granularities through appropriate scaling and clock emulation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static TIMECoordinate PositionToCoordinate(double timePosition, TIMEPositionReferenceSystem positionTRS, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (positionTRS == null)
                throw new OWLException($"Cannot convert position to coordinate because given '{nameof(positionTRS)}' parameter is null");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            //Normalize origin of the given positional TRS according to the metrics of the given calendar TRS
            TIMECoordinate coordinate = NormalizeCoordinate(positionTRS.Origin, calendarTRS);

            //Scale the given time position of the factor specified by the unit of the given position TRS
            double scaledTimePosition = timePosition * positionTRS.Unit.ScaleFactor;

            #region Large-Scale
            if (positionTRS.HasLargeScale)
            {
                coordinate.Metadata = new TIMECoordinateMetadata(calendarTRS, RDFVocabulary.TIME.UNIT_YEAR);

                //Transform the scaled time position to years (since the large scale works at this level of detail)
                double timePositionYears =
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Second ? scaledTimePosition / calendarTRS.Metrics.SecondsInMinute / calendarTRS.Metrics.MinutesInHour / calendarTRS.Metrics.HoursInDay  / (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Minute ? scaledTimePosition / calendarTRS.Metrics.MinutesInHour   / calendarTRS.Metrics.HoursInDay    / (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Hour   ? scaledTimePosition / calendarTRS.Metrics.HoursInDay      / (calendarTRS.Metrics.DaysInYear   / calendarTRS.Metrics.MonthsInYear) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Day    ? scaledTimePosition / (calendarTRS.Metrics.DaysInYear     / calendarTRS.Metrics.MonthsInYear) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Month  ? scaledTimePosition / calendarTRS.Metrics.MonthsInYear :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Year   ? scaledTimePosition :
                    0;

                //Set year component and reset the others (unuseful at large scale level)
                coordinate.Year = Math.Truncate(positionTRS.Origin.Year.Value + timePositionYears);
                coordinate.Month = null;
                coordinate.Day = null;
                coordinate.Hour = null;
                coordinate.Minute = null;
                coordinate.Second = null;
            }
            #endregion

            #region Little-Scale
            else
            {
                coordinate.Metadata = new TIMECoordinateMetadata(calendarTRS, RDFVocabulary.TIME.UNIT_SECOND);

                //Transform the scaled time position to seconds (since the clock emulator works at this level of detail)
                double scaledTimePositionSeconds =
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Second ? scaledTimePosition :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Minute ? scaledTimePosition * calendarTRS.Metrics.SecondsInMinute :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Hour   ? scaledTimePosition * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Day    ? scaledTimePosition * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Month  ? scaledTimePosition * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Year   ? scaledTimePosition * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear :
                    0;

                //Execute the clock emulator on the transformed time position
                if (scaledTimePositionSeconds > 0)
                    TickForward(scaledTimePositionSeconds, coordinate, calendarTRS);
                else if (scaledTimePositionSeconds < 0)
                    TickBackward(scaledTimePositionSeconds, coordinate, calendarTRS);
            }
            #endregion

            return coordinate;
        }

        /// <summary>
        /// Converts a temporal coordinate (expressed in a calendar TRS) into a numeric time position (expressed in a positional TRS).
        /// This method performs the inverse operation of CoordinateFromPosition, calculating the temporal distance between the coordinate and the positional TRS origin,
        /// scaled according to the positional TRS unit
        /// </summary>
        public static double CoordinateToPosition(TIMECoordinate timeCoordinate, TIMEPositionReferenceSystem positionTRS, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (timeCoordinate == null)
                throw new OWLException($"Cannot convert coordinate to position because given '{nameof(timeCoordinate)}' parameter is null");
            if (positionTRS == null)
                throw new OWLException($"Cannot convert coordinate to position because given '{nameof(positionTRS)}' parameter is null");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            #region Utility
            double ConvertCoordinateToSeconds(TIMECoordinate coordinate)
            {
                double totalSeconds = 0;
    
                // Add years (traverse each year to handle leap years correctly)
                double currentYear = 0;
                double targetYear = coordinate.Year ?? 0;
    
                while (currentYear < targetYear)
                {
                    uint[] monthsInYear = calendarTRS.Metrics.LeapYearRule?.Invoke(currentYear) ?? calendarTRS.Metrics.Months;
                    double daysInYear = monthsInYear.Sum(m => m);
                    totalSeconds += daysInYear * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.SecondsInMinute;
                    currentYear++;
                }
    
                // Add months (traverse each month in the target year)
                uint[] monthsInTargetYear = calendarTRS.Metrics.LeapYearRule?.Invoke(targetYear) ?? calendarTRS.Metrics.Months;
                for (int month = 1; month < (coordinate.Month ?? 1); month++)
                {
                    totalSeconds += monthsInTargetYear[month - 1] * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.SecondsInMinute;
                }
    
                // Add remaining time components
                totalSeconds += ((coordinate.Day ?? 0) * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.SecondsInMinute);
                totalSeconds += ((coordinate.Hour ?? 0) * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.SecondsInMinute);
                totalSeconds += ((coordinate.Minute ?? 0) * calendarTRS.Metrics.SecondsInMinute);
                totalSeconds += (coordinate.Second ?? 0);
    
                return totalSeconds;
            }
            #endregion

            // Normalize both origin and input coordinate according to the metrics of the given calendar TRS
            TIMECoordinate normalizedOrigin = NormalizeCoordinate(positionTRS.Origin, calendarTRS);
            TIMECoordinate normalizedCoordinate = NormalizeCoordinate(timeCoordinate, calendarTRS);

            #region Large-Scale
            if (positionTRS.HasLargeScale)
            {
                // For large-scale, we work only at year level - use arithmetic calculation
                double originYear = normalizedOrigin.Year ?? 0;
                double coordinateYear = normalizedCoordinate.Year ?? 0;
                double yearsDifference = coordinateYear - originYear;
    
                // Convert years difference to the unit of the positional TRS
                double positionInTargetUnit =
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Year   ? yearsDifference :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Month  ? yearsDifference * calendarTRS.Metrics.MonthsInYear :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Day    ? yearsDifference * calendarTRS.Metrics.DaysInYear :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Hour   ? yearsDifference * calendarTRS.Metrics.DaysInYear * calendarTRS.Metrics.HoursInDay :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Minute ? yearsDifference * calendarTRS.Metrics.DaysInYear * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.MinutesInHour :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Second ? yearsDifference * calendarTRS.Metrics.DaysInYear * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.SecondsInMinute :
                    0;

                // Scale by the inverse of the unit's scale factor
                return positionInTargetUnit / positionTRS.Unit.ScaleFactor;
            }
            #endregion

            #region Little-Scale
            else
            {
                // Convert origin coordinate to total seconds from year 0
                double originSeconds = ConvertCoordinateToSeconds(normalizedOrigin);
    
                // Convert input coordinate to total seconds from year 0
                double coordinateSeconds = ConvertCoordinateToSeconds(normalizedCoordinate);
    
                // Calculate the difference in seconds
                double secondsDifference = coordinateSeconds - originSeconds;
    
                // Convert seconds difference to the unit of the positional TRS
                double positionInTargetUnit =
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Second ? secondsDifference :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Minute ? secondsDifference / calendarTRS.Metrics.SecondsInMinute :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Hour ? secondsDifference / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Day ? secondsDifference / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Month ? secondsDifference / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear)) :
                    positionTRS.Unit.UnitType == TIMEEnums.TIMEUnitType.Year ? secondsDifference / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear) :
                    0;
    
                // Scale by the inverse of the unit's scale factor
                return positionInTargetUnit / positionTRS.Unit.ScaleFactor;
            }
            #endregion
        }

        /// <summary>
        /// Ensures that a temporal coordinate has valid component values according to the metrics of a given calendar TRS,
        /// propagating overflows across all six dimensions (seconds → minutes → hours → days → months → years)
        /// to produce a canonicalized representation
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static TIMECoordinate NormalizeCoordinate(TIMECoordinate timeCoordinate, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (timeCoordinate == null)
                throw new OWLException($"Cannot normalize coordinate because given '{nameof(timeCoordinate)}' parameter is null");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            //Normalize second
            double coordinateSecond = timeCoordinate.Second ?? 0;
            double normalizedSecond = coordinateSecond % calendarTRS.Metrics.SecondsInMinute;
            double remainingMinutes = Math.Truncate(coordinateSecond / calendarTRS.Metrics.SecondsInMinute);
            //Normalize minute
            double coordinateMinute = (timeCoordinate.Minute ?? 0) + remainingMinutes;
            double normalizedMinute = coordinateMinute % calendarTRS.Metrics.MinutesInHour;
            double remainingHours = Math.Truncate(coordinateMinute / calendarTRS.Metrics.MinutesInHour);
            //Normalize hour
            double coordinateHour = (timeCoordinate.Hour ?? 0) + remainingHours;
            double normalizedHour = coordinateHour % calendarTRS.Metrics.HoursInDay;
            double remainingDays = Math.Truncate(coordinateHour / calendarTRS.Metrics.HoursInDay);

            //Pre-Normalize month
            double coordinateMonth = timeCoordinate.Month ?? 0;
            if (coordinateMonth < 1) //Calendar month start at 1
                coordinateMonth = 1;
            double normalizedMonth = coordinateMonth % calendarTRS.Metrics.MonthsInYear;
            if (normalizedMonth == 0)
                normalizedMonth = calendarTRS.Metrics.MonthsInYear;
            double remainingYears = Math.Truncate(coordinateMonth / calendarTRS.Metrics.MonthsInYear);
            if (coordinateMonth % calendarTRS.Metrics.MonthsInYear == 0)
                remainingYears--;

            //Normalize day
            double coordinateDay = timeCoordinate.Day ?? 0;
            if (coordinateDay < 1) //Calendar day start at 1
                coordinateDay = 1;
            coordinateDay += remainingDays;
            uint[] metricsMonths = calendarTRS.Metrics.LeapYearRule?.Invoke((timeCoordinate.Year ?? 0) + remainingYears)
                                     ?? calendarTRS.Metrics.Months;
            double daysOfNormalizedMonth = metricsMonths[Convert.ToUInt32(normalizedMonth) - 1];
            double normalizedDay = coordinateDay % daysOfNormalizedMonth;
            if (normalizedDay == 0)
                normalizedDay = daysOfNormalizedMonth;
            double remainingMonths = Math.Truncate(coordinateDay / daysOfNormalizedMonth);
            if (coordinateDay % daysOfNormalizedMonth == 0)
                remainingMonths--;

            //Post-Normalize month
            while (remainingMonths > 0)
            {
                normalizedMonth += remainingMonths;
                remainingMonths = 0;
                double postNormalizedMonth = normalizedMonth % calendarTRS.Metrics.MonthsInYear;
                if (postNormalizedMonth == 0)
                    postNormalizedMonth = calendarTRS.Metrics.MonthsInYear;
                remainingYears += Math.Truncate(normalizedMonth / calendarTRS.Metrics.MonthsInYear);
                if (normalizedMonth % calendarTRS.Metrics.MonthsInYear == 0)
                    remainingYears--;
                normalizedMonth = postNormalizedMonth;

                //If normalizedDay exceeds metrics configured for normalizedMonth,
                //we need an extra iteration to further normalize the situation
                daysOfNormalizedMonth = metricsMonths[Convert.ToUInt32(normalizedMonth) - 1];
                if (normalizedDay > daysOfNormalizedMonth)
                {
                    normalizedDay = 1;
                    remainingMonths++;
                }
            }

            //Normalize year
            double normalizedYear = (timeCoordinate.Year ?? 0) + remainingYears;

            return new TIMECoordinate(
                Math.Truncate(normalizedYear),
                Math.Truncate(normalizedMonth),
                Math.Truncate(normalizedDay),
                Math.Truncate(normalizedHour),
                Math.Truncate(normalizedMinute),
                normalizedSecond) { Metadata = new TIMECoordinateMetadata(calendarTRS, RDFVocabulary.TIME.UNIT_SECOND) };
        }

        /// <summary>
        /// Converts a numeric temporal duration (expressed with a specific unit type) into a structured temporal extent
        /// with decomposed components (years, months, weeks, days, hours, minutes, seconds) according to calendar TRS metrics
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static TIMEExtent DurationToExtent(double timeDuration, TIMEUnit unitType, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (timeDuration < 0)
                throw new OWLException($"Cannot convert duration to extent because given '{nameof(timeDuration)}' parameter must be greater or equal than zero");
            if (unitType == null)
                throw new OWLException($"Cannot convert duration to extent because given '{nameof(unitType)}' parameter is null");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            TIMEExtent extent = new TIMEExtent { Metadata = new TIMEExtentMetadata(calendarTRS) };

            //Scale the given time duration of the factor specified by its unit type
            double scaledTimeDuration = timeDuration * unitType.ScaleFactor;

            //Transform the scaled time duration to seconds (since duration works at this level of detail)
            double timeDurationSeconds =
                unitType.UnitType == TIMEEnums.TIMEUnitType.Second ? scaledTimeDuration :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Minute ? scaledTimeDuration * calendarTRS.Metrics.SecondsInMinute :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Hour   ? scaledTimeDuration * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Day    ? scaledTimeDuration * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Month  ? scaledTimeDuration * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear) :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Year   ? scaledTimeDuration * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear :
                0;

            //Extract components of the time extent according to the metrics of the given calendar TRS
            //Calendarization (Months, Years) can be calculated only if the given calendar TRS has exact metric
            extent.Seconds = Math.Truncate(timeDurationSeconds  % calendarTRS.Metrics.SecondsInMinute);
            extent.Minutes = Math.Truncate((timeDurationSeconds / calendarTRS.Metrics.SecondsInMinute) % calendarTRS.Metrics.MinutesInHour);
            extent.Hours   = Math.Truncate((timeDurationSeconds / calendarTRS.Metrics.SecondsInMinute  / calendarTRS.Metrics.MinutesInHour) % calendarTRS.Metrics.HoursInDay);
            extent.Days    = Math.Truncate((timeDurationSeconds / calendarTRS.Metrics.SecondsInMinute  / calendarTRS.Metrics.MinutesInHour  / calendarTRS.Metrics.HoursInDay));
            extent.Months  = 0;
            extent.Weeks   = 0;
            extent.Years   = 0;
            if (calendarTRS.Metrics.HasExactMetric)
            {
                extent.Days %= (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear);
                extent.Months = Math.Truncate((timeDurationSeconds / calendarTRS.Metrics.SecondsInMinute / calendarTRS.Metrics.MinutesInHour / calendarTRS.Metrics.HoursInDay / (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear)) % calendarTRS.Metrics.MonthsInYear);
                extent.Years = Math.Truncate((timeDurationSeconds / calendarTRS.Metrics.SecondsInMinute / calendarTRS.Metrics.MinutesInHour / calendarTRS.Metrics.HoursInDay / calendarTRS.Metrics.DaysInYear) % calendarTRS.Metrics.DaysInYear);
            }

            return extent;
        }

        /// <summary>
        /// Converts a temporal extent (with decomposed components) into a numeric temporal duration expressed in a specific unit type.
        /// This method performs the inverse operation of ExtentFromDuration, aggregating all extent components into a single duration
        /// value scaled according to the specified unit.
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static double ExtentToDuration(TIMEExtent timeExtent, TIMEUnit unitType, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (timeExtent == null)
                throw new OWLException($"Cannot get duration from extent because given '{nameof(timeExtent)}' parameter is null");
            if (unitType == null)
                throw new OWLException($"Cannot get duration from extent because given '{nameof(unitType)}' parameter is null");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            //Transform the components to seconds (since duration works at this level of detail)
            double totalSeconds = timeExtent.Seconds ?? 0;
            totalSeconds += ((timeExtent.Minutes     ?? 0) * calendarTRS.Metrics.SecondsInMinute);
            totalSeconds += ((timeExtent.Hours       ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour);
            totalSeconds += ((timeExtent.Days        ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay);
            totalSeconds += ((timeExtent.Weeks       ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * TIMEUnit.Week.ScaleFactor);
            totalSeconds += ((timeExtent.Months      ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear));
            totalSeconds += ((timeExtent.Years       ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear);

            //Convert seconds to the target unit type
            double durationInTargetUnit =
                unitType.UnitType == TIMEEnums.TIMEUnitType.Second ? totalSeconds :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Minute ? totalSeconds / calendarTRS.Metrics.SecondsInMinute :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Hour   ? totalSeconds / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour) :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Day    ? totalSeconds / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay) :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Month  ? totalSeconds / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear)) :
                unitType.UnitType == TIMEEnums.TIMEUnitType.Year   ? totalSeconds / (calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear) :
                0;

            //Scale by the inverse of the unit's scale factor
            return durationInTargetUnit / unitType.ScaleFactor;
        }

        /// <summary>
        /// Reduces a temporal extent to its canonical form by converting all components to seconds,
        /// then redistributing them into normalized components, suppressing inexact calendar units
        /// (years, months, weeks) that cannot be precisely represented
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static TIMEExtent NormalizeExtent(TIMEExtent timeExtent, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (timeExtent == null)
                throw new OWLException($"Cannot normalize extent because given '{nameof(timeExtent)}' parameter is null");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            //Transform the components to seconds (since duration works at this level of detail)
            double timeExtentSeconds = timeExtent.Seconds ?? 0;
            timeExtentSeconds += ((timeExtent.Minutes     ?? 0) * calendarTRS.Metrics.SecondsInMinute);
            timeExtentSeconds += ((timeExtent.Hours       ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour);
            timeExtentSeconds += ((timeExtent.Days        ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay);
            timeExtentSeconds += ((timeExtent.Weeks       ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * TIMEUnit.Week.ScaleFactor);
            timeExtentSeconds += ((timeExtent.Months      ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear));
            timeExtentSeconds += ((timeExtent.Years       ?? 0) * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear);

            //Obtain an equivalent calendar TRS with forced inexact metrics: this is needed to suppress
            //eventual representation of Years, Months and Weeks (which all cumulate into Days)
            TIMECalendarReferenceSystem inexactCalendarTRS = new TIMECalendarReferenceSystem(calendarTRS,
                new TIMECalendarReferenceSystemMetrics(calendarTRS.Metrics.SecondsInMinute, calendarTRS.Metrics.MinutesInHour, calendarTRS.Metrics.HoursInDay, calendarTRS.Metrics.Months))
                {
                    Metrics = { LeapYearRule = null }
                };

            return DurationToExtent(timeExtentSeconds, TIMEUnit.Second, inexactCalendarTRS);
        }

        /// <summary>
        /// Calculates the temporal extent (duration) between two temporal coordinates by normalizing both,
        /// converting them to seconds, computing the difference, and returning the result as a structured extent
        /// according to calendar TRS metrics
        /// </summary>
        /// <exception cref="OWLException"></exception>
        public static TIMEExtent ExtentBetweenCoordinates(TIMECoordinate timeCoordinateStart, TIMECoordinate timeCoordinateEnd, TIMECalendarReferenceSystem calendarTRS=null)
        {
            #region Guards
            if (timeCoordinateStart == null)
                throw new OWLException($"Cannot get extent between coordinates because given '{nameof(timeCoordinateStart)}' parameter is null");
            if (timeCoordinateEnd == null)
                throw new OWLException($"Cannot get extent between coordinates because given '{nameof(timeCoordinateEnd)}' parameter is null");

            if (calendarTRS == null)
                calendarTRS = TIMECalendarReferenceSystem.Gregorian;
            #endregion

            //Normalize the given coordinates according to the metrics of the given calendar TRS
            TIMECoordinate normalizedStart = NormalizeCoordinate(timeCoordinateStart, calendarTRS);
            TIMECoordinate normalizedEnd = NormalizeCoordinate(timeCoordinateEnd, calendarTRS);

            //Determine if the extent would be negative: if so, just swap the parameters
            if (normalizedStart.CompareTo(normalizedEnd) > -1)
            {
                TIMECoordinate swapCoordinate = new TIMECoordinate(normalizedStart.Year, normalizedStart.Month, normalizedStart.Day,
                    normalizedStart.Hour, normalizedStart.Minute, normalizedStart.Second, new TIMECoordinateMetadata {
                        TRS = normalizedStart.Metadata.TRS, UnitType = normalizedStart.Metadata.UnitType });
                normalizedStart = normalizedEnd;
                normalizedEnd = swapCoordinate;
            }

            //Reduce start coordinate to seconds
            double normalizedStartSeconds = normalizedStart.Second.Value
                                             + (normalizedStart.Minute.Value * calendarTRS.Metrics.SecondsInMinute)
                                             + (normalizedStart.Hour.Value   * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour)
                                             + (normalizedStart.Day.Value    * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay)
                                             + (normalizedStart.Month.Value  * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear))
                                             + (normalizedStart.Year.Value   * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear);

            //Reduce end coordinate to seconds
            double normalizedEndSeconds =  normalizedEnd.Second.Value
                                             + (normalizedEnd.Minute.Value   * calendarTRS.Metrics.SecondsInMinute)
                                             + (normalizedEnd.Hour.Value     * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour)
                                             + (normalizedEnd.Day.Value      * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay)
                                             + (normalizedEnd.Month.Value    * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * (calendarTRS.Metrics.DaysInYear / calendarTRS.Metrics.MonthsInYear))
                                             + (normalizedEnd.Year.Value     * calendarTRS.Metrics.SecondsInMinute * calendarTRS.Metrics.MinutesInHour * calendarTRS.Metrics.HoursInDay * calendarTRS.Metrics.DaysInYear);

            //Return extent between start/end coordinates
            return DurationToExtent(normalizedEndSeconds - normalizedStartSeconds, TIMEUnit.Second, calendarTRS);
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Advances a temporal coordinate forward by a specified number of seconds using optimized batch processing,
        /// handling overflows across time dimensions (minutes → hours → days → months → years)
        /// with special consideration for variable month lengths
        /// </summary>
        internal static void TickForward(double secondsToConsume, TIMECoordinate timeCoordinate, TIMECalendarReferenceSystem calendarTRS)
        {
            uint[] metricsMonths = calendarTRS.Metrics.LeapYearRule?.Invoke(timeCoordinate.Year ?? 0)
                                    ?? calendarTRS.Metrics.Months;

            // Batch process: consume complete minutes
            if (secondsToConsume >= calendarTRS.Metrics.SecondsInMinute)
            {
                double minutesToAdd = Math.Floor(secondsToConsume / calendarTRS.Metrics.SecondsInMinute);
                secondsToConsume -= minutesToAdd * calendarTRS.Metrics.SecondsInMinute;
                timeCoordinate.Minute = (timeCoordinate.Minute ?? 0) + minutesToAdd;

                // Handle minute overflow -> hours
                if (timeCoordinate.Minute >= calendarTRS.Metrics.MinutesInHour)
                {
                    double hoursToAdd = Math.Floor(timeCoordinate.Minute.Value / calendarTRS.Metrics.MinutesInHour);
                    timeCoordinate.Minute = timeCoordinate.Minute.Value % calendarTRS.Metrics.MinutesInHour;
                    timeCoordinate.Hour = (timeCoordinate.Hour ?? 0) + hoursToAdd;

                    // Handle hour overflow -> days
                    if (timeCoordinate.Hour >= calendarTRS.Metrics.HoursInDay)
                    {
                        double daysToAdd = Math.Floor(timeCoordinate.Hour.Value / calendarTRS.Metrics.HoursInDay);
                        timeCoordinate.Hour = timeCoordinate.Hour.Value % calendarTRS.Metrics.HoursInDay;

                        // Handle day overflow -> months/years (this part needs iterative approach due to variable days per month)
                        while (daysToAdd > 0)
                        {
                            uint daysInCurrentMonth = metricsMonths[Convert.ToInt32(timeCoordinate.Month ?? 1) - 1];
                            double daysRemainingInMonth = daysInCurrentMonth - (timeCoordinate.Day ?? 1) + 1;

                            if (daysToAdd < daysRemainingInMonth)
                            {
                                timeCoordinate.Day = (timeCoordinate.Day ?? 1) + (uint)daysToAdd;
                                daysToAdd = 0;
                            }
                            else
                            {
                                daysToAdd -= daysRemainingInMonth;
                                timeCoordinate.Day = 1;
                                timeCoordinate.Month = (timeCoordinate.Month ?? 1) + 1;

                                // Handle month overflow -> year
                                if (timeCoordinate.Month > calendarTRS.Metrics.MonthsInYear)
                                {
                                    timeCoordinate.Month = 1;
                                    timeCoordinate.Year = (timeCoordinate.Year ?? 0) + 1;
                                    metricsMonths = calendarTRS.Metrics.LeapYearRule?.Invoke(timeCoordinate.Year ?? 0)
                                                     ?? calendarTRS.Metrics.Months;
                                }
                            }
                        }
                    }
                }
            }

            // Add remaining seconds
            timeCoordinate.Second = Math.Truncate(timeCoordinate.Second.Value + secondsToConsume);
        }

        /// <summary>
        /// Moves a temporal coordinate backward by a specified number of seconds using optimized batch processing,
        /// handling underflows across time dimensions (minutes → hours → days → months → years)
        /// with special consideration for variable month lengths
        /// </summary>
        internal static void TickBackward(double secondsToConsume, TIMECoordinate timeCoordinate, TIMECalendarReferenceSystem calendarTRS)
        {
            uint[] metricsMonths = calendarTRS.Metrics.LeapYearRule?.Invoke(timeCoordinate.Year ?? 0)
                                    ?? calendarTRS.Metrics.Months;

            // Batch process: consume complete minutes (backwards)
            if (secondsToConsume < 0)
            {
                double minutesToSubtract = Math.Ceiling(Math.Abs(secondsToConsume) / calendarTRS.Metrics.SecondsInMinute);
                secondsToConsume += minutesToSubtract * calendarTRS.Metrics.SecondsInMinute;
                timeCoordinate.Minute = (timeCoordinate.Minute ?? 0) - minutesToSubtract;

                // Handle minute underflow -> hours
                if (timeCoordinate.Minute < 0)
                {
                    double hoursToSubtract = Math.Ceiling(Math.Abs(timeCoordinate.Minute.Value) / calendarTRS.Metrics.MinutesInHour);
                    timeCoordinate.Minute = timeCoordinate.Minute.Value + (hoursToSubtract * calendarTRS.Metrics.MinutesInHour);
                    timeCoordinate.Hour = (timeCoordinate.Hour ?? 0) - hoursToSubtract;

                    // Handle hour underflow -> days
                    if (timeCoordinate.Hour < 0)
                    {
                        double daysToSubtract = Math.Ceiling(Math.Abs(timeCoordinate.Hour.Value) / calendarTRS.Metrics.HoursInDay);
                        timeCoordinate.Hour = timeCoordinate.Hour.Value + (daysToSubtract * calendarTRS.Metrics.HoursInDay);

                        // Handle day underflow -> months/years (iterative due to variable days per month)
                        while (daysToSubtract > 0)
                        {
                            double currentDay = timeCoordinate.Day ?? 1;
                            if (daysToSubtract < currentDay)
                            {
                                timeCoordinate.Day = currentDay - (uint)daysToSubtract;
                                daysToSubtract = 0;
                            }
                            else
                            {
                                daysToSubtract -= currentDay;
                                timeCoordinate.Month = (timeCoordinate.Month ?? 1) - 1;

                                // Handle month underflow -> year
                                if (timeCoordinate.Month == 0)
                                {
                                    timeCoordinate.Month = calendarTRS.Metrics.MonthsInYear;
                                    timeCoordinate.Year = (timeCoordinate.Year ?? 0) - 1;
                                    metricsMonths = calendarTRS.Metrics.LeapYearRule?.Invoke(timeCoordinate.Year ?? 0)
                                                     ?? calendarTRS.Metrics.Months;
                                }

                                timeCoordinate.Day = metricsMonths[Convert.ToInt32(timeCoordinate.Month ?? 1) - 1];
                            }
                        }
                    }
                }
            }

            // Add remaining seconds (which should be >= 0 at this point)
            timeCoordinate.Second = Math.Truncate(timeCoordinate.Second.Value + secondsToConsume);
        }
        #endregion
    }
}