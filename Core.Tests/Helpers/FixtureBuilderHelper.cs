using AutoFixture;
using Core.Tests.Models;

namespace Core.Tests.Helpers
{
    public class FixtureBuilderHelper
    {
        public Fixture BuildFixture()
        {
            var fixture = new Fixture();

            fixture.Customize<NestedModel>(x => x.With(m => m.ParentRef,
                fixture.Build<ParentModel>().With(z => z.NestedRef, new NestedModel()).Create()
            ));

            return fixture;
        }
    }
}