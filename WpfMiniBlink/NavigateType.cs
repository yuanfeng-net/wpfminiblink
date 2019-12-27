namespace Miniblink
{
    public enum NavigateType
    {
        /// <summary>
        /// 链接
        /// </summary>
        LinkClick,
        /// <summary>
        /// 表单提交submit
        /// </summary>
        Submit,
        /// <summary>
        /// 前进或后退
        /// </summary>
        BackForward,
        /// <summary>
        /// 重新载入
        /// </summary>
        ReLoad,
        /// <summary>
        /// 表单重新提交resubmit
        /// </summary>
        ReSubmit,
        /// <summary>
        /// 其他
        /// </summary>
        Other,
        /// <summary>
        /// window.open引发
        /// </summary>
        WindowOpen,
        /// <summary>
        /// 拥有target=blank属性的a链接引发
        /// </summary>
        BlankLink
    }
}
