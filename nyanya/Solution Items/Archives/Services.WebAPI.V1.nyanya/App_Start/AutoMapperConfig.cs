// FileInformation: nyanya/Services.WebAPI.V1.nyanya/AutoMapperConfig.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/11   4:21 AM

using System;
using AutoMapper;

namespace Services.WebAPI.V1.nyanya.App_Start
{
    /// <summary>
    ///     AutoMapperConfig
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        ///     RegisterAllMaps
        /// </summary>
        public static void RegisterAllMaps()
        {
            Mapper.CreateMap<DateTime?, DateTime>().ConvertUsing<NullableDateTimeConverter>();

            Mapper.CreateMap<string, string>().ConvertUsing<NullableStringConverter>();

            Mapper.AssertConfigurationIsValid();
        }
    }

    /// <summary>
    ///     NullableConverter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullableConverter<T> : TypeConverter<Nullable<T>, T> where T : struct, IComparable
    {
        protected override T ConvertCore(T? source)
        {
            return !source.HasValue ? default(T) : source.Value;
        }
    }

    /// <summary>
    ///     NullableDatetimeConverter
    /// </summary>
    public class NullableDateTimeConverter : TypeConverter<DateTime?, DateTime>
    {
        /// <summary>
        ///     When overridden in a base class, this method is provided the casted source object
        ///     extracted from the <see cref="T:AutoMapper.ResolutionContext" />
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Destination object</returns>
        protected override DateTime ConvertCore(DateTime? source)
        {
            return source ?? DateTime.Now.AddYears(-10);
        }
    }

    /// <summary>
    ///     NullableDecimalConverter
    /// </summary>
    public class NullableDecimalConverter : TypeConverter<decimal?, decimal>
    {
        /// <summary>
        ///     When overridden in a base class, this method is provided the casted source object
        ///     extracted from the <see cref="T:AutoMapper.ResolutionContext" />
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Destination object</returns>
        protected override decimal ConvertCore(decimal? source)
        {
            return source ?? 0;
        }
    }

    /// <summary>
    ///     NullableIntConverter
    /// </summary>
    public class NullableIntConverter : TypeConverter<int?, int>
    {
        /// <summary>
        ///     When overridden in a base class, this method is provided the casted source object
        ///     extracted from the <see cref="T:AutoMapper.ResolutionContext" />
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Destination object</returns>
        protected override int ConvertCore(int? source)
        {
            return source ?? 0;
        }
    }

    /// <summary>
    ///     NullableStringConverter
    /// </summary>
    public class NullableStringConverter : TypeConverter<string, string>
    {
        /// <summary>
        ///     When overridden in a base class, this method is provided the casted source object
        ///     extracted from the <see cref="T:AutoMapper.ResolutionContext" />
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Destination object</returns>
        protected override string ConvertCore(string source)
        {
            return source ?? "";
        }
    }
}