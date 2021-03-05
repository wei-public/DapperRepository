using System;
using Wei.DapperExtension.Attributes;

namespace Wei.Demo.WebApi
{
    public class TestModelInt
    {
        //[Key] int/long  Id默认识别为主键自增Id,可以不用key特性标记
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string Result { get; set; }
    }

    public class TestModelMultipeKey
    {

        /// <summary>
        /// 联合主键-1
        /// </summary>
        [Key(false)]
        public string TypeId { get; set; }

        /// <summary>
        /// 联合主键-2
        /// </summary>
        [Key(false)]
        public string Type { get; set; }

        public string MethodName { get; set; }
        public string Result { get; set; }
    }
}
