using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace AETest.WebAPI.Tests
{
    /// <summary>
    /// Specifically customizes linq2db POCO objects to exclude direct (one-to-one) assoiations
    /// </summary>
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() =>
            {
                return new Fixture().Customize(new AutoMoqCustomization());
            })
        {
        }
    }
    /// <summary>
    /// To be used instead Inline Attribute, but when required to enable AutoFixure+xUnit objects creation
    /// </summary>
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects)
            : base(new AutoMoqDataAttribute(), objects) { }
    }
}
