﻿using Middleware_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware_CoreWeb
{
    public class NavigatorItem
    {
        public menu_model FatherItem { get; set; }

        public IEnumerable<menu_model> sonMenuItem { get; set; }
    }
}