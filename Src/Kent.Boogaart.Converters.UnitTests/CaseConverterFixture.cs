namespace Kent.Boogaart.Converters.UnitTests
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using Xunit;

    public sealed class CaseConverterFixture
    {
        [Fact]
        public void ctor_sets_source_casing_to_normal()
        {
            var converter = new CaseConverter();
            Assert.Equal(CharacterCasing.Normal, converter.SourceCasing);
        }

        [Fact]
        public void ctor_sets_target_casing_to_normal()
        {
            var converter = new CaseConverter();
            Assert.Equal(CharacterCasing.Normal, converter.TargetCasing);
        }

        [Fact]
        public void ctor_that_takes_single_casing_should_set_source_casing_and_target_casing_to_that_casing()
        {
            var converter = new CaseConverter(CharacterCasing.Upper);
            Assert.Equal(CharacterCasing.Upper, converter.SourceCasing);
            Assert.Equal(CharacterCasing.Upper, converter.TargetCasing);

            converter = new CaseConverter(CharacterCasing.Lower);
            Assert.Equal(CharacterCasing.Lower, converter.SourceCasing);
            Assert.Equal(CharacterCasing.Lower, converter.TargetCasing);
        }

        [Fact]
        public void ctor_that_takes_source_casing_and_target_casing_should_set_source_casing_and_target_casing()
        {
            var converter = new CaseConverter(CharacterCasing.Upper, CharacterCasing.Lower);
            Assert.Equal(CharacterCasing.Upper, converter.SourceCasing);
            Assert.Equal(CharacterCasing.Lower, converter.TargetCasing);
        }

        [Fact]
        public void source_casing_throws_if_invalid_value_is_assigned()
        {
            var converter = new CaseConverter();
            var ex = Assert.Throws<ArgumentException>(() => converter.SourceCasing = (CharacterCasing)100);
            Assert.Equal("Enum value '100' is not defined for enumeration 'System.Windows.Controls.CharacterCasing'.\r\nParameter name: value", ex.Message);
        }

        [Fact]
        public void target_casing_throws_if_invalid_value_is_assigned()
        {
            var converter = new CaseConverter();
            var ex = Assert.Throws<ArgumentException>(() => converter.TargetCasing = (CharacterCasing)100);
            Assert.Equal("Enum value '100' is not defined for enumeration 'System.Windows.Controls.CharacterCasing'.\r\nParameter name: value", ex.Message);
        }

        [Fact]
        public void casing_throws_if_invalid_value_is_assigned()
        {
            var converter = new CaseConverter();
            var ex = Assert.Throws<ArgumentException>(() => converter.Casing = (CharacterCasing)100);
            Assert.Equal("Enum value '100' is not defined for enumeration 'System.Windows.Controls.CharacterCasing'.\r\nParameter name: value", ex.Message);
        }

        [Fact]
        public void casing_sets_both_source_casing_and_target_casing()
        {
            var converter = new CaseConverter();
            Assert.Equal(CharacterCasing.Normal, converter.SourceCasing);
            Assert.Equal(CharacterCasing.Normal, converter.TargetCasing);

            converter.Casing = CharacterCasing.Upper;
            Assert.Equal(CharacterCasing.Upper, converter.SourceCasing);
            Assert.Equal(CharacterCasing.Upper, converter.TargetCasing);

            converter.Casing = CharacterCasing.Lower;
            Assert.Equal(CharacterCasing.Lower, converter.SourceCasing);
            Assert.Equal(CharacterCasing.Lower, converter.TargetCasing);
        }

        [Fact]
        public void convert_return_unset_value_if_value_is_not_a_string()
        {
            var converter = new CaseConverter();
            Assert.Same(DependencyProperty.UnsetValue, converter.Convert(123, null, null, null));
            Assert.Same(DependencyProperty.UnsetValue, converter.Convert(123d, null, null, null));
            Assert.Same(DependencyProperty.UnsetValue, converter.Convert(DateTime.Now, null, null, null));
        }

        [Fact]
        public void convert_returns_same_value_if_target_casing_is_normal()
        {
            var converter = new CaseConverter
            {
                TargetCasing = CharacterCasing.Normal
            };

            Assert.Equal("abcd", converter.Convert("abcd", null, null, null));
            Assert.Equal("ABCD", converter.Convert("ABCD", null, null, null));
            Assert.Equal("AbCd", converter.Convert("AbCd", null, null, null));
        }

        [Fact]
        public void convert_returns_lower_cased_value_if_target_casing_is_lower()
        {
            var converter = new CaseConverter
            {
                TargetCasing = CharacterCasing.Lower
            };

            Assert.Equal("abcd", converter.Convert("abcd", null, null, null));
            Assert.Equal("abcd", converter.Convert("ABCD", null, null, null));
            Assert.Equal("abcd", converter.Convert("AbCd", null, null, null));
        }

        [Fact]
        public void convert_returns_upper_cased_value_if_target_casing_is_upper()
        {
            var converter = new CaseConverter
            {
                TargetCasing = CharacterCasing.Upper
            };

            Assert.Equal("ABCD", converter.Convert("abcd", null, null, null));
            Assert.Equal("ABCD", converter.Convert("ABCD", null, null, null));
            Assert.Equal("ABCD", converter.Convert("AbCd", null, null, null));
        }

        [Fact]
        public void convert_uses_specified_culture_when_converting_to_lower_case()
        {
            var converter = new CaseConverter
            {
                TargetCasing = CharacterCasing.Lower
            };
            var cultureInfo = new CultureInfo("tr");

            Assert.Equal("ijk", converter.Convert("ijk", null, null, cultureInfo));
            Assert.Equal("ıjk", converter.Convert("IJK", null, null, cultureInfo));
            Assert.Equal("ijk", converter.Convert("iJk", null, null, cultureInfo));
        }

        [Fact]
        public void convert_uses_specified_culture_when_converting_to_upper_case()
        {
            var converter = new CaseConverter
            {
                TargetCasing = CharacterCasing.Upper
            };
            var cultureInfo = new CultureInfo("tr");

            converter.TargetCasing = CharacterCasing.Upper;
            Assert.Equal("İJK", converter.Convert("ijk", null, null, cultureInfo));
            Assert.Equal("IJK", converter.Convert("IJK", null, null, cultureInfo));
            Assert.Equal("İJK", converter.Convert("iJk", null, null, cultureInfo));
        }

        [Fact]
        public void convert_back_returns_unset_value_if_value_is_not_a_string()
        {
            var converter = new CaseConverter();
            Assert.Same(DependencyProperty.UnsetValue, converter.ConvertBack(123, null, null, null));
            Assert.Same(DependencyProperty.UnsetValue, converter.ConvertBack(123d, null, null, null));
            Assert.Same(DependencyProperty.UnsetValue, converter.ConvertBack(DateTime.Now, null, null, null));
        }

        [Fact]
        public void convert_back_returns_same_value_if_source_casing_is_normal()
        {
            var converter = new CaseConverter
            {
                SourceCasing = CharacterCasing.Normal
            };

            Assert.Equal("abcd", converter.ConvertBack("abcd", null, null, null));
            Assert.Equal("ABCD", converter.ConvertBack("ABCD", null, null, null));
            Assert.Equal("AbCd", converter.ConvertBack("AbCd", null, null, null));
        }

        [Fact]
        public void convert_back_returns_lower_cased_value_if_source_casing_is_lower()
        {
            var converter = new CaseConverter
            {
                SourceCasing = CharacterCasing.Lower
            };

            Assert.Equal("abcd", converter.ConvertBack("abcd", null, null, null));
            Assert.Equal("abcd", converter.ConvertBack("ABCD", null, null, null));
            Assert.Equal("abcd", converter.ConvertBack("AbCd", null, null, null));
        }

        [Fact]
        public void convert_back_returns_upper_cased_value_if_source_casing_is_upper()
        {
            var converter = new CaseConverter
            {
                SourceCasing = CharacterCasing.Upper
            };

            Assert.Equal("ABCD", converter.ConvertBack("abcd", null, null, null));
            Assert.Equal("ABCD", converter.ConvertBack("ABCD", null, null, null));
            Assert.Equal("ABCD", converter.ConvertBack("AbCd", null, null, null));
        }

        [Fact]
        public void convert_back_uses_specified_culture_when_converting_to_lower_case()
        {
            var converter = new CaseConverter
            {
                SourceCasing = CharacterCasing.Lower
            };
            var cultureInfo = new CultureInfo("tr");

            Assert.Equal("ijk", converter.ConvertBack("ijk", null, null, cultureInfo));
            Assert.Equal("ıjk", converter.ConvertBack("IJK", null, null, cultureInfo));
            Assert.Equal("ijk", converter.ConvertBack("iJk", null, null, cultureInfo));
        }

        [Fact]
        public void convert_back_uses_specified_culture_when_converting_to_upper_case()
        {
            var converter = new CaseConverter
            {
                SourceCasing = CharacterCasing.Upper
            };
            var cultureInfo = new CultureInfo("tr");

            Assert.Equal("İJK", converter.ConvertBack("ijk", null, null, cultureInfo));
            Assert.Equal("IJK", converter.ConvertBack("IJK", null, null, cultureInfo));
            Assert.Equal("İJK", converter.ConvertBack("iJk", null, null, cultureInfo));
        }
    }
}
