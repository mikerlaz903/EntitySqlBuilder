using EntitySqlBuilder;
using EntitySqlBuilder.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EntitySqlBuilderTests
{
    [TestClass]
    public class SqlBuilderTest
    {
        [ClassInitialize]
        public static void AssemblyInit(TestContext context)
        {
            new EntityBuilder("DOC_NEW")
                .ConfigureParameter("DID", typeof(int), true)
                .ConfigureParameter("DID1", typeof(int))
                .Build();
        }

        [TestMethod]
        public void GetUpdateSql_NoUpdatesOptionDontThrowException_ReturnNull()
        {
            var entity = Entity.GetEntity("DOC_NEW");
            var sqlBuilder = new SqlUpdateBuilder(entity);

            Assert.IsNull(sqlBuilder.UpdateSql);
        }
        [TestMethod]
        public void GetUpdateSql_NoUpdatesOptionThrowException_ThrowException()
        {
            var options = EntityUpdaterOptions.ThrowExceptionIfFieldMissing;

            var entity = Entity.GetEntity("DOC_NEW", options);
            Assert.ThrowsException<NoDataException>(() =>
            {
                _ = new SqlUpdateBuilder(entity);
            });
        }
        [TestMethod]
        public void GetUpdateSql_NoUpdatesWithKeyOptionThrowException_ThrowException()
        {
            var options = EntityUpdaterOptions.ThrowExceptionIfFieldMissing;

            var entity = Entity.GetEntity("DOC_NEW", options);
            entity.SetParam("DID", 123);
            Assert.ThrowsException<UpdatableFieldMissingException>(() =>
            {
                _ = new SqlUpdateBuilder(entity);
            });
        }


        [TestMethod]
        public void GetUpdateSql_UpdatesNoKeyOptionDontThrowException_ReturnNull()
        {
            var entity = Entity.GetEntity("DOC_NEW");
            entity.SetParam("DID1", 123);
            var sqlBuilder = new SqlUpdateBuilder(entity);

            Assert.IsNull(sqlBuilder.UpdateSql);
        }
        [TestMethod]
        public void GetUpdateSql_UpdatesNoKeyOptionThrowException_ThrowException()
        {
            var options = EntityUpdaterOptions.ThrowExceptionIfKeyMissing;

            var entity = Entity.GetEntity("DOC_NEW", options);
            entity.SetParam("DID1", 123);
            entity.SetParam("DID1", 1234);
            Assert.ThrowsException<KeyMissingException>(() =>
            {
                _ = new SqlUpdateBuilder(entity);
            });
        }

        [TestMethod]
        public void GetUpdateSql_CorrectData_CorrectUpdateSql()
        {
            var expectedSql = "update DOC_NEW set DID1 = 1234 where DID = 1";

            var entity = Entity.GetEntity("DOC_NEW");
            entity.SetParam("DID", 1);
            entity.SetParam("DID1", 123);
            entity.SetParam("DID1", 1234);

            var sqlBuilder = new SqlUpdateBuilder(entity);

            Assert.AreEqual(expectedSql, sqlBuilder.UpdateSql);
        }
    }
}
