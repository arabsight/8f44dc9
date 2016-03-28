using System;

namespace Expenses.Core.Shared
{
    public interface ITrackable
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }

        int CreatedBy { get; set; }
        int UpdatedBy { get; set; }
    }
}
