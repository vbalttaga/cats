// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestResult.cs" company="GalexStudio">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the RequestResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.Helpers
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The request result.
    /// </summary>
    public class RequestResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether result.
        /// </summary>
        public RequestResultType Result { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string RedirectURL { get; set; }

        /// <summary>
        /// Gets or sets the Error Fields.
        /// </summary>
        public List<string> ErrorFields { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public Dictionary<string,object> Data { get; set; }
    }
}
