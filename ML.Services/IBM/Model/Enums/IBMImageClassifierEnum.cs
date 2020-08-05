using System.ComponentModel;

namespace ML.Services.IBM.Model.Enums
{
    public enum IBMImageClassifierEnum
    {
        [Description("default")]
        Default = 1,
        [Description("explicit")]
        ExplicitSex = 2,
        [Description("food")]
        Food = 3,
        [Description("person")]
        Person = 4,
        [Description("not explicit")]
        NotExplicitSex = 5,

    }
}
