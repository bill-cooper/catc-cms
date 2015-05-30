using System;

namespace ceenq.org.Services
{
    public class Warning
    {
        /// <summary>
        ///     The error code that pertains to the error.
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        ///     An HTML human-readable description of the error. Any links in the code point to the Esv website, but you can manipulate them to point wherever you wish.
        /// </summary>
        public String Readable { get; set; }
    }
}