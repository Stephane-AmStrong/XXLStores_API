using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Application.Enums
{
    public enum Roles
    {
        [Description("Customer")]
        Customer = 1,

        [Description("Vendor")]
        Vendor = 2,

        [Description("Admin")]
        Admin = 3,

        [Description("SuperAdmin")]
        SuperAdmin = 4,
    }
}
