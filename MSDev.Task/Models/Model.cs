using System;

namespace MSDev.Task.Models
{
    public class Model
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public Int32 Status { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedTime { get; set; }
    }
}
