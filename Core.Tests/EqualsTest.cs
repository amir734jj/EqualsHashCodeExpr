using AutoFixture;
using Core.Builders;
using Core.Tests.Helpers;
using Core.Tests.Models;
using Xunit;

namespace Core.Tests
{
    public class EqualsTest : IClassFixture<FixtureBuilderHelper>
    {
        private readonly Fixture _fixture;

        public EqualsTest(FixtureBuilderHelper fixtureBuilderHelper)
        {
            _fixture = fixtureBuilderHelper.BuildFixture();
        }
        
        [Fact]
        public void Test__Equals_False()
        {
            // Arrange
            var c1 = _fixture.Create<ParentModel>();
            var c2 = _fixture.Create<ParentModel>();

            // Act
            var result = new EqualsBuilder().BuildFunc<ParentModel>()(c1, c2);

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public void Test__Equals_True()
        {
            // Arrange
            var c1 = _fixture.Create<ParentModel>();
            var c2 = _fixture.Build<ParentModel>().FromSeed(_ => c1).Create();

            // Act
            var result = new EqualsBuilder().BuildFunc<ParentModel>()(c1, c2);

            // Assert
            Assert.True(result);
        }
    }
}