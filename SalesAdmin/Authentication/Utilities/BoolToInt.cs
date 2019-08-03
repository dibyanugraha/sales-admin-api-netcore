
namespace SalesAdmin.Authentication.Utilities
{
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BoolToInt : ValueConverter<bool, int>
    {
        public BoolToInt() : base(
            x => Convert.ToInt32(x),
            y => Convert.ToBoolean(y))
        {

        }
    }
}
