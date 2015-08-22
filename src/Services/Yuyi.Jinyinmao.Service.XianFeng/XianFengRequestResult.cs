namespace Yuyi.Jinyinmao.Service.XianFeng
{
    public class XianFengRequestResult
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the response string.
        /// </summary>
        /// <value>The response string.</value>
        public string ResponseString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="XianFengRequestResult"/> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }
    }
}
