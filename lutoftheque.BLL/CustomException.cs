﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll
{
    public class CustomException : Exception
    {
        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
