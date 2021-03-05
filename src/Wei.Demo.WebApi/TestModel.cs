using System;
using Wei.DapperExtension.Attributes;

namespace Wei.Demo.WebApi
{
    public class TestModelInt
    {
        //[Key] int/long  IdĬ��ʶ��Ϊ��������Id,���Բ���key���Ա��
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string Result { get; set; }
    }

    public class TestModelMultipeKey
    {

        /// <summary>
        /// ��������-1
        /// </summary>
        [Key(false)]
        public string TypeId { get; set; }

        /// <summary>
        /// ��������-2
        /// </summary>
        [Key(false)]
        public string Type { get; set; }

        public string MethodName { get; set; }
        public string Result { get; set; }
    }
}
