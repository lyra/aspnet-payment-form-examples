using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Lyranetwork
{

    /// <summary>
    /// Enumeration of generic possible payment statuses.
    /// </summary>
    public enum PaymentStatus
    {
        ACCEPTED,
        PENDING,
        CANCELLED,
        FAILED        
    }
}