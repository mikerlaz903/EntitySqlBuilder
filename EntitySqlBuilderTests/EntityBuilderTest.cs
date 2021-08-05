using EntitySqlBuilder;
using EntitySqlBuilder.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntitySqlBuilderTests
{
    [TestClass]
    public class EntityBuilderTest
    {
        [ClassInitialize]
        public static void AssemblyInit(TestContext context)
        {
            new EntityBuilder("DOC")
                .ConfigureParameter("DID", typeof(int), true)
                .Build();
        }

        [TestMethod]
        public void BuildNewEntity_ConfigureParameter_ParameterSet()
        {
            var hasParam = Entity.GetEntity("DOC").HasParam("DID");
            Assert.IsTrue(hasParam);
        }

        [TestMethod]
        public void ConfigureParam_AfterBuild_ThrowException()
        {
            Assert.ThrowsException<ConfigureLockedParameterException>(() =>
            {
                new EntityBuilder("DOC")
                    .ConfigureParameter("DID", typeof(int), true);
            });
        }

        [TestMethod]
        public void ConfigureParam_SameNamed_ThrowException()
        {
            Assert.ThrowsException<DuplicateParameterNameException>(() =>
            {
                new EntityBuilder("NEWDOC")
                    .ConfigureParameter("DID", typeof(int), true)
                    .ConfigureParameter("DID", typeof(int), true);
            });
        }
    }
}
