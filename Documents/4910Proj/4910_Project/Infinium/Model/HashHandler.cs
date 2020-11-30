using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infinium;

namespace Infinium.Model
{
    public static class HashHandler
    {
        public static string ToHexString(this IEnumerable<byte> hash)
             => String.Join("", hash.Select(x => $"{x:x2}"));
    }
}
