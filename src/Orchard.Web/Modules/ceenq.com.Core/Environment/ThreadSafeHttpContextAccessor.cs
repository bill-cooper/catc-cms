using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ceenq.com.Core.Environment
{
    public static class ThreadSafeHttpContextAccessor
    {
        private static HttpContext _context;
        public static void SetContext(HttpContext context)
        {
            _context = context;
        }
        public static HttpContext GetContext()
        {
            return _context;
        }
    }
}