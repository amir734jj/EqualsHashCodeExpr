using AutoFixture;
using Core.Tests.Helpers;
using Core.Tests.Models;
using Xunit;

namespace Core.Tests
{
    public class HashCodeTest : IClassFixture<FixtureBuilderHelper>
    {
        private readonly Fixture _fixture;

        public HashCodeTest(FixtureBuilderHelper fixtureBuilderHelper)
        {
            _fixture = fixtureBuilderHelper.BuildFixture();
        }
        
        [Fact]
        public void Test__HashCode_False()
        {
            // Arrange
            var c1 = _fixture.Create<ParentModel>();
            var c2 = _fixture.Create<ParentModel>();

            // Act
            var result = new HashCodeBuilder().BuildFunc<ParentModel>()(c1) == new HashCodeBuilder().BuildFunc<ParentModel>()(c2);

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public void Test__HashCode_True()
        {
            // Arrange
            var c1 = _fixture.Create<ParentModel>();
            var c2 = _fixture.Build<ParentModel>().FromSeed(_ => c1).Create();

            // Act
            var result = new HashCodeBuilder().BuildFunc<ParentModel>()(c1) == new HashCodeBuilder().BuildFunc<ParentModel>()(c2);

            // Assert
            Assert.True(result);
        }
    }
}