﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.SharedLibrary.response
{
    public record Response(bool Flag = false, string Message = null! )
    {

    }
}
