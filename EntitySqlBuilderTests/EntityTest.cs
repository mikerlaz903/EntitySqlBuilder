using EntitySqlBuilder;
using EntitySqlBuilder.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EntitySqlBuilderTests
{
    [TestClass]
    public class EntityTest
    {
        [ClassInitialize]
        public static void AssemblyInit(TestContext context)
        {
            new EntityBuilder("LIB")
                .ConfigureParameter("LID", typeof(int), true)
                .ConfigureParameter("LINT1", typeof(int))
                .ConfigureParameter("LNAME", typeof(string))
                .Build();
        }

        [TestMethod]
        public void SetCurrentValue_InitialValueUndefined_InitialValueEqualSettableValue()
        {
            var expectedString = "test";

            var entity = Entity.GetEntity("LIB");
            entity.SetParam("LNAME", expectedString);
            var result = entity.GetParam<string>("LNAME");

            Assert.AreEqual(result, expectedString);
        }

        [TestMethod]
        public void SetCurrentValue_InitialValueDefined_CurrentValueEqualSettableValue()
        {
            var expectedString = "test";
            var notExpectedString = "notExpectedString";

            var entity = Entity.GetEntity("LIB");
            entity.SetParam("LNAME", notExpectedString);
            entity.SetParam("LNAME", expectedString);
            var result = entity.GetParam<string>("LNAME");

            Assert.AreEqual(result, expectedString);
        }

        [TestMethod]
        public void SetCurrentValue_NewValueWrongType_RaiseException()
        {
            var entity = Entity.GetEntity("LIB");

            Assert.ThrowsException<ModifyingValueIncorrectTypeException>(() =>
            {
                entity.SetParam("LNAME", 18);
            });
        }

        [TestMethod]
        public void SetCurrentValue_NewKeyValue_RaiseException()
        {
            var entity = Entity.GetEntity("LIB");
            entity.SetParam("LID", 1);

            Assert.ThrowsException<ModifyingKeyValueException>(() =>
            {
                entity.SetParam("LID", 2);
            });
        }

        [TestMethod]
        public void SetCurrentValue_WrongParameterName_ThrowException()
        {
            var entity = Entity.GetEntity("LIB");
            Assert.ThrowsException<UndefinedParameterNameException>(() =>
            {
                entity.SetParam("L", 1);
            });
        }

        [TestMethod]
        public void SetCurrentValue_WrongCaseParameterWithIgnoreCaseOption_SetCurrentValue()
        {
            var expectedValue = 1;

            var options = EntityUpdaterOptions.IgnoreEntityAndParameterNameCase;
            var entity = Entity.GetEntity("LIB", options);
            entity.SetParam("lid", expectedValue);

            Assert.AreEqual(entity.GetParam<int>("lId"), expectedValue);
        }

        [TestMethod]
        public void SetCurrentValue_NewValueIsNull_SetCurrentValue()
        {
            object expectedValue = null;

            var entity = Entity.GetEntity("LIB");
            entity.SetParam("LNAME", expectedValue);

            Assert.IsNull(entity.GetParam<string>("LNAME"));
        }

        [TestMethod]
        public void SetCurrentValue_SetSameValue_NothingChanged()
        {
            var expectedValue = 1;

            var entity = Entity.GetEntity("LIB");
            entity.SetParam("LINT1", expectedValue);

            Assert.AreEqual(entity.GetParam<int>("LINT1"), expectedValue);

            entity.SetParam("LINT1", expectedValue);

            Assert.AreEqual(entity.GetParam<int>("LINT1"), expectedValue);
        }
        [TestMethod]
        public void SetCurrentValue_SetNull_NothingChanged()
        {
            var entity = Entity.GetEntity("LIB");
            entity.SetParam("LINT1", null);
            entity.SetParam("LINT1", null);

            Assert.IsFalse(entity.IsModified("LINT1"));
        }
    }
}
