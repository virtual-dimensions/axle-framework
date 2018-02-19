﻿using Axle.Verification;


namespace Axle.Conversion
{
    #if !NETSTANDARD
    [System.Serializable]
    #endif
    public sealed class ReverseConverter<TS, TD> : ITwoWayConverter<TS, TD>
    {
        private readonly ITwoWayConverter<TD, TS> _converter;

        public ReverseConverter(ITwoWayConverter<TD, TS> converter)
        {
            _converter = converter.VerifyArgument(nameof(converter)).IsNotNull().Value;
        }

        /// <inheritdoc />
        public TD Convert(TS source) => _converter.ConvertBack(source);

        /// <inheritdoc />
        public TS ConvertBack(TD source) => _converter.Convert(source);

        /// <inheritdoc />
        public IConverter<TD, TS> Invert() => _converter;

        /// <inheritdoc />
        public bool TryConvert(TS source, out TD target) => _converter.TryConvertBack(source, out target);

        /// <inheritdoc />
        public bool TryConvertBack(TD source, out TS target) => _converter.TryConvert(source, out target);
    }
}